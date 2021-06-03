using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NovinTehran.Web.Startup))]
namespace NovinTehran.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
