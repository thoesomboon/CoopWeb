using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Entities;
using Coop.Models.POCO;

namespace Coop.Models.Repository
{
    public interface IReasonRepository : IRepository<Reason>
    {
        IQueryable<ReasonModel> ReadDetail();
        //IQueryable<ReasonModel> ReasonList();
    }
    public class ReasonRepository : Repository<Reason>, IReasonRepository
    {
        public ReasonRepository(CoopWebEntities context) : base(context) { }

        public IQueryable<ReasonModel> ReadDetail()
        {
            var Reason = from t in Read()
                        select new ReasonModel
                        {
                            ReasonID = t.ReasonID,
                            ReasonName = t.ReasonName
                        };
            return Reason;
        }
    }
}