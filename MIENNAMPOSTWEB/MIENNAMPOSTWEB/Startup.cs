using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MIENNAMPOSTWEB.Startup))]
namespace MIENNAMPOSTWEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
