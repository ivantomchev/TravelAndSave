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
    }
}
