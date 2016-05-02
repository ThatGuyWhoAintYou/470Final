using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_470_Final_AGAIN.Startup))]
namespace _470_Final_AGAIN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
