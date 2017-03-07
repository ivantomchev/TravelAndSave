namespace TravelAndSave.Server.Authorization.Providers
{
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OAuth;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            var props = CreateProperties("FirstName", "LastName", "Administrator");
            AuthenticationTicket ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);

            return base.GrantResourceOwnerCredentials(context);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            return base.GrantRefreshToken(context);
        }

        public static AuthenticationProperties CreateProperties(string firstName, string lastName, string role)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {"FirstName", firstName },
                { "LastName", lastName },
                {"Role", role }
            };

            return new AuthenticationProperties(data);
        }
    }
}