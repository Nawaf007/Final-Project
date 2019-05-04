using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentManagmentSystem.Startup))]
namespace StudentManagmentSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
