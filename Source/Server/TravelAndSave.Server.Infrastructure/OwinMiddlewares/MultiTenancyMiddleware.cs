namespace TravelAndSave.Server.Infrastructure.OwinMiddlewares
{
    using Extensions;
    using Microsoft.Owin;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public class MultiTenancyMiddleware
    {
        private AppFunc next;

        public MultiTenancyMiddleware(AppFunc next)
        {
            this.next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);

            var requestDomain = context.GetRequestDomain();
            if (!string.IsNullOrEmpty(requestDomain))
            {
                //Check if requestDomain corresponds with any tenant registered in Database
                //If if does set the tenant into OwinContext using SetCurrentTenant extension method

                //Example:
                //var currentTenant = new TenantsService().GetTenantByDomain(requestDomain);
                //if (currentTenant != null)
                //{
                //    context.SetCurrentTenant(currentTenant);
                //}
            }

            await this.next(environment);
        }
    }
}
