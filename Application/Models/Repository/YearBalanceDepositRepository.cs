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
    public interface IYearBalanceDepositRepository : IRepository<YearBalanceDeposit>
    {
        IQueryable<YearBalanceDepositModel> ReadDetail();
        IQueryable<YearBalanceDepositModel> ReadDetail(String AccNo, String yr, int prd1, int prd2);
        TransactionResultModel sp_BatYrDepositBal(int coopId, string depTypeID, int userID, string budgetYear, int period1, int period2);
    }

    public class YearBalanceDepositRepository : Repository<YearBalanceDeposit>, IYearBalanceDepositRepository
    {
        public YearBalanceDepositRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<YearBalanceDepositModel> ReadDetail()
        {
            var YearBalanceDeposit = from m in Read()
                              select new YearBalanceDepositModel
                              {
                                  CreatedBy = m.CreatedBy,
                                  CreatedDate = m.CreatedDate,
                                  ModifiedBy = m.ModifiedBy,
                                  ModifiedDate = m.ModifiedDate,
                                  Filestatus = m.Filestatus,
                                  CoopID = m.CoopID,
                                  AccountNo = m.AccountNo,
                                  BudgetYear = m.BudgetYear,
                                  Period1 = m.Period1,
                                  Period2 = m.Period2,
                                  BFLedgerBal = m.BFLedgerBal,
                                  Deposit = m.Deposit,
                                  Withdraw = m.Withdraw,
                                  CFLedgerBal = m.CFLedgerBal,
                                  AccInt = m.AccInt
                              };
            return YearBalanceDeposit;
        }
        public IQueryable<YearBalanceDepositModel> ReadDetail(String AccNo, String yr, int prd1, int prd2)
        {
            var yearBalanceDeposit = ReadDetail().Where(m => m.AccountNo == AccNo && m.BudgetYear == yr && m.Period1 == prd1 && m.Period2 == prd2);
            return yearBalanceDeposit;
        }

        public TransactionResultModel sp_BatYrDepositBal(int coopId, string depTypeID, int userID, string budgetYear, int period1, int period2)
        {
            //var stDate = 
            //set TimeOut
            ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this._context).ObjectContext.CommandTimeout = 600;

            TransactionResultModel transactionResult = _context.Database
                .SqlQuery<TransactionResultModel>(@"EXECUTE [dbo].[BatYrDepositBal] @CoopID, @DepTypeID, @UserID, @BudgetYear, @Period1, @Period2"
                    , new SqlParameter("@CoopID", coopId)
                    , new SqlParameter("@DepTypeID", depTypeID)
                    //, new SqlParameter("@StartDate", startDate)
                    //, new SqlParameter("@EndDate", endDate)
                    , new SqlParameter("@UserID", userID)
                    , new SqlParameter("@BudgetYear", budgetYear)
                    , new SqlParameter("@Period1", period1)
                    , new SqlParameter("@Period2", period2)).FirstOrDefault();
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

        //public void BatYrDepositBalX(int coopId, string depTypeID, int userID, string budgetYear, int period)
        //{
        //    _context.BatYrDepositBal(coopId, depTypeID, userID, budgetYear, period);
        //}
    }
}