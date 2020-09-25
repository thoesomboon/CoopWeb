using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coop.Models.Repository
{
    public interface IUserRepository : IRepository<Users>
    {
        IQueryable<ViewUser> ViewUser();
        IQueryable<UserModel> ReadUserModelList();
        UserModel ReadUserModelById(int userId);
        IQueryable<UserTypes> ReadUserTypeModel();
        Users CreateModel(UserModel o);
        Users UpdateUserModel(UserModel usr);
        UserTypeModels GetUserTypeById(int id);
        IQueryable<UserModel> CheckUserIDAndPass(int userId, string password);
        IQueryable<Users> ReadUsers(int userId);
    }
    public class UserRepository : Repository<Users>, IUserRepository
    {
        public UserRepository(CoopWebEntities context) : base(context) { }

        public IQueryable<ViewUser> ViewUser()
        {
            return from p in Read()
                   join et in _context.UserTypes on p.UserTypeID equals et.UserTypeID
                   orderby p.UserID
                   select new ViewUser
                   {
                       UserID = p.UserID,
                       FullName = p.FirstName + " " + p.LastName,
                       UserName = p.UserName,
                       TypeName = et.UserTypeName,
                       IsActive = p.IsActive,
                       IsExpired = false
                   };
        }

        public Users CreateModel(UserModel o)
        {
            var usr = new Users
            {
                FirstName = o.FirstName,
                LastName = o.LastName,
                UserName = o.UserName,
                Password = o.Password,
                UserTypeID = o.UserTypeID,
                IsActive = o.IsActive,
                CreatedBy = AuthorizeHelper.Current.UserAccount().UserID,
                CreatedDate = DateTime.Now,
                ModifiedBy = AuthorizeHelper.Current.UserAccount().UserID,
                ModifiedDate = DateTime.Now,
            };
            return ReadByCreate(usr);
        }

        public IQueryable<UserModel> ReadUserModelList()
        {
            return from r in Read()
                   select new UserModel
                   {
                       UserID = r.UserID,
                       UserTypeID = r.UserTypeID,
                       FirstName = r.FirstName,
                       LastName = r.LastName,
                       UserName = r.UserName,
                       Password = r.Password,
                       IsActive = r.IsActive,
                   };
        }
        public UserModel ReadUserModelById(int usrId)
        {
            var q = from p in Read()
                    where p.UserID == usrId
                    select new UserModel
                    {
                        UserID = p.UserID,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        UserName = p.UserName,
                        Password = p.Password,
                        UserTypeID = p.UserTypeID,
                        IsActive = p.IsActive
                    };
            return q.SingleOrDefault();
        }
        public Users UpdateUserModel(UserModel model)
        {
            var usr = _context.Users.FirstOrDefault(p => p.UserID == model.UserID);
            if (usr != null)
            {
                usr.FirstName = model.FirstName;
                usr.LastName = model.LastName;
                usr.UserName = model.UserName;
                usr.Password = model.Password;
                usr.UserTypeID = model.UserTypeID;
                usr.IsActive = model.IsActive;
                usr.ModifiedBy = model.ModifiedBy;
                usr.ModifiedDate = model.ModifiedDate;
                _context.SaveChanges();
                return usr;
            };
            return new Users();
        }

        public IQueryable<UserTypes> ReadUserTypeModel()
        {
            var q = from p in _context.UserTypes
                    where p.IsActive
                    select p;
            return q;
        }

        public UserTypeModels GetUserTypeById(int id)
        {
            return (from e in _context.UserTypes
                    join c in _context.Users on e.UserTypeID equals c.UserTypeID
                    where c.UserID == id
                    select new UserTypeModels
                    {
                        UserTypeID = e.UserTypeID,
                        UserTypeName = e.UserTypeName,
                        UserTypeDescription = e.UserTypeDescription,
                        IsActive = e.IsActive,
                        SortOrder = e.SortOrder,
                        ModifiedBy = e.ModifiedBy,
                        ModifiedDate = e.ModifiedDate
                    }).Single();
        }
        public IQueryable<UserModel> CheckUserIDAndPass(int userId, string password)
        {
            return (from r in _context.Users
                    where r.UserID == userId
                    && r.Password != password
                    select new UserModel
                    {
                        UserID = r.UserID
                    });
        }

        public IQueryable<Users> ReadUsers(int userId)
        {
            return from r in Read()
                   where r.UserID == userId
                   select r;
        }
    }
}