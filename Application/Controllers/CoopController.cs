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

namespace Coop.Controllers
{
    public class CoopController : BaseController
    {
        // GET: Coop
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public CoopController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorization]
        [HttpPost]        
        public ActionResult ReadCoop()
        {
            var pInfo = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();

            bool result = false;
            string msg = string.Empty;

            /// Account Exist
            if (pInfo != null)
            {
                result = true;
            }
            else
            {
                result = false;
                msg = "Coop Not Found";
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
        // Start
        public ActionResult TransferInCoopControl(CoopControlModel copModel)
        {
            if (copModel == null || copModel.Equals(new CoopControlModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new CoopControlModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            CoopControlModel model = copModel;

            /// check Deposit
            if (model.CoopID > 0)
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Invalid Deposit Code" };
                return Json(new object[] { oper, new CoopControlModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }
            var copID = model.CoopID;
            var pInfo = _unitOfWork.CoopControl.ReadDetail(copID).FirstOrDefault();

            bool result = false;
            string msg = string.Empty;

            /// Account Exist
            if (pInfo != null)
            {
                result = true;
            }
            else
            {
                result = false;
                msg = "CoopControl Not Found";
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
        public ActionResult TransferOutCoopControl(CoopControlModel copModel)
        {
            if (copModel == null || copModel.Equals(new CoopControlModel()))
            {
                OperationResult oper = new OperationResult { Result = false, Message = "Model Empty" };
                return Json(new object[] { oper, new CoopControlModel() }, "application/json", JsonRequestBehavior.AllowGet);
            }

            CoopControlModel model = copModel;
            var copID = model.CoopID;

            bool result = false;
            string msg = string.Empty;

            var uCoopControl = _unitOfWork.CoopControl.ReadDetail(copID).FirstOrDefault();

            if (uCoopControl != null)
            {
                /// Account Exist => update one

                if (uCoopControl != null)
                {
                    uCoopControl.Filestatus = model.Filestatus;
                    uCoopControl.CoopID = model.CoopID;
                    uCoopControl.CoopName = model.CoopName;
                    uCoopControl.ManagerName = model.ManagerName;
                    uCoopControl.Address = model.Address;
                    uCoopControl.Province = model.Province;
                    uCoopControl.PostalCode = model.PostalCode;
                    uCoopControl.Telephone = model.Telephone;
                    uCoopControl.Fax = model.Fax;
                    uCoopControl.PrevSystemDate = DateLib.DateInCE(model.PrevSystemDateTH);
                    uCoopControl.SystemDate = DateLib.DateInCE(model.SystemDateTH);
                    uCoopControl.NextSystemDate = DateLib.DateInCE(model.NextSystemDateTH);
                    uCoopControl.BudgetYear = model.BudgetYear;
                    uCoopControl.StartBudgetDate = DateLib.DateInCE(model.StartBudgetDateTH);
                    uCoopControl.EndBudgetDate = DateLib.DateInCE(model.EndBudgetDateTH);
                    uCoopControl.PrevBudgetYear = model.PrevBudgetYear;
                    uCoopControl.PrevStartBudgetDate = DateLib.DateInCE(model.PrevStartBudgetDateTH);
                    uCoopControl.PrevEndBudgetDate = DateLib.DateInCE(model.PrevEndBudgetDateTH);
                    uCoopControl.ManagerName = model.ManagerName;
                    uCoopControl.AccountPeriod = model.AccountPeriod;
                    uCoopControl.DaysINYear = model.DaysINYear;
                    uCoopControl.RoundIntMethod = model.RoundIntMethod;
                    uCoopControl.LastReceiptBookNo = model.LastReceiptBookNo;
                    uCoopControl.LastReceiptRunNo = model.LastReceiptRunNo;

                    uCoopControl.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
                    uCoopControl.ModifiedDate = DateTime.Now;

                    using (TransactionScope tranScope = new TransactionScope())
                    {
                        try
                        {
                            _unitOfWork.CoopControl.Update(uCoopControl);

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
    //End
}