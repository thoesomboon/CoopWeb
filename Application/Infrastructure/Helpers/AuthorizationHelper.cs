#region Using
using Coop.Entities;
using Coop.Models.POCO;
using System.Collections.Generic;
using System.Web.Mvc;
#endregion

namespace Coop.Infrastructure.Helpers
{
    ///<summary>
    ///  http://aspnet.codeplex.com/sourcecontrol/changeset/view/23011?projectName=aspnet#266456
    ///  http://aspnet.codeplex.com/sourcecontrol/changeset/view/23011?projectName=aspnet#266437
    ///</summary>
    ///<summary>
    ///  The 'Singleton' class
    ///</summary>
    ///

    public interface IAuthorization
    {
        List<ModuleModel> ModuleMenulist();
        List<ModuleModel> ModuleMenuCategorylist();
        CurrentAppControlModel AppControl();
        CurrentApplicationVersion ApplicationVersion();
        CurrentUserAccountModel UserAccount();
        CoopControlModel CoopControls();
    }

    public static class AuthorizeHelper
    {
        ///<summary>
        ///  Static members are 'eagerly initialized', that is, 
        ///  immediately when class is loaded for the first time.
        ///  .NET guarantees thread safety for static initialization
        ///  readonly if not refresh method
        ///</summary>
        ///
        public static IAuthorization Current
        {
            get
            {
                var resolver = DependencyResolver.Current;
                return resolver.GetService<Authorization>();
                ;
            }
        }

    }
    public class Authorization : IAuthorization
    {
        private readonly IStatefulStorage _storage = StatefulStorageHelper.PerSession;
        private readonly IUnitOfWork _unitOfWork;

        public Authorization(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }
        public CoopControlModel CoopControls()
        {
            return _storage.GetOrAdd("CoopControl", () => new CoopControlModel());
        }
        /// <summary>
        ///   //UserAccount is key of session key for get current user infomation in UserAccountModel class by StatefullStorageHelper class
        /// </summary>
        /// <returns> </returns>
        /// 
        public CurrentUserAccountModel UserAccount()
        {
            return _storage.GetOrAdd("UserAccount", () => new CurrentUserAccountModel());
        }
        public List<ModuleModel> ModuleMenulist()
        {
            return _storage.GetOrAdd("ModuleMenulist", () => new List<ModuleModel>());
        }

        public List<ModuleModel> ModuleMenuCategorylist()
        {
            return _storage.GetOrAdd("ModuleMenuCategorylist", () => new List<ModuleModel>());
        }
        public CurrentAppControlModel AppControl()
        {
            return _storage.GetOrAdd("AppControl", () => new CurrentAppControlModel());
        }
        public CurrentApplicationVersion ApplicationVersion()
        {
            return _storage.GetOrAdd("ApplicationVersion", () => new CurrentApplicationVersion());
        }
        //public AccessTransactionModels MachineName()
        //{
        //    return _storage.GetOrAdd("MachineName", () => new AccessTransactionModels());
        //}
    }
}