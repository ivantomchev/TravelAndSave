using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using TravelAndSave.Server.Authorization.Providers;

[assembly: OwinStartup(typeof(TravelAndSave.Server.Authorization.Startup))]

namespace TravelAndSave.Server.Authorization
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                Provider = new AuthorizationServerProvider(),
                RefreshTokenProvider = new RefreshTokenProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(1)
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
        }
    }
}
