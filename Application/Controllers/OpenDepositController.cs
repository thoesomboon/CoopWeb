using System;
using Coop.Entities;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
//using Coop.Resources;

using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Coop.Library;
using Coop.Controllers;

//using Microsoft.Ajax.Utilities;
//using System.Globalization;
//using System.Web.UI;

namespace Coop.Controllers
{
    public class OpenDepositController : Controller
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public OpenDepositController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            ViewBag.DepositTypeList = new SelectList(GetDepositTypeList(), "DepositTypeID", "DepositTypeName");
            ViewBag.TxnCodeList = new SelectList(GetTxnCodeList(), "TTxnCode", "Descript");
            return View();
        }
        public List<DepositType> GetDepositTypeList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<DepositType> DepositTypes = db.DepositType.Where(d => d.Filestatus == "A").ToList();
            return DepositTypes;
        }
        public List<TxnCode> GetTxnCodeList()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<TxnCode> TxnCodes = db.TxnCode.Where(p => p.Filestatus == FileStatus.A && p.ProgramName == "OpenDeposit").ToList();
            //List<TxnCode> TxnCodes = db.TTxnCode.Where(p => p.TxnType == TxnType.DEP && p.Filestatus == FileStatus.A && p.ProgramName == Otx.DEP).ToList();
            return TxnCodes;
        }
        [Authorization]
         public ActionResult TransferInMemberOpen(OpenDepositModel openModel)
        {
            if (openModel == null || openModel.Equals(new OpenDepositModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new OpenDepositModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            OpenDepositModel model = openModel;

            /// check Member
            if (string.IsNullOrWhiteSpace(model.MemberID))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Invalid Member Code" };
                return Json(new object[] { oper, new OpenDepositModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            /// check if exist
            var mID = model.MemberID;

            /// get Bank Account No.
            /// 
            //var memData = _unitOfWork.Member.ReadDetail(pCode).FirstOrDefault();
            bool result = false;
            string msg = string.Empty;
            var memData = _unitOfWork.Member.ReadMember(mID);
            /// Account Exist
            if (memData != null && !string.IsNullOrWhiteSpace(memData.MemberID))
            {          
                result = true;
                model.Name = memData.TitleName + " " + memData.Name;
                model.Address = memData.Address + " " + memData.SubDistrictName + " " + memData.DistrictName + " " + memData.ProvinceName + " " + memData.PostalCode;
                //model.MemberGroupName = memData.MemberGroupID + " " + memData.MemberGroupName;
                //model.MemberTypeName = memData.MemberTypeID + " " + memData.MemberTypeName;
                model.BirthDate = memData.BirthDate;
                model.ApplyDate = memData.ApplyDate;
                model.IdCard = memData.IdCard;
                model.Telephone = memData.Telephone;
                model.Mobile = memData.Mobile;
                var depTypeData = _unitOfWork.DepositType.ReadDetail(openModel.DepositTypeID).FirstOrDefault();
                model.MinOpenAmt = depTypeData.MinOpenAmt;
                model.MaxOpenAmt = depTypeData.MaxOpenAmt;
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
            object[] retObj = new object[] { oResult, model };

            return Json(retObj, "application/json", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult TransferOutOpenDeposit(OpenDepositModel OpenDepModel)
        {
            if (OpenDepModel == null || OpenDepModel.Equals(new OpenDepositModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new OpenDepositModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            OpenDepositModel model = OpenDepModel;

            bool result = false;
            string msg = string.Empty;
            var depTypeID = OpenDepModel.DepositTypeID;
            if (depTypeID == "S12")
            {
                depTypeID = "S06";
            }
            var depTypeData = _unitOfWork.DepositType.ReadDetail(depTypeID).FirstOrDefault();
            var AccNo = Coop.Library.Deposit.IssueAccountNo(depTypeData, 8);
            var BookNo = Coop.Library.Deposit.IssueBookNo(depTypeData, 5);

            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
            DateTime sysDate = (DateTime)coopData.SystemDate;

            var txnSeq = 1;
            var ttlfDeposit = _unitOfWork.TtlfDeposit.ReadBySeqDesc(sysDate).FirstOrDefault();
            if (ttlfDeposit != null)
            {
                txnSeq = ttlfDeposit.TxnSeq + 1;
            }

            // Create new
            DepositModel dModel = new DepositModel
            {
                Filestatus = "A",
                CoopID = model.CoopID,
                AccountNo = AccNo,
                DepositTypeID = model.DepositTypeID,
                MemberID = model.MemberID,
                AccountName = model.AccountName,
                BookNo = BookNo,
                OpenDate = DateLib.DateInCE(model.TxnDateTH),
                LastContact = DateLib.DateInCE(model.TxnDateTH),
                LastCalcInt = DateLib.DateInCE(model.TxnDateTH),
                IntType = model.IntType,
                BFLedgerBal = 0,
                LedgerBal = model.Amt,
                AvailBal = model.Amt,
                BookBal = 0,
                AccInt = 0,
                LastBookLine = 0,
                BookPage = 1,
                BudgetYear = AuthorizeHelper.Current.CoopControls().BudgetYear,

                CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                CreatedDate = DateTime.Now,
                ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                ModifiedDate = DateTime.Now
            };
            var objCreate = _unitOfWork.Deposit.Create(dModel);
            if (objCreate != null && !objCreate.Equals(new OpenDepositModel()))
            {
                //model.AccountNo = objCreate.AccountNo;
                //var txnCode = _unitOfWork.TTxnCode.ReadDetailByType(model.TTxnCode, "DEP").FirstOrDefault();

                // Update DepositType
                result = false;
                var uDepositType = _unitOfWork.DepositType.ReadDetail(depTypeID).FirstOrDefault();
                if (uDepositType != null)
                {
                    /// update one
                    uDepositType.DepositTypeID = depTypeID;
                    uDepositType.LastAccountNo = (Convert.ToInt32(AccNo) / 10);
                    uDepositType.LastBookNo = Convert.ToInt32(BookNo);
                    //uDepositType.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
                    //uDepositType.ModifiedDate = System.DateTime.Now;
                    result = _unitOfWork.DepositType.UpdateIssueAccNo(uDepositType);
                }

                // Log NoBook
                var uNoBook = _unitOfWork.NoBook.ReadBySeqDesc(AccNo).FirstOrDefault();
                int iSeq = 1;
                if (uNoBook != null)
                {
                    iSeq = uNoBook.Seq + 1;
                };
                //int iItem = 0;
                //var txnCode = _unitOfWork.TTxnCode.ReadDetailByType(model.TTxnCode, "DEP").FirstOrDefault();
                //var depType = _unitOfWork.DepositType.ReadDetail(model.DepositTypeID).FirstOrDefault();
                //if (depType != null && depType.ItemStatus == true)
                //{
                //    iItem = 1;
                //};
                NoBookModel nbModel = new NoBookModel
                {
                    Filestatus = "A",
                    CoopID = OpenDepModel.CoopID,
                    AccountNo = AccNo,
                    Seq = iSeq,
                    TxnDate = DateLib.DateInCE(OpenDepModel.TxnDateTH),
                    BackDate = DateLib.DateInCE(OpenDepModel.TxnDateTH),
                    TTxnCode = OpenDepModel.TTxnCode,
                    AbbCode = OpenDepModel.AbbCode,
                    ItemNo = OpenDepModel.ItemNo,
                    CDCode = OpenDepModel.CDCode,
                    TxnAmt = OpenDepModel.Amt,
                    CfLedgerBal = OpenDepModel.Amt,
                    ChequeAmt = 0,
                    Tax = 0,
                    CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                    ModifiedDate = DateTime.Now,
                };
                //_unitOfWork.NoBook.Create(nbModel);
                var objCreateN = _unitOfWork.NoBook.Create(nbModel);
                if (objCreateN != null && !objCreateN.Equals(new NoBookModel()))
                {
                    msg = "ข้อมูลไม่ถูกต้อง";
                }

                // Log TtlfDeposit
                TtlfDepositModel tModel = new TtlfDepositModel
                {
                    CoopID = OpenDepModel.CoopID,
                    //TxnDate = DateLib.DateInCE(OpenDepModel.TxnDateTH),
                    TxnDate = sysDate,
                    TxnSeq = txnSeq,
                    TxnTime = DateTime.Now,
                    //WorkstationID = OpenDepModel.WorkstationID,
                    //BranchId = OpenDepModel.BranchId,
                    OriginalProcess = "OpenDeposit",
                    Filestatus = "A",
                    MemberID = OpenDepModel.MemberID,
                    DepositTypeID = OpenDepModel.DepositTypeID,
                    AccountNo = AccNo,
                    BackDate = DateLib.DateInCE(OpenDepModel.TxnDateTH),
                    BFLedgerBal = 0,
                    Debit = 0,
                    Credit = OpenDepModel.Amt,
                    CFLedgerBal = OpenDepModel.Amt,
                    Fee = OpenDepModel.Fee,
                    AccInt = 0,
                    ChargeAmt = 0,
                    IntDueAmt = 0,
                    //Tax = OpenDepModel.Tax,
                    ItemNo = OpenDepModel.ItemNo,
                    BookFlag = OpenDepModel.BookFlag,
                    ReferenceNo = OpenDepModel.ReferenceNo,
                    BudgetYear = AuthorizeHelper.Current.CoopControls().BudgetYear,
                    Type = OpenDepModel.DepositTypeID,
                    TTxnCode = OpenDepModel.TTxnCode,
                    CDCode = OpenDepModel.CDCode,
                    OCFlag = OpenDepModel.OCFlag,
                    InstrumentType = OpenDepModel.InstrumentType
                };
                _unitOfWork.TtlfDeposit.LogTtlfDeposit(tModel);
                // end
            }
            else
            {
                result = false;
                msg = "ข้อมูลไม่ถูกต้อง";
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
     }
}
