using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
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

            //Token Consumption, configuring the Resource Server to accept (consume only) tokens with bearer scheme
            //1. Extracting the access token which is sent in the “Authorization header” with the “Bearer” scheme.
            //2. Extracting the authentication ticket from access token, this ticket will contain claims identity and any additional authentication properties.
            //3. Checking the validity period of the authentication ticket.
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
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
