using Microsoft.Owin;
using Owin;
using System.Data.Entity;
using System.Web.Http;
using TravelAndSave.Data;
using TravelAndSave.Data.Migrations;
using TravelAndSave.Server.Api.Config;

[assembly: OwinStartup(typeof(TravelAndSave.Server.Api.Startup))]

namespace TravelAndSave.Server.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TravelAndSaveDbContext, Configuration>());

            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);

            app.UseWebApi(httpConfiguration);
        }
    }
}
