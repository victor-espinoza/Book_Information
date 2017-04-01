using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcBook.Startup))]
namespace MvcBook{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }//close Configuration(...)
    }//close class Startup
}//close namespace MvcBook
