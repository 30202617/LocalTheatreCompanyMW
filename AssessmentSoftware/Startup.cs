using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AssessmentSoftware.Startup))]
namespace AssessmentSoftware
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
