using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FronEnd.Startup))]
namespace FronEnd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
