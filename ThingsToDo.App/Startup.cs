using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ThingsToDo.App.Startup))]
namespace ThingsToDo.App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
