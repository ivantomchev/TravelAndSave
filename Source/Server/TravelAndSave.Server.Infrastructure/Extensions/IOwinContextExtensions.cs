namespace TravelAndSave.Server.Infrastructure.Extensions
{
    using Microsoft.Owin;
    using System;
    using System.Linq;

    public static class IOwinContextExtensions
    {
        private const string CurrentTenantKey = "CurrentTenant";

        public static T GetCurrentTenant<T>(this IOwinContext context) where T : class
        {
            object tenant;
            if (!context.Environment.TryGetValue(CurrentTenantKey, out tenant))
            {
                return null;
            }

            return tenant as T;
        }

        public static void SetCurrentTenant<T>(this IOwinContext context, T tenant)
        {
            if (tenant != null)
            {
                context.Environment.Add(CurrentTenantKey, tenant);
            }
        }

        public static string GetRequestDomain(this IOwinContext context)
        {
            var refererHeader = context.Request.Headers.FirstOrDefault(h => h.Key == "Referer");
            if (refererHeader.Key == null)
            {
                return null;
            }

            var headerValue = refererHeader.Value.FirstOrDefault();
            if (string.IsNullOrEmpty(headerValue))
            {
                return null;
            }

            Uri uri;
            if (!Uri.TryCreate(headerValue, UriKind.Absolute, out uri))
            {
                return null;
            }
            var hostName = uri.Host.ToLower();

            return hostName;
        }

        public static IOwinContext Set<T>(this IOwinContext context, T value)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return context.Set(GetKey(typeof(T)), value);
        }

        public static T Get<T>(this IOwinContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return context.Get<T>(GetKey(typeof(T)));
        }

        private static string GetKey(Type t)
        {
            return t.FullName;
        }
    }
}
