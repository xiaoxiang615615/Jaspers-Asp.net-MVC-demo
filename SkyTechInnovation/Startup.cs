using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SkyTechInnovation.Startup))]
namespace SkyTechInnovation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
