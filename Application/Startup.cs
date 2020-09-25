using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Coop.Startup))]
namespace Coop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
