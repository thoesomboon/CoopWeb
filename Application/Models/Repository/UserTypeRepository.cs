using Coop.Entities;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.Repository
{
    public interface IUserTypeRepository : IRepository<UserTypes>
    {
        IQueryable<UserTypeModels> ReadDetail();
    }
        public class UserTypeRepository : Repository<UserTypes>, IUserTypeRepository
    {
        public UserTypeRepository(CoopWebEntities context) : base(context) { }

        public IQueryable<UserTypeModels> ReadDetail()
        {

            return from r in Read()
                   select new UserTypeModels
                   {
                       UserTypeID = r.UserTypeID,
                       UserTypeName = r.UserTypeName
                   };
        }
    }
}