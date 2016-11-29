namespace TravelAndSave.Server.Infrastructure.Extensions
{
    using HttpActionResults;
    using System.Web.Http;

    public static class ApiControllerExtensions
    {
        public static IHttpActionResult FileResult(this ApiController apiController, byte[] bytes, string fileName, string mediaTypeHeaderValue)
        {
            return new FileResult(bytes, fileName, mediaTypeHeaderValue);
        }
    }
}
