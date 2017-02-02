namespace TravelAndSave.Server.Infrastructure.Attributes
{
    using Common.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class ImageValidationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Maximum image height in px
        /// </summary>
        public uint MaxHeight { get; set; }

        /// <summary>
        /// Minimum image height in px
        /// </summary>
        public uint MinHeight { get; set; }

        /// <summary>
        /// Maximum image width in px
        /// </summary>
        public uint MaxWidth { get; set; }

        /// <summary>
        /// Minimum image width in px
        /// </summary>
        public uint MinWidth { get; set; }

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

            StreamContent streamContent = await this.GetBufferedStreamContentAsync(request.Content);

            MultipartMemoryStreamProvider provider = null;
            try
            {
                provider = await streamContent.ReadAsMultipartAsync();
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

            //Validate that all the files are images
            var isAnyNonImageFile = files.Any(f => f.Headers.ContentType.MediaType.Substring(0, f.Headers.ContentType.MediaType.IndexOf("/")) != "image");
            if (isAnyNonImageFile)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Only image files are allowed.");
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

            //Validate image/images allowed resolution
            if (this.MaxHeight != 0 || this.MinHeight != 0 || this.MaxWidth != 0 || this.MinWidth != 0)
            {
                var images = await this.GetImagesFromContentAsync(files.ToArray());
                if (this.MaxHeight != 0 && images.Any(i => i.Height > this.MaxHeight))
                {
                    var errorMessage = string.Format("The required maximum image height is {0}.", this.MaxHeight);
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    return;
                }
                if (this.MinHeight != 0 && images.Any(i => i.Height < this.MinHeight))
                {
                    var errorMessage = string.Format("The required minimum image height is {0}.", this.MinHeight);
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    return;
                }
                if (this.MaxWidth != 0 && images.Any(i => i.Width > this.MaxWidth))
                {
                    var errorMessage = string.Format("The required maximum image width is {0}.", this.MaxWidth);
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    return;
                }
                if (this.MinWidth != 0 && images.Any(i => i.Width < this.MinWidth))
                {
                    var errorMessage = string.Format("The required minimum image width is {0}.", this.MinWidth);
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorMessage);
                    return;
                }
            }

            await base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        private async Task<IEnumerable<byte[]>> ReadAsByteArraysAsync(params HttpContent[] httpContent)
        {
            var filesBytesTasks = new List<Task<byte[]>>();
            foreach (var content in httpContent)
            {
                var currentTask = content.ReadAsByteArrayAsync();
                filesBytesTasks.Add(currentTask);
            }

            return await Task.WhenAll(filesBytesTasks);
        }

        private async Task<StreamContent> GetBufferedStreamContentAsync(HttpContent httpContent)
        {
            var httpContentAsString = await httpContent.ReadAsStringAsync();

            MemoryStream buffer = new MemoryStream();
            StreamWriter writer = new StreamWriter(buffer);
            await writer.WriteAsync(httpContentAsString);
            await writer.FlushAsync();
            buffer.Position = 0;

            StreamContent streamContent = new StreamContent(buffer);

            foreach (var header in httpContent.Headers)
            {
                streamContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return streamContent;
        }

        private async Task<IEnumerable<Image>> GetImagesFromContentAsync(params HttpContent[] httpContent)
        {
            var tasks = httpContent.Select(async content =>
            {
                var stream = await content.ReadAsStreamAsync();
                return Image.FromStream(stream);
            });

            var images = await Task.WhenAll(tasks);

            return images;
        }
    }
}