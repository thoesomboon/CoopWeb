using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using Coop.Library;
using Coop.Infrastructure.ActionFilters;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Coop.Controllers
{
    public class BatTrfMilkPaymentController : BaseController
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;

        public BatTrfMilkPaymentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            //OleDbConnection Econ;
        }
        // GET: /BatTrfMilkPayment/
        [HttpGet]
        public ActionResult Index()
        {
            var coopData = _unitOfWork.CoopControl.ReadDetail().FirstOrDefault();
            var model = new BatTrfMilkPaymentModel
            {
                CoopID = coopData.CoopID,
                CalcDate = coopData.SystemDate,
                //UserId = AuthorizeHelper.Current.UserAccount().UserID
                //WorkStationId = AuthorizeHelper.Current.CoopGETMachine().WorkStationID
            };
            return View(model);
        }
        [Authorization]
        //public JsonResult ProcessBatTrfMilkPayment(int coopId, DateTime cDate, string budgetYear)
        //{
        //    var userId = AuthorizeHelper.Current.UserAccount().UserID;
        //    var workId = "A01";
        //    _unitOfWork.MonthBalanceDeposit.sp_BatTrfMilkPayment(coopId, cDate, userId, workId);
        //    return Json("** ประมวณผลเสร็จแล้ว **", JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase postedFile)
        {
            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                //Create a DataTable.
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[6] { new DataColumn("MemberID", typeof(string)),
                                new DataColumn("startDate", typeof(string)),
                                new DataColumn("EndDate", typeof(string)),
                                new DataColumn("CalcDate", typeof(string)),
                                new DataColumn("AccountNo", typeof(string)),
                                new DataColumn("Receive",typeof(double)) });

                //Read the contents of CSV file.
                string csvData = System.IO.File.ReadAllText(filePath);

                //Execute a loop over the rows.
                foreach (string row in csvData.Split('\r'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        dt.Rows.Add();
                        int i = 0;

                        //Execute a loop over the columns.s
                        foreach (string cell in row.Split(','))
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell;
                            i++;
                        }
                    }
                }

                //DataTable dt = ds.Tables[0];
                //sqlBulkCopy.DestinationTableName = "TRF.MilkPayment";
                //sqlBulkCopy.ColumnMappings.Add("MemberID", "MemberID");
                //sqlBulkCopy.ColumnMappings.Add("StartDate", "StartDate");
                //sqlBulkCopy.ColumnMappings.Add("EndDate", "EndDate");
                //sqlBulkCopy.ColumnMappings.Add("CalcDate", "CalcDate");
                //sqlBulkCopy.ColumnMappings.Add("AccountNo", "AccountNo");
                //sqlBulkCopy.ColumnMappings.Add("Receive", "Receive");

                //SqlConnection conn = new SqlConnection();
                //conn.ConnectionString = "Data Source=(local);Initial Catalog=testDBExamples;Integrated Security=True";
                //conn.Open();
                //DataTable info = conn.GetSchema();

                //connectionString
                string conString = ConfigurationManager.ConnectionStrings["CoopWebEntities"].ConnectionString;
                //SqlConnection con = _unitOfWork.new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "TRF.MilkPayment";

                        //[OPTIONAL]: Map the DataTable columns with that of the database table
                        sqlBulkCopy.ColumnMappings.Add("MemberID", "MemberID");
                        sqlBulkCopy.ColumnMappings.Add("StartDate", "StartDate");
                        sqlBulkCopy.ColumnMappings.Add("EndDate", "EndDate");
                        sqlBulkCopy.ColumnMappings.Add("CalcDate", "CalcDate");
                        sqlBulkCopy.ColumnMappings.Add("AccountNo", "AccountNo");
                        sqlBulkCopy.ColumnMappings.Add("Receive", "Receive");

                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
            }

            return View();
        }
    }
}


