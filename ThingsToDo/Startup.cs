using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ThingsToDo.Startup))]
namespace ThingsToDo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
