using Coop.Entities;
using Coop.Library;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace Coop.Controllers
{
    public class MemberController : BaseController
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public MemberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorization]
        public ActionResult Index()
        {
            ViewBag.TitleList = new SelectList(GetTitleList(), "TitleID", "TitleName");
            ViewBag.ProvinceList = new SelectList(GetProvinceList(), "ProvinceID", "ProvinceName");
            ViewBag.DistrictList = new SelectList(GetDistrictList(), "DistrictID", "DistrictName");
            ViewBag.SubDistrictList = new SelectList(GetSubDistrictList(), "SubDistrictID", "SubDistrictName");
            ViewBag.MemberTypeList = new SelectList(GetMemberTypeList(), "MemberTypeID", "MemberTypeName");
            ViewBag.MemberGroupList = new SelectList(GetMemberGroupList(), "MemberGroupID", "MemberGroupName");
            var model = new MemberModel();

            return View(model);
        }
        [Authorization]
        [HttpPost]
        public ActionResult TransferOutMember(MemberModel memModel)
        {
            if (memModel == null || memModel.Equals(new MemberModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new MemberModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            MemberModel model = memModel;
            var id = model.MemberID;

            bool result = false;
            string msg = string.Empty;

            if (id != null)
            {
                /// Account Exist => update one
                var uMember = _unitOfWork.Member.ReadDetail(id).FirstOrDefault();

                if (uMember != null && !string.IsNullOrWhiteSpace(uMember.MemberID))
                {
                    //uMember.MemberID = model.MemberID, /// primary key ==> identity
                    uMember.CoopID = model.CoopID;
                    uMember.MemberID = model.MemberID;
                    uMember.TitleID = model.TitleID;
                    uMember.Name = model.Name;
                    uMember.Address = model.Address;
                    uMember.SubDistrictID = model.SubDistrictID;
                    uMember.DistrictID = model.DistrictID;
                    uMember.ProvinceID = model.ProvinceID;
                    uMember.PostalCode = model.PostalCode;
                    uMember.Telephone = model.Telephone;
                    uMember.Mobile = model.Mobile;
                    uMember.EMail = model.EMail;
                    uMember.IdCard = model.IdCard;
                    uMember.LineID = model.LineID;
                    uMember.BirthDate = model.BirthDate;
                    uMember.ApplyDate = model.ApplyDate;
                    uMember.ResignDate = model.ResignDate;
                    uMember.Salary = model.Salary;
                    uMember.MemberGroupID = model.MemberGroupID;
                    uMember.MemberTypeID = model.MemberTypeID;
                    uMember.Filestatus = model.Filestatus;
                    uMember.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
                    uMember.ModifiedDate = DateTime.Now;

                    using (TransactionScope tranScope = new TransactionScope())
                    {
                        try
                        {
                            _unitOfWork.Member.Update(uMember);

                            tranScope.Complete();
                            result = true;
                        }
                        catch (Exception exception)
                        {
                            result = false;
                            msg = "Transaction Roll backed due to some exception:" + exception.Message;
                        }
                    }
                }
            }
            else
            {
                /// Create new
                MemberModel mModel = new MemberModel
                {
                    CoopID = model.CoopID,
                    MemberID = model.MemberID,
                    TitleID = model.TitleID,
                    Name = model.Name,
                    Address = model.Address,
                    SubDistrictID = model.SubDistrictID,
                    DistrictID = model.DistrictID,
                    ProvinceID = model.ProvinceID,
                    PostalCode = model.PostalCode,
                    Telephone = model.Telephone,
                    Mobile = model.Mobile,
                    EMail = model.EMail,
                    IdCard = model.IdCard,
                    LineID = model.LineID,

                    BirthDate = model.BirthDate,
                    ApplyDate = model.ApplyDate,
                    ResignDate = model.ResignDate,

                    Salary = model.Salary,
                    MemberGroupID = model.MemberGroupID,
                    MemberTypeID = model.MemberTypeID,
                    Filestatus = model.Filestatus,
                    CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    ModifiedDate = DateTime.Now,
                };

                var objCreate = _unitOfWork.Member.Create(mModel);
                if (objCreate != null && !objCreate.Equals(new MemberModel()))
                {
                    model.MemberID = objCreate.MemberID;
                    result = true;
                }
                else
                {
                    result = false;
                    msg = "Data Not Found";
                }
            }

            OperationResult oResult = new OperationResult();
            if (result)
            {
                oResult.Result = result;
                oResult.Message = "Successful";
            }
            else
            {
                oResult.Result = result;
                oResult.Message = msg;
            }
            object[] retObj = new object[] { oResult, model };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
        public ActionResult TransferInMember(MemberModel memModel)
        {
            if (memModel == null || memModel.Equals(new MemberModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new MemberModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            MemberModel model = memModel;

            /// check Member
            if (string.IsNullOrWhiteSpace(model.MemberID))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Invalid Member Code" };
                return Json(new object[] { oper, new MemberModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            /// check if exist
            var mID = model.MemberID;

            /// get Bank Account No.
            /// 
            //var pInfo = _unitOfWork.Member.ReadDetail(pCode).FirstOrDefault();
            var pInfo = _unitOfWork.Member.ReadMember(mID);

            bool result = false;
            string msg = string.Empty;

            /// Account Exist
            if (pInfo != null && !string.IsNullOrWhiteSpace(pInfo.MemberID))
            {
                result = true;
            }
            else
            {
                result = false;
                msg = "Member Not Found";
            }
            OperationResult oResult = new OperationResult();
            if (result)
            {
                oResult.Result = result;
                oResult.Message = "Successful";
            }
            else
            {
                oResult.Result = result;
                oResult.Message = msg;
            }
            object[] retObj = new object[] { oResult, pInfo };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
        public ActionResult TransferInMemberOtx(MemberModel memModel)
        {
            if (memModel == null || memModel.Equals(new MemberModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new MemberModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            MemberModel model = memModel;

            /// check Member
            if (string.IsNullOrWhiteSpace(model.MemberID))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Invalid Member Code" };
                return Json(new object[] { oper, new MemberModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            /// check if exist
            var mID = model.MemberID;

            /// get Bank Account No.
            /// 
            //var pInfo = _unitOfWork.Member.ReadDetail(pCode).FirstOrDefault();
            var pInfo = _unitOfWork.Member.ReadMember(mID);
            pInfo.Name = pInfo.TitleName + " " + pInfo.Name;
            pInfo.Address = pInfo.Address + " " + pInfo.SubDistrictName + " " + pInfo.DistrictName + " " + pInfo.ProvinceName + " " + pInfo.PostalCode;
            pInfo.MemberGroupName = pInfo.MemberGroupID + " " + pInfo.MemberGroupName;
            pInfo.MemberTypeName = pInfo.MemberTypeID + " " + pInfo.MemberTypeName;
            bool result = false;
            string msg = string.Empty;

            /// Account Exist
            if (pInfo != null && !string.IsNullOrWhiteSpace(pInfo.MemberID))
            {
                result = true;
            }
            else
            {
                result = false;
                msg = "Member Not Found";
            }
            OperationResult oResult = new OperationResult();
            if (result)
            {
                oResult.Result = result;
                oResult.Message = "Successful";
            }
            else
            {
                oResult.Result = result;
                oResult.Message = msg;
            }
            object[] retObj = new object[] { oResult, pInfo };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
        public List<Title> GetTitleList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<Title> titles = db.Title.ToList();
            return titles;
        }
        public List<Province> GetProvinceList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<Province> provinces = db.Province.ToList();
            return provinces;
        }
        public List<District> GetDistrictList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<District> districts = db.District.ToList();
            return districts;
        }
        public List<SubDistrict> GetSubDistrictList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<SubDistrict> subDistricts = db.SubDistrict.ToList();
            return subDistricts;
        }
        public List<MemberGroup> GetMemberGroupList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<MemberGroup> memberGroups = db.MemberGroup.ToList();
            return memberGroups;
        }
        public List<MemberType> GetMemberTypeList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<MemberType> memberTypes = db.MemberType.ToList();
            return memberTypes;
        }
        public ActionResult GetSubDistrictListByCode(string PostCode)
        {
            CoopWebEntities db = new CoopWebEntities();
            List<SubDistrict> subDistricts = db.SubDistrict.Where(x=>x.PostalCode==PostCode).ToList();
            ViewBag.SubDistrictOptions = new SelectList(subDistricts, "SubDistrictID", "SubDistrictName");
            return PartialView("SubDistrictOptionPartial");
        }
    }
}
