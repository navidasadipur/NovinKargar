using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NovinKargar.Web.Startup))]
namespace NovinKargar.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
