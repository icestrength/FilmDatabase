using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FilmDatabase.Startup))]
namespace FilmDatabase
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
