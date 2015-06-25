using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Orkidea.PollaExpress.WebFront.Startup))]
namespace Orkidea.PollaExpress.WebFront
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
