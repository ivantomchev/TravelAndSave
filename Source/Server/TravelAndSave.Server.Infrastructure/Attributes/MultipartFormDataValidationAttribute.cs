namespace TravelAndSave.Server.Infrastructure.Attributes
{
    using Common.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class MultipartFormDataValidationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Maximum file size in KB
        /// </summary>
        public uint MaxFileSize { get; set; }

        /// <summary>
        /// Comma separated file extensions allowed for upload
        /// </summary>
        public string AllowedFileExtensions { get; set; }

        /// <summary>
        /// Allowing multiple file upload
        /// </summary>
        public bool AllowMultipleFiles { get; set; }

        public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var request = actionContext.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Unsupported MediaType");
                return;
            }

            var maxApplicationRequestLength = this.GetApplicationMaxRequestLength();
            if (maxApplicationRequestLength < request.Content.Headers.ContentLength / 1024)
            {
                var errorMessage = string.Format("Maximum request length for the application is limited to {0} Kb.", maxApplicationRequestLength);
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessage);
                return;
            }

            StreamContent streamContent = await this.GetBufferedStreamContentAsync(request.Content);

            MultipartMemoryStreamProvider provider = new MultipartMemoryStreamProvider();
            try
            {
                await streamContent.ReadAsMultipartAsync(provider);
            }
            catch (IOException ex)
            {
                var message = ex.Message;
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Unsupported MediaType");
                return;
            }

            var files = provider.Contents.Where(x => x.Headers.ContentType != null);
            if (!files.Any())
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "File is required.");
                return;
            }

            if (this.AllowMultipleFiles == false && files.Count() > 1)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Uploading multiple files is not allowed.");
                return;
            }

            //Validate allowed file extensions
            if (!string.IsNullOrEmpty(this.AllowedFileExtensions))
            {
                var allowedFileExtensionsAsArray = this.AllowedFileExtensions.ToLower().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var fileExtensions = files.Select(f => f.Headers.ContentDisposition.FileName.GetFileExtension().Trim('\"')?.ToLower());
                if (fileExtensions.Any(fe => !allowedFileExtensionsAsArray.Contains(fe)))
                {
                    var errorMessage = string.Format("Allowed file extensions are {0}.", string.Join(", ", AllowedFileExtensions));
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    return;
                }
            }

            //Validate allowed file maximum size
            if (this.MaxFileSize != 0)
            {
                var byteArrays = await this.ReadAsByteArraysAsync(files.ToArray());
                if (byteArrays.Any(b => MaxFileSize < b.Length / 1024))
                {
                    var errorMessage = string.Format("Max file size allowed is {0} Kb.", MaxFileSize);
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    return;
                }
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        private async Task<IEnumerable<byte[]>> ReadAsByteArraysAsync(params HttpContent[] httpContents)
        {
            var tasks = httpContents.Select(async content =>
            {
                return await content.ReadAsByteArrayAsync();
            });

            return await Task.WhenAll(tasks);
        }

        private async Task<StreamContent> GetBufferedStreamContentAsync(HttpContent httpContent)
        {
            await httpContent.LoadIntoBufferAsync();

            var buffer = new MemoryStream();
            await httpContent.CopyToAsync(buffer);
            buffer.Position = 0;

            StreamContent streamContent = new StreamContent(buffer);

            foreach (var header in httpContent.Headers)
            {
                streamContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return streamContent;
        }

        private int GetApplicationMaxRequestLength()
        {
            var section = ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            if (section != null)
            {
                return section.MaxRequestLength;
            }
            return 4096;
        }
    }
}
