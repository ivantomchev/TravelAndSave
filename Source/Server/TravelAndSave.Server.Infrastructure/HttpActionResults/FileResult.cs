namespace TravelAndSave.Server.Infrastructure.HttpActionResults
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class FileResult : IHttpActionResult
    {
        private const string ContentDispositionHeaderValue = "attachment";

        private readonly byte[] bytes;
        private readonly string fileName;
        private readonly string mediaTypeHeaderValue;

        public FileResult(byte[] bytes, string fileName, string mediaTypeHeaderValue)
        {
            this.bytes = bytes;
            this.fileName = fileName;
            this.mediaTypeHeaderValue = mediaTypeHeaderValue;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(this.bytes);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue(this.mediaTypeHeaderValue);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(ContentDispositionHeaderValue)
            {
                FileName = fileName
            };

            return Task.FromResult(result);
        }
    }
}
