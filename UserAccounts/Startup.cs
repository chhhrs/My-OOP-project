using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UserAccounts.Startup))]
namespace UserAccounts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
