using System;
using System.Linq;
using Coop.Entities;
using Coop.Infrastructure.Helpers;
using Coop.Models.POCO;

namespace Coop.Models.Repository
{
    public interface IAccessTransactionRepository : IRepository<AccessTransactions>
    {
        void LogOutAccessTransactions(int userId);
        //Comment out by Suwan
        AccessTransactionModels Create(AccessTransactionModels o);
    }

    public class AccessTransactionRepository : Repository<AccessTransactions>, IAccessTransactionRepository
    {
        public AccessTransactionRepository(CoopWebEntities context) : base(context) { }

        public void LogOutAccessTransactions(int userId)
        {
            var transactionId = (from ac in Read() where ac.UserID == userId select ac.AccessTransactionID).Max();
            var model = (from ac in Read() where ac.AccessTransactionID == transactionId select ac).FirstOrDefault();

            if (model != null)
            {
                model.LogoutDate = DateTime.Now;
                _context.SaveChanges();
            }
        }

        public AccessTransactionModels Create(AccessTransactionModels o)
        {
            var access = ModelHelper<AccessTransactions>.Apply(o);
            return ModelHelper<AccessTransactionModels>.Apply(ReadByCreate(access));
        }
    }
}
