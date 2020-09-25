using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using Coop.Models.Repository;
//using Coop.Models.Repositories; 

namespace Coop.Models.Repository
{
    public interface IProvinceRepository : IRepository<Province>
    {
        IQueryable<ProvinceModel> ReadDetail();
        IQueryable<ProvinceModel> ReadDetail(int PID);
        bool NotActive(int PID);
    }

    public class ProvinceRepository : Repository<Province>, IProvinceRepository
    {
        public ProvinceRepository(CoopWebEntities context) : base(context)
        {
        }
        public IQueryable<ProvinceModel> ReadDetail()
        {
            var province = from l in Read()
                           select new ProvinceModel
                           {
                               ProvinceID = l.ProvinceID,
                               ProvinceName = l.ProvinceName
                           };
            return province;
        }
        public IQueryable<ProvinceModel> ReadDetail(int PID)
        {
            var province = ReadDetail().Where(l => l.ProvinceID == PID);
            return province;
        }
        public bool NotActive(int PID)
        {
            var data = (from prov in Read()
                        where prov.ProvinceID == PID
                        select prov).FirstOrDefault();

            if (data == null) { return false; }

            data.IsActive = false;
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
    }
}