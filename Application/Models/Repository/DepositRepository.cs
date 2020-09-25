using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Entities;
using Coop.Models.POCO;
//using Coop.Library;
using Coop.Infrastructure.Helpers;

using System.Data.SqlClient;
using System.Data;
using System.Data.Objects;

namespace Coop.Models.Repository
{
    public interface IDepositRepository : IRepository<Deposit>
    {
        IQueryable<DepositModel> ReadDetail();
        IQueryable<DepositModel> ReadDetail(string AccNo);
        //IQueryable<OtxDepositModel> ReadOtxDeposit(string AccNo);
        DepositModel Create(DepositModel model);
        bool Update(DepositModel model);
        DepositModel ReadDeposit(string AccNo);
        OtxDepositModel ReadOtxDeposit(string AccNo);
        bool UpdateOtxDeposit(DepositModel model);
        bool UpdateOtxDepositPrintBook(DepositModel model);
        TransactionResultModel sp_BatPeriodDepositIntDue(int coopId, string depTypeID, DateTime calcDate, int userID, string branchID, string ProgName, string WorkId);
    }
    public class DepositRepository : Repository<Deposit>, IDepositRepository
    {
        public DepositRepository(CoopWebEntities context) : base(context) { }
        public IQueryable<DepositModel> ReadDetail()
        {
            var deposit = from d in Read()
                          select new DepositModel
                       {
                        //CreatedBy = d.CreatedBy,
                        //CreatedDate = d.CreatedDate,
                        //ModifiedBy = d.ModifiedBy,
                        //ModifiedDate = d.ModifiedDate,
                        Filestatus = d.Filestatus,
                        CoopID = d.CoopID,
                        AccountNo = d.AccountNo,
                        DepositTypeID = d.DepositTypeID,
                        MemberID = d.MemberID,
                        AccountName = d.AccountName,
                        BookNo = d.BookNo,
                        OpenDate = d.OpenDate,
                        LastContact = d.LastContact,
                        LastCalcInt = d.LastCalcInt,
                        IntType = d.IntType,
                        BFLedgerBal = d.BFLedgerBal,
                        LedgerBal = d.LedgerBal,
                        AvailBal = d.AvailBal,
                        BookBal = d.BookBal,
                        AccInt = d.AccInt,
                        LastLedgerLine = d.LastLedgerLine,
                        LastBookLine = d.LastBookLine,
                        BookPage = d.BookPage,
                        HoldTypeID = d.HoldTypeID,
                        HoldAmt = d.HoldAmt,
                        IntDueAmt = d.IntDueAmt,
                        BudgetYear = d.BudgetYear,
                        UnpayInt = d.UnpayInt,
                        BookSeq = d.BookSeq,
                        MonthDepAmt = d.MonthDepAmt,
                        MonthDepositDate = d.MonthDepositDate,
                        MonthWithdrawAmt = d.MonthWithdrawAmt,
                        MonthWithdrawTimes = d.MonthWithdrawTimes,
                        Amt1 = d.Amt1,
                        Amt2 = d.Amt2,
                        Amt3 = d.Amt3
            };
            return deposit;
        }
        public IQueryable<DepositModel> ReadDetail(string AccNo)
        {
            var deposit = ReadDetail().Where(l => l.AccountNo == AccNo);
            return deposit;
        }
        public DepositModel Create(DepositModel model)
        {
            DepositModel cModel = new DepositModel
            {
                Filestatus = model.Filestatus,
                CoopID = model.CoopID,
                AccountNo = model.AccountNo,
                DepositTypeID = model.DepositTypeID,
                MemberID = model.MemberID,
                AccountName = model.AccountName,
                BookNo = model.BookNo,
                OpenDate = model.OpenDate,
                LastContact = model.LastContact,
                LastCalcInt = model.LastCalcInt,
                IntType = model.IntType,
                BFLedgerBal = model.BFLedgerBal,
                LedgerBal = model.LedgerBal,
                AvailBal = model.AvailBal,
                BookBal = model.BookBal,
                AccInt = model.AccInt,
                LastLedgerLine = model.LastLedgerLine,
                LastBookLine = model.LastBookLine,
                BookPage = model.BookPage,
                HoldTypeID = model.HoldTypeID,
                HoldAmt = model.HoldAmt,
                IntDueAmt = model.IntDueAmt,
                BudgetYear = model.BudgetYear,
                UnpayInt = model.UnpayInt,
                BookSeq = model.BookSeq,
                MonthDepAmt = model.MonthDepAmt,
                MonthDepositDate = model.MonthDepositDate,
                MonthWithdrawAmt = model.MonthWithdrawAmt,
                MonthWithdrawTimes = model.MonthWithdrawTimes,
                Amt1 = model.Amt1,
                Amt2 = model.Amt2,
                Amt3 = model.Amt3,

                CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                CreatedDate = DateTime.Now,
                ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                ModifiedDate = DateTime.Now
            };
            var cDeposit = ModelHelper<Deposit>.Apply(cModel);
            return ModelHelper<DepositModel>.Apply(ReadByCreate(cDeposit));

        }
        public bool Update(DepositModel model)
        {
            var data = (from dep in Read()
                        where dep.AccountNo == model.AccountNo
                        select dep).FirstOrDefault();
            if (data == null) { return false; }

            data.Filestatus = model.Filestatus;
            data.CoopID = model.CoopID;
            data.AccountNo = model.AccountNo;
            data.DepositTypeID = model.DepositTypeID;
            data.MemberID = model.MemberID;
            data.AccountName = model.AccountName;
            data.BookNo = model.BookNo;
            data.OpenDate = model.OpenDate;
            data.LastContact = model.LastContact;
            data.LastCalcInt = model.LastCalcInt;
            data.IntType = model.IntType;
            data.BFLedgerBal = model.BFLedgerBal;
            data.LedgerBal = model.LedgerBal;
            data.AvailBal = model.AvailBal;
            data.BookBal = model.BookBal;
            data.AccInt = model.AccInt;
            data.LastLedgerLine = model.LastLedgerLine;
            data.LastBookLine = model.LastBookLine;
            data.BookPage = model.BookPage;
            data.HoldTypeID = model.HoldTypeID;
            data.HoldAmt = model.HoldAmt;
            data.IntDueAmt = model.IntDueAmt;
            data.BudgetYear = model.BudgetYear;
            data.UnpayInt = model.UnpayInt;
            data.BookSeq = model.BookSeq;
            data.MonthDepAmt = model.MonthDepAmt;
            data.MonthDepositDate = model.MonthDepositDate;
            data.MonthWithdrawAmt = model.MonthWithdrawAmt;
            data.MonthWithdrawTimes = model.MonthWithdrawTimes;
            data.Amt1 = model.Amt1;
            data.Amt2 = model.Amt2;
            data.Amt3 = model.Amt3;

            data.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
            data.ModifiedDate = System.DateTime.Now;

            int returnVal = 0;
            bool result = false;

            try
            {
                returnVal = _context.SaveChanges();
                result = returnVal > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public DepositModel ReadDeposit(string AccNo)
        {
            var q = from d in Read()
                    join m in _context.Member on d.MemberID equals m.MemberID
                    join dt in _context.DepositType on d.DepositTypeID equals dt.DepositTypeID
                    where d.AccountNo == AccNo //&& m.CoopID == coopId
                    select new DepositModel
                    {
                        Filestatus = d.Filestatus,
                        CoopID = d.CoopID,
                        AccountNo = d.AccountNo,
                        DepositTypeID = d.DepositTypeID,
                        DepositTypeName= dt.DepositTypeName,
                        MemberID = d.MemberID,
                        Name = m.Name,
                        AccountName = d.AccountName,
                        BookNo = d.BookNo,
                        OpenDate = d.OpenDate,
                        LastContact = d.LastContact,
                        LastCalcInt = d.LastCalcInt,
                        IntType = d.IntType,
                        BFLedgerBal = d.BFLedgerBal,
                        LedgerBal = d.LedgerBal,
                        AvailBal = d.AvailBal,
                        BookBal = d.BookBal,
                        AccInt = d.AccInt,
                        LastLedgerLine = d.LastLedgerLine,
                        LastBookLine = d.LastBookLine,
                        BookPage = d.BookPage,
                        HoldTypeID = d.HoldTypeID,
                        HoldAmt = d.HoldAmt,
                        IntDueAmt = d.IntDueAmt,
                        BudgetYear = d.BudgetYear,
                        UnpayInt = d.UnpayInt,
                        BookSeq = d.BookSeq,
                        MonthDepAmt = d.MonthDepAmt,
                        MonthDepositDate = d.MonthDepositDate,
                        MonthWithdrawAmt = d.MonthWithdrawAmt,
                        MonthWithdrawTimes = d.MonthWithdrawTimes,
                        Amt1 = d.Amt1,
                        Amt2 = d.Amt2,
                        Amt3 = d.Amt3
                    };
            return q.FirstOrDefault();
        }
        public OtxDepositModel ReadOtxDeposit(string AccNo)
        {
            var q = from d in Read()
                    join m in _context.Member on d.MemberID equals m.MemberID
                    join dt in _context.DepositType on d.DepositTypeID equals dt.DepositTypeID
                    where d.AccountNo == AccNo && d.Filestatus == "A"
                    select new OtxDepositModel
                    {
                        Filestatus = d.Filestatus,
                        CoopID = d.CoopID,
                        AccountNo = d.AccountNo,
                        DepositTypeID = d.DepositTypeID,
                        DepositTypeName = dt.DepositTypeName,
                        MemberID = d.MemberID,
                        Name = m.Name,
                        AccountName = d.AccountName,
                        BookNo = d.BookNo,
                        OpenDate = d.OpenDate,
                        LastContact = d.LastContact,
                        LastCalcInt = d.LastCalcInt,
                        IntType = d.IntType,
                        BFLedgerBal = d.BFLedgerBal,
                        LedgerBal = d.LedgerBal,
                        AvailBal = d.AvailBal,
                        BookBal = d.BookBal,
                        AccInt = d.AccInt,
                        //LastLedgerLine = d.LastLedgerLine,
                        LastBookLine = d.LastBookLine,
                        BookPage = d.BookPage,
                        HoldTypeID = d.HoldTypeID,
                        HoldAmt = d.HoldAmt,
                        IntDueAmt = d.IntDueAmt,
                        BudgetYear = d.BudgetYear,
                        UnpayInt = d.UnpayInt,
                        BookSeq = d.BookSeq,
                        MonthDepAmt = d.MonthDepAmt,
                        MonthDepositDate = d.MonthDepositDate,
                        MonthWithdrawAmt = d.MonthWithdrawAmt,
                        MonthWithdrawTimes = d.MonthWithdrawTimes,
                        Amt1 = d.Amt1,
                        Amt2 = d.Amt2,
                        Amt3 = d.Amt3,
                        // ส่วนนี้มาจาก DepositType
                        CalcIntType = dt.CalcIntType,
                        CalcIntRate = dt.CalcIntRate,
                        MonthMaxWithdrawAmt = dt.MonthMaxWithdrawAmt,
                        MonthMaxWithdrawTimes = dt.MonthMaxWithdrawTimes,
                        MaxChargeAmt = dt.MaxChargeAmt,
                        MinChargeAmt = dt.MinChargeAmt,
                        WithdrawChargePercent = dt.WithdrawChargePercent,
                        IntAmt = 0
                    };
            return q.FirstOrDefault();
        }
        public bool UpdateOtxDeposit(DepositModel model)
        {
            var data = (from dep in Read()
                        where dep.AccountNo == model.AccountNo
                        select dep).FirstOrDefault();
            if (data == null) { return false; }

            data.AccountNo = model.AccountNo;
            data.Filestatus = model.Filestatus;
            data.LastContact = model.LastContact;
            data.LastCalcInt = model.LastCalcInt;
            data.LedgerBal = model.LedgerBal;
            data.AvailBal = model.AvailBal;
            data.AccInt = model.AccInt;
            data.MonthWithdrawAmt = model.MonthWithdrawAmt;
            data.MonthWithdrawTimes = model.MonthWithdrawTimes;
            data.Amt2 = model.Amt2;
            data.Amt3 = model.Amt3;

            data.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
            data.ModifiedDate = System.DateTime.Now;

            int returnVal = 0;
            bool result = false;

            try
            {
                returnVal = _context.SaveChanges();
                result = returnVal > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public bool UpdateOtxDepositPrintBook(DepositModel model)
        {
            var data = (from dep in Read()
                        where dep.AccountNo == model.AccountNo
                        select dep).FirstOrDefault();
            if (data == null) { return false; }

            data.BookBal = model.BookBal;
            data.LastBookLine = model.LastBookLine;
            data.BookPage = model.BookPage;

            data.ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID;
            data.ModifiedDate = System.DateTime.Now;

            int returnVal = 0;
            bool result = false;

            try
            {
                returnVal = _context.SaveChanges();
                result = returnVal > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        //public void stored_BatMtnDeposit(string coopId, string userId, int modeId, string StartRoute, string EndRoute, string StartmemberId, string EndmemberId, bool IsApp, bool IsShare, bool IsLoan, bool IsDeposit, bool IsPolicyLoan, bool IsPolicyLife)
        //{
        //    SqlConnection conn = APIConnection.APIConnection.ApplicationServicesConnection();
        //    using (conn)
        //    {
        //        string SQLcommand = "EXECUTE [dbo].[BatMthSLD]" + " '" + coopId + "','" + userId + "','" + modeId + "','" + StartRoute + "','" + EndRoute + "','" + StartmemberId + "','" + EndmemberId + "','" + IsApp + "','" + IsShare + "','" + IsLoan + "','" + IsDeposit + "','" + IsPolicyLoan + "','" + IsPolicyLife + "'";

        //        if (conn.State != ConnectionState.Open)
        //        {
        //            conn.Open();
        //        }

        //        SqlDataReader da;
        //        SqlCommand sqlCommand = new SqlCommand(SQLcommand, conn);
        //        //sqlCommand.CommandText = SQLcommand;
        //        //sqlCommand.Connection = conn;
        //        //sqlCommand.CommandType = System.Data.CommandType.Text;
        //        sqlCommand.CommandTimeout = 500000;
        //        da = sqlCommand.ExecuteReader();


        //        if (conn.State != ConnectionState.Closed)
        //        {
        //            conn.Close();
        //        }
        //    }
        //}
        public TransactionResultModel sp_BatPeriodDepositIntDue(int coopId, string depTypeID, DateTime calcDate, int userID, string branchID, string ProgName, string WorkId)
        {
            //var stDate = 
            //set TimeOut
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this._context).ObjectContext.CommandTimeout = 600;

            TransactionResultModel transactionResult = _context.Database
                .SqlQuery<TransactionResultModel>(@"EXECUTE [dbo].[BatPeriodDepositIntDue] @CoopID, @DepTypeID, @CalcDate, @UserID, @BranchID, @ProgramName, @WorkStationId"
                    , new SqlParameter("@CoopID", coopId)
                    , new SqlParameter("@DepTypeID", depTypeID)
                    , new SqlParameter("@CalcDate", calcDate)
                    , new SqlParameter("@UserID", userID)
                    , new SqlParameter("@BranchID", branchID)
                    , new SqlParameter("@ProgramName", ProgName)
                    , new SqlParameter("@WorkStationId", WorkId)).FirstOrDefault();
            return transactionResult;
        }

    }
}