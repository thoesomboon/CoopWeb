using Coop.Entities;
using Coop.Infrastructure.ActionFilters;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Adapters;
//using Coop.Library;
using System.Configuration;
using System.Data.SqlClient;
//using Microsoft.Reporting.WebForms;
//using Microsoft.Reporting.Map;
namespace Coop.Controllers
{
    public class ReportDailyController : Controller
    {
        // GET: ReportDaily
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;
        public ReportDailyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ReadDeposit()
        {
            CoopWebEntities db = new CoopWebEntities();
            List<DepositType> DepositTypes = db.DepositType.ToList();
            return View(DepositTypes);
        }
        //[HttpGet]
        //public void GetDepositReport()
        //{
        //    ReportParams objReportParams = new ReportParams();
        //    var data = GetDepositData();
        //    objReportParams.DataSource = data.Tables[0];
        //    objReportParams.ReportTitle = "รายงานบัญชีเงินฝาก";
        //    objReportParams.RptFileName = "RaDeposit.rdlc";
        //    objReportParams.ReportType = "ออมทรัพย์";
        //    objReportParams.DataSetName = "Deposit";
        //    this.HttpContext.Session["ReportParam"] = objReportParams;
        //}
        //public void GetDepositData()
        //{
            
        //    string constr = ConfigurationManager.ConnectionStrings["CoopWebConnectionString"].ConnectionString;
        //    DataSet ds = new DataSet();
        //    string sql = "select * from deposit";
        //    SqlConnection con = new SqlConnection(constr);
        //    SqlCommand cmd = new SqlCommand(sql, con);
        //    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
        //    adpt.Fill(ds);
        //    return ds;
        //}
    }
}