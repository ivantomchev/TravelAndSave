namespace TravelAndSave.Server.Infrastructure.Attributes
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class MultipartFormDataValidationAttribute : ActionFilterAttribute
    {
        public double MaxFileSize { get; set; } = double.MaxValue;
        public string[] AllowedFileExtensions { get; set; }
        public bool AllowMultipleFiles { get; set; } = false;

        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var request = actionContext.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Unsupported MediaType");
                return;
            }

            StreamContent streamContent = await this.GetBufferedStreamContent(request.Content);

            var provider = await streamContent.ReadAsMultipartAsync();

            var files = provider.Contents.Where(x => x.Headers.ContentType != null);
            if (files.Count() == 0)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "File is required.");
                return;
            }

            if (this.AllowMultipleFiles == false && files.Count() > 1)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Uploading multiple files is not allowed.");
                return;
            }

            var fileExtensions = files.Select(f => GetFileExtension(f.Headers.ContentDisposition.FileName.Trim('\"'))?.ToLower());
            if (AllowedFileExtensions != null && AllowedFileExtensions.Length > 0 && fileExtensions.Any(fe => !AllowedFileExtensions.Contains(fe)))
            {
                var errorMessage = string.Format("Allowed file extensions are {0}.", string.Join(", ", AllowedFileExtensions));
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                return;
            }

            var byteArrays = await this.ReadAsByteArraysAsync(files.ToArray());
            if (byteArrays.Any(b => MaxFileSize < (double)b.Length / 1024 / 1024))
            {
                var errorMessage = string.Format("Max file size allowed is {0} Mb.", MaxFileSize);
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                return;
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        private string GetFileExtension(string fullName)
        {
            var pointIndex = fullName.LastIndexOf('.');
            if (pointIndex == -1)
            {
                return string.Empty;
            }

            var extension = fullName.Substring(pointIndex, fullName.Length - pointIndex);
            return extension.TrimStart('.');
        }

        private async Task<IEnumerable<byte[]>> ReadAsByteArraysAsync(params HttpContent[] bytes)
        {
            var filesBytesTasks = new List<Task<byte[]>>();
            foreach (var file in bytes)
            {
                var currentTask = file.ReadAsByteArrayAsync();
                filesBytesTasks.Add(currentTask);
            }

            return await Task.WhenAll(filesBytesTasks);
        }

        private async Task<StreamContent> GetBufferedStreamContent(HttpContent httpContent)
        {
            MemoryStream buffer = new MemoryStream();
            StreamWriter writer = new StreamWriter(buffer);
            await writer.WriteAsync(await httpContent.ReadAsStringAsync());
            await writer.FlushAsync();
            buffer.Position = 0;

            StreamContent streamContent = new StreamContent(buffer);

            foreach (var header in httpContent.Headers)
            {
                streamContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return streamContent;
        }
    }
}
