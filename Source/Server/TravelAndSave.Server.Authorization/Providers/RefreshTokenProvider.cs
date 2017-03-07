namespace TravelAndSave.Server.Authorization.Providers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Owin.Security.Infrastructure;
    using System.Security.Claims;
    using Microsoft.Owin.Security;

    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private string inMemoryProtectedData;

        public void Create(AuthenticationTokenCreateContext context)
        {
        }

        public Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            ClaimsIdentity claimsIdentity = context.Ticket.Identity;
            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary);
            refreshTokenProperties.IssuedUtc = context.Ticket.Properties.IssuedUtc;
            refreshTokenProperties.ExpiresUtc = DateTime.UtcNow.AddDays(10);

            var refreshTokenTicket = new AuthenticationTicket(claimsIdentity, refreshTokenProperties);
            this.inMemoryProtectedData = context.SerializeTicket();
            context.SetToken("test refresh token");

            return Task.FromResult<object>(null);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
        }

        public Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            if (!string.IsNullOrEmpty(this.inMemoryProtectedData))
            {
                context.DeserializeTicket(this.inMemoryProtectedData);
            }
            return Task.FromResult<object>(null);
        }
    }
}