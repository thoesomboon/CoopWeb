using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Linq;
//using Coop.Models.Repositories;

namespace Coop.Models.Repository
{
    public interface IInterestRepository : IRepository<Interest>
    {
        IQueryable<InterestModel> ReadDetail();
    }

    public class InterestRepository : Repository<Interest>, IInterestRepository
    {
        public InterestRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<InterestModel> ReadDetail()
        {
            var interest = from i in Read()
                select new InterestModel
                {
                    CreatedBy = i.CreatedBy,
                    CreatedDate = i.CreatedDate,
                    ModifiedBy = i.ModifiedBy,
                    ModifiedDate = i.ModifiedDate,
                    Filestatus = i.Filestatus,
                    CoopID = i.CoopID,
                    Type = i.Type,
                    TInt = i.TInt,
                    FirstEffectDate = i.FirstEffectDate,
                    LastEffectDate = i.LastEffectDate,
                    Balance1 = i.Balance1,
                    Rate1 = i.Rate1,
                    ChargeRate1 = i.ChargeRate1,
                    Balance2 = i.Balance2,
                    Rate2 = i.Rate2,
                    ChargeRate2 = i.ChargeRate2,
                    Balance3 = i.Balance3,
                    Rate3 = i.Rate3,
                    ChargeRate3 = i.ChargeRate3,
                    Balance4 = i.Balance4,
                    Rate4 = i.Rate4,
                    ChargeRate4 = i.ChargeRate4,
                    Balance5 = i.Balance5,
                    Rate5 = i.Rate5,
                    ChargeRate5 = i.ChargeRate5
                };
            return interest;
        }
    }
}