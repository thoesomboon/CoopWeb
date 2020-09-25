using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Coop.Models.Repository
{
    public interface IMonthBalanceDepositRepository : IRepository<MonthBalanceDeposit>
    {
        IQueryable<MonthBalanceDepositModel> ReadDetail();
        IQueryable<MonthBalanceDepositModel> ReadDetail(String AccNo, String yr, int mthNo);
        TransactionResultModel sp_BatMthDepositBal(int coopId, string depTypeID, int userID, string budgetYear, int period);
    }

    public class MonthBalanceDepositRepository : Repository<MonthBalanceDeposit>, IMonthBalanceDepositRepository
    {
        public MonthBalanceDepositRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<MonthBalanceDepositModel> ReadDetail()
        {
            var MonthBalanceDeposit = from m in Read()
                              select new MonthBalanceDepositModel
                              {
                                  CreatedBy = m.CreatedBy,
                                  CreatedDate = m.CreatedDate,
                                  ModifiedBy = m.ModifiedBy,
                                  ModifiedDate = m.ModifiedDate,
                                  Filestatus = m.Filestatus,
                                  CoopID = m.CoopID,
                                  AccountNo = m.AccountNo,
                                  BudgetYear = m.BudgetYear,
                                  Period = m.Period,
                                  BFLedgerBal = m.BFLedgerBal,
                                  Deposit = m.Deposit,
                                  Withdraw = m.Withdraw,
                                  CFLedgerBal = m.CFLedgerBal,
                                  AccInt = m.AccInt
                              };
            return MonthBalanceDeposit;
        }
        public IQueryable<MonthBalanceDepositModel> ReadDetail(String AccNo, String yr, int mthNo)
        {
            var MonthBalanceDeposit = ReadDetail().Where(m => m.AccountNo == AccNo && m.BudgetYear == yr && m.Period == mthNo);
            return MonthBalanceDeposit;
        }

        public TransactionResultModel sp_BatMthDepositBal(int coopId, string depTypeID, int userID, string budgetYear, int period)
        {
            //var stDate = 
            //set TimeOut
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this._context).ObjectContext.CommandTimeout = 600;

            TransactionResultModel transactionResult = _context.Database
                .SqlQuery<TransactionResultModel>(@"EXECUTE [dbo].[BatMthDepositBal] @CoopID, @DepTypeID, @UserID, @BudgetYear, @Period"
                    , new SqlParameter("@CoopID", coopId)
                    , new SqlParameter("@DepTypeID", depTypeID)
                    , new SqlParameter("@UserID", userID)
                    , new SqlParameter("@BudgetYear", budgetYear)
                    , new SqlParameter("@Period", period)).FirstOrDefault();
            return transactionResult;
        }
        //public void BatDayClose(DateTime nextdate)
        //{
        //    var coopId = AuthorizeHelper.Current.CoopSystem().coop_id;
        //    var userId = AuthorizeHelper.Current.UserLogin().UserName;
        //    _context.BatDayClose(coopId, userId, nextdate);
        //}
        //public void BatDayClose(DateTime nextdate)
        //{
        //    var coopId = AuthorizeHelper.Current.CoopSystem().coop_id;
        //    var userId = AuthorizeHelper.Current.UserLogin().UserName;
        //    _context.BatDayClose(coopId, userId, nextdate);
        //}

        //public void BatMthDepositBalX(int coopId, string depTypeID, int userID, string budgetYear, int period)
        //{
        //    _context.BatMthDepositBal(coopId, depTypeID, userID, budgetYear, period);
        //}
    }
}