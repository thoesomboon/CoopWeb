using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Entities;
using Coop.Models.POCO;
//using Coop.Models.Repository;

namespace Coop.Models.Repository
{
    public interface ISubDistrictRepository : IRepository<SubDistrict>
    {
        IQueryable<SubDistrictModel> ReadDetail();
    }
    public class SubDistrictRepository : Repository<SubDistrict>, ISubDistrictRepository
    {
        public SubDistrictRepository(CoopWebEntities context) : base(context) { }

        public IQueryable<SubDistrictModel> ReadDetail()
        {
            var subdistrict = from s in Read()
                        select new SubDistrictModel
                        {
                            ProvinceID = s.ProvinceID,
                            DistrictID = s.DistrictID,
                            SubDistrictID = s.SubDistrictID,
                            SubDistrictName = s.SubDistrictName
                        };
            return subdistrict;
        }
    }
}