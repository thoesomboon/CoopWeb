using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Entities;
using Coop.Models.POCO;

namespace Coop.Models.Repository
{
    public interface IDistrictRepository : IRepository<District>
    {
        IQueryable<DistrictModel> ReadDetail();
    }
    public class DistrictRepository : Repository<District>, IDistrictRepository
    {
        public DistrictRepository(CoopWebEntities context) : base(context) { }

        public IQueryable<DistrictModel> ReadDetail()
        {
            var district = from t in Read()
                        select new DistrictModel
                        {
                            ProvinceID = t.ProvinceID,
                            DistrictID = t.DistrictID,
                            DistrictName = t.DistrictName
                        };
            return district;
        }
    }
}