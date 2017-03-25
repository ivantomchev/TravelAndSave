namespace TravelAndSave.Server.Infrastructure.OwinMiddlewares
{
    using Extensions;
    using Microsoft.Owin;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

    public class CreatePerOwinContextMiddleware<T>
    {
        private AppFunc next;
        private T instance;


        public CreatePerOwinContextMiddleware(AppFunc next, T instance)
        {
            this.next = next;
            this.instance = instance;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);
            context.Set(instance);

            await this.next(environment);
        }
    }
}