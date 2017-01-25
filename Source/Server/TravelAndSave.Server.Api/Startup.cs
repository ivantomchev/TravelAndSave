using Microsoft.Owin;
using Owin;
using System;
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
            this.InitializeDatabase();

            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);

            app.UseWebApi(httpConfiguration);
        }

        private void InitializeDatabase()
        {
            var environment = System.Configuration.ConfigurationManager.AppSettings.Get("Environment");
            if (environment == "Production")
            {
                var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                if (!Database.Exists(connectionString))
                {
                    throw new InvalidOperationException("Failed to connect to database.");
                }
                Database.SetInitializer(new NullDatabaseInitializer<TravelAndSaveDbContext>());
            }
            else
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<TravelAndSaveDbContext, Configuration>());
            }
        }
    }
}
