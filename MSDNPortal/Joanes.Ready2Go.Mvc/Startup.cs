using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Joanes.Ready2Go.Mvc.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace Joanes.Ready2Go.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
