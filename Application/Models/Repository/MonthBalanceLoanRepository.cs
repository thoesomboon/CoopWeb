using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.Repository
{
    public interface IMonthBalanceLoanRepository : IRepository<MonthBalanceLoan>
    {
        IQueryable<MonthBalanceLoanModel> ReadDetail();
        IQueryable<MonthBalanceLoanModel> ReadDetail(String memID, String yr, int mthNo);
    }

    public class MonthBalanceLoanRepository : Repository<MonthBalanceLoan>, IMonthBalanceLoanRepository
    {
        public MonthBalanceLoanRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<MonthBalanceLoanModel> ReadDetail()
        {
            var MonthBalanceLoan = from l in Read()
                                      select new MonthBalanceLoanModel
                                      {
                                          CreateBy = l.CreateBy,
                                          CreateDate = l.CreateDate,
                                          ModifiedBy = l.ModifiedBy,
                                          ModifiedDate = l.ModifiedDate,
                                          Filestatus = l.Filestatus,
                                          CoopID = l.CoopID,
                                          LoanID = l.LoanID,
                                          BudgetYear = l.BudgetYear,
                                          Period = l.Period,
                                          LoanTypeID = l.LoanTypeID,
                                          MemberID = l.MemberID
                                          //BFbalance = l.BFbalance,
                                          //BalanceD = l.BalanceD,
                                          //BalanceC = l.BalanceC,
                                          //Balance = l.Balance,
                                          //BalancecDue = l.BalancecDue,
                                          //BalanceCBDue = l.BalanceCBDue,
                                          //BFint = l.BFint,
                                          //BFintC = l.BFintC,
                                          //IntCalC = l.IntCalC,
                                          //IntCalcC = l.IntCalcC,
                                          //UnpayIntCalc = l.UnpayIntCalc,
                                          //BFcharge = l.BFcharge,
                                          //BFChargeC = l.BFChargeC,
                                          //IntCharge = l.IntCharge,
                                          //IntChargeC = l.IntChargeC,
                                          //UnpayChargeCalc = l.UnpayChargeCalc,
                                          //BalanceA = l.BalanceA,
                                          //BalanceCB = l.BalanceCB,
                                          //BalanceCM = l.BalanceCM,
                                          //BalanceCN = l.BalanceCN,
                                          //BalanceCR = l.BalanceCR,
                                          //BalanceCt = l.BalanceCt,
                                          //BFDiscInt = l.BFDiscInt,
                                          //DiscIntCalc = l.DiscIntCalc,
                                          //DiscIntCalcC = l.DiscIntCalcC,
                                          //UnpayDiscIntCalc = l.UnpayDiscIntCalc
                                      };
            return MonthBalanceLoan;
        }
        public IQueryable<MonthBalanceLoanModel> ReadDetail(String lonID, String yr, int mthNo)
        {
            var mthBalLoan = ReadDetail().Where(l => l.LoanID == lonID || l.BudgetYear == yr || l.Period == mthNo);
            return mthBalLoan;
        }
    }
}