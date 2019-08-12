using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(webapp.Startup))]
namespace webapp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
