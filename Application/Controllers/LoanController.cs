using Coop.Entities;
using Coop.Library;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
//using System.Web;
using System.Web.Mvc;
//using Microsoft.Ajax.Utilities;

namespace Coop.Controllers
{
    public class LoanController : BaseController
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public LoanController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: Loan
        public ActionResult Index()
        {
            ViewBag.LoanTypeList = new SelectList(GetLoanTypeList(), "LoanTypeID", "LoanTypeName");
            ViewBag.InstallMethodList = new SelectList(GetInstallMethodList(), "InstallMethodID", "InstallMethodName");
            ViewBag.ReasonList = new SelectList(GetReasonList(), "ReasonID", "ReasonName");
            //ViewBag.LoanDueList = new SelectList(GetLoanDueList(), "LoanID", "MemberID");
            return View();
        }
        [Authorization]
        [HttpPost]
        public ActionResult TransferOutLoan(LoanModel LonModel)
        {
            if (LonModel == null || LonModel.Equals(new LoanModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new LoanModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            LoanModel model = LonModel;
            var id = model.LoanID;

            bool result = false;
            string msg = string.Empty;

            var uLoan = _unitOfWork.Loan.ReadDetail(id).FirstOrDefault();

            if (uLoan != null)
            {
                /// Account Exist => update one

                if (uLoan != null && !string.IsNullOrWhiteSpace(uLoan.LoanID))
                {
                    uLoan.Filestatus = model.Filestatus;
                    uLoan.CoopID = model.CoopID;
                    uLoan.LoanID = model.LoanID;
                    uLoan.LoanTypeID = model.LoanTypeID;
                    uLoan.MemberID = model.MemberID;
                    uLoan.IntType = model.IntType;
                    uLoan.LoanAmt = model.LoanAmt;
                    uLoan.LoanBal = model.LoanBal;
                    uLoan.BFBal = model.BFBal;
                    uLoan.BFInt = model.BFInt;
                    uLoan.BFCharge = model.BFCharge;
                    uLoan.AccInt = model.AccInt;
                    uLoan.YTDAccInt = model.YTDAccInt;
                    uLoan.UnpayInt = model.UnpayInt;
                    uLoan.UnpayPrinciple = model.UnpayPrinciple;
                    uLoan.UnpayCharge = model.UnpayCharge;
                    uLoan.IntRate = model.IntRate;
                    uLoan.ReasonID = model.ReasonID;
                    uLoan.PayFlag = model.PayFlag;
                    uLoan.BFUnpayInt = model.BFUnpayInt;
                    uLoan.BFUnpayCharge = model.BFUnpayCharge;
                    uLoan.InstallAmt = model.InstallAmt;
                    uLoan.InstallMethodID = model.InstallMethodID;
                    uLoan.DiscIntFlag = model.DiscIntFlag;
                    uLoan.BFDiscInt = model.BFDiscInt;
                    uLoan.DiscInt = model.DiscInt;
                    uLoan.BFUnpayDiscInt = model.BFUnpayDiscInt;
                    uLoan.UnpayDiscInt = model.UnpayDiscInt;
                    uLoan.TmpUnpayInt = model.TmpUnpayInt;
                    uLoan.TmpUnpayPrinciple = model.TmpUnpayPrinciple;
                    uLoan.TmpUnpayCharge = model.TmpUnpayCharge;
                    uLoan.TmpDiscInt = model.TmpDiscInt;
                    uLoan.TmpMilkAmt = model.TmpMilkAmt;
                    //uLoan.LoanDate = model.LoanDate;
                    //uLoan.FirstInstallDate = model.FirstInstallDate;
                    //uLoan.LastContact = model.LastContact;
                    //uLoan.StartCalcInt = model.StartCalcInt;
                    //uLoan.LastCalcInt = model.LastCalcInt;
                    //uLoan.LastCalcCharge = model.LastCalcCharge;

                    uLoan.LoanDate = DateLib.DateInCE(model.LoanDateTH);
                    //uLoan.FirstInstallDate = DateLib.DateInCE(model.FirstInstallDateTH);
                    uLoan.LastContact = DateLib.DateInCE(model.LastContactTH);
                    uLoan.StartCalcInt = DateLib.DateInCE(model.StartCalcIntTH);
                    uLoan.LastCalcInt = DateLib.DateInCE(model.LastCalcIntTH);
                    uLoan.LastCalcCharge = DateLib.DateInCE(model.LastCalcChargeTH);
                  
                    uLoan.Filestatus = model.Filestatus;
                    uLoan.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
                    uLoan.ModifiedDate = DateTime.Now;

                    using (TransactionScope tranScope = new TransactionScope())
                    {
                        try
                        {
                            _unitOfWork.Loan.Update(uLoan);

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
                LoanModel mModel = new LoanModel
                {
                    Filestatus = model.Filestatus,
                    CoopID = model.CoopID,
                    LoanID = model.LoanID,
                    LoanTypeID = model.LoanTypeID,
                    MemberID = model.MemberID,
                    IntType = model.IntType,
                    LoanAmt = model.LoanAmt,
                    LoanBal = model.LoanBal,
                    BFBal = model.BFBal,
                    BFInt = model.BFInt,
                    BFCharge = model.BFCharge,
                    AccInt = model.AccInt,
                    YTDAccInt = model.YTDAccInt,
                    UnpayInt = model.UnpayInt,
                    UnpayPrinciple = model.UnpayPrinciple,
                    UnpayCharge = model.UnpayCharge,
                    IntRate = model.IntRate,
                    ReasonID = model.ReasonID,
                    PayFlag = model.PayFlag,
                    BFUnpayInt = model.BFUnpayInt,
                    BFUnpayCharge = model.BFUnpayCharge,
                    InstallAmt = model.InstallAmt,
                    InstallMethodID = model.InstallMethodID,
                    DiscIntFlag = model.DiscIntFlag,
                    BFDiscInt = model.BFDiscInt,
                    DiscInt = model.DiscInt,
                    BFUnpayDiscInt = model.BFUnpayDiscInt,
                    UnpayDiscInt = model.UnpayDiscInt,
                    TmpUnpayInt = model.TmpUnpayInt,
                    TmpUnpayPrinciple = model.TmpUnpayPrinciple,
                    TmpUnpayCharge = model.TmpUnpayCharge,
                    TmpDiscInt = model.TmpDiscInt,
                    TmpMilkAmt = model.TmpMilkAmt,
                    
                    //LoanDate = model.LoanDate,
                    //FirstInstallDate = model.FirstInstallDate,
                    //LastContact = model.LastContact,
                    //StartCalcInt = model.StartCalcInt,
                    //LastCalcInt = model.LastCalcInt,
                    //LastCalcCharge = model.LastCalcCharge,
                    LoanDate = DateLib.DateInCE(model.LoanDateTH),
                    //FirstInstallDate = DateLib.DateInCE(model.FirstInstallDateTH),
                    LastContact = DateLib.DateInCE(model.LastContactTH),
                    StartCalcInt = DateLib.DateInCE(model.StartCalcIntTH),
                    LastCalcInt = DateLib.DateInCE(model.LastCalcIntTH),
                    LastCalcCharge = DateLib.DateInCE(model.LastCalcChargeTH),
                    CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    ModifiedDate = DateTime.Now
                };

                var objCreate = _unitOfWork.Loan.Create(mModel);
                if (objCreate != null && !objCreate.Equals(new LoanModel()))
                {
                    model.LoanID = objCreate.LoanID;
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
        public ActionResult TransferInLoan(LoanModel LonModel)
        {
            if (LonModel == null || LonModel.Equals(new LoanModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "ไม่พบข้อมูล" };
                return Json(new object[] { oper, new LoanModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            LoanModel model = LonModel;

            /// check Loan
            if (string.IsNullOrWhiteSpace(model.LoanID))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "เลขที่สัญญาเงินกู้ไม่ถูกต้อง" };
                return Json(new object[] { oper, new LoanModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            /// check if exist
            var mID = model.LoanID;

            /// get Bank Account No.
            /// 
            //var pInfo = _unitOfWork.Loan.ReadDetail(pCode).FirstOrDefault();
            var pInfo = _unitOfWork.Loan.ReadLoanInfo(mID);

            bool result = false;
            string msg = string.Empty;

            /// Account Exist
            if (pInfo != null && !string.IsNullOrWhiteSpace(pInfo.LoanID))
            {
                result = true;
            }
            else
            {
                result = false;
                msg = "Loan Not Found";
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
        public List<LoanType> GetLoanTypeList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<LoanType> loanTypes = db.LoanType.ToList();
            return loanTypes;
        }
        public List<InstallMethod> GetInstallMethodList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<InstallMethod> installMethods = db.InstallMethod.ToList();
            return installMethods;
        }
        public List<Reason> GetReasonList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<Reason> reasons = db.Reason.ToList();
            return reasons;
        }
        //public List<LoanDue> GetLoanDueList()
        //{
        //    CoopWebEntities db = new CoopWebEntities();
        //    List<LoanDue> loanDues = db.LoanDue.Where(l=>l.LoanID == "ป00012547").ToList();
        //    return loanDues;
        //}
    }
}
