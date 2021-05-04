using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LoyaltyProgram.Startup))]
namespace LoyaltyProgram
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
