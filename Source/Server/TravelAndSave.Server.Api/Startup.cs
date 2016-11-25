using Microsoft.Owin;
using Owin;
using System.Web.Http;
using TravelAndSave.Server.Api.Config;

[assembly: OwinStartup(typeof(TravelAndSave.Server.Api.Startup))]

namespace TravelAndSave.Server.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);

            app.UseWebApi(httpConfiguration);
        }
    }
}
