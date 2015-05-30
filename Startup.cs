using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IDFWebApp.Startup))]
namespace IDFWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
