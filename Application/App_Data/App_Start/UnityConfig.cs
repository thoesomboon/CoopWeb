using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Coop.Entities;
using Coop.Controllers;

namespace Coop
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());
            //container.RegisterType<IController, EmployeeController>("Employee");
            //container.RegisterType<IController, ReconfirmController>("Reconfirm");
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}