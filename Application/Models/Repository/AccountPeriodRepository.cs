using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Linq;
//using Coop.Models.Repositories;

namespace Coop.Models.Repository
{
    public interface IAccountPeriodRepository : IRepository<AccountPeriod>
    {
        IQueryable<AccountPeriodModel> ReadDetail();
        IQueryable<AccountPeriodModel> ReadDetail(string yr);
        IQueryable<AccountPeriodModel> ReadDetail(string yr, int prd);
    }
    public class AccountPeriodRepository : Repository<AccountPeriod>, IAccountPeriodRepository
    {
        public AccountPeriodRepository(CoopWebEntities context) : base(context) { }
        public IQueryable<AccountPeriodModel> ReadDetail()
        {
            var AccountPeriod = from d in Read()
                              select new AccountPeriodModel
                              {
                                  BudgetYear = d.BudgetYear,
                                  PeriodID = d.PeriodID,
                                  StartDate = d.StartDate,
                                  EndDate = d.EndDate
                              };
            return AccountPeriod;
        }
        public IQueryable<AccountPeriodModel> ReadDetail(string yr)
        {
            var AccountPeriod = ReadDetail().Where(d => d.BudgetYear == yr);
            return AccountPeriod;
        }
        public IQueryable<AccountPeriodModel> ReadDetail(string yr, int prd)
        {
            var AccountPeriod = ReadDetail().Where(d => d.BudgetYear == yr && d.PeriodID == prd);
            return AccountPeriod;
        }        
    }
}