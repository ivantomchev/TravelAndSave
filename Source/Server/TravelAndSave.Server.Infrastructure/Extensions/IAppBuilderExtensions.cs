namespace TravelAndSave.Server.Infrastructure.Extensions
{
    using Owin;
    using OwinMiddlewares;

    public static class IAppBuilderExtensions
    {
        public static IAppBuilder UseMultiTenancy(this IAppBuilder app)
        {
            app.Use<MultiTenancyMiddleware>();
            return app;
        }

        public static IAppBuilder CreatePerOwinContext<T>(this IAppBuilder app, T instance)
        {
            app.Use<CreatePerOwinContextMiddleware<T>>(instance);
            return app;
        }
    }
}
