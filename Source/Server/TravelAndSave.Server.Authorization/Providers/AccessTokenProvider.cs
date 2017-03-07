namespace TravelAndSave.Server.Authorization.Providers
{
    using Microsoft.Owin.Security.Infrastructure;
    using System.Threading.Tasks;

    public class AccessTokenProvider : AuthenticationTokenProvider, IAuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            base.Create(context);
        }

        public override Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            return base.CreateAsync(context);
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            base.Receive(context);
        }

        public override Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            return base.ReceiveAsync(context);
        }
    }
}