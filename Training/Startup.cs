using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SAPI.Startup))]
namespace SAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
