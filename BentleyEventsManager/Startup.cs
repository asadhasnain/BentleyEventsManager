using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BentleyEventsManager.Startup))]
namespace BentleyEventsManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
