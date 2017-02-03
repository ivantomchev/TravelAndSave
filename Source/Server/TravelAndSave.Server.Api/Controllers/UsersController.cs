namespace TravelAndSave.Server.Api.Controllers
{
    using Base;
    using System.Web.Http;
    using Services.Data.Interfaces;
    using Services.Data;
    using System.Net;
    using Infrastructure.Attributes;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Linq;

    [RoutePrefix("api/users")]
    public class UsersController : BaseController
    {
        private readonly ILocationsService locationsService;

        public UsersController()
        {
            this.locationsService = new LocationsService();
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Ok();
        }

        [HttpGet]
        [Route("test")]
        public IHttpActionResult GetData()
        {
            var locationsResult = this.locationsService.GetAll();
            if (locationsResult.IsFailure)
            {
                return Content(HttpStatusCode.InternalServerError, locationsResult.ErrorMessage);
            }

            return Ok(locationsResult.Value);
        }

        [HttpPost]
        [Route("test2")]
        [MultipartFormDataValidation(AllowedFileExtensions = "txt,pdf,json", AllowMultipleFiles = true, MaxFileSize = 2000)]
        //[ImageValidation(MaxFileSize = 865, AllowedFileExtensions = "jpeg,jpg,png", AllowMultipleFiles = true, MaxHeight = 1600, MaxWidth = 1586)]
        public async Task<IHttpActionResult> GetData2()
        {
            var res = await this.Request.Content.ReadAsMultipartAsync();

            var bytes = await res.Contents.FirstOrDefault().ReadAsByteArrayAsync();

            return Ok(bytes.Length / 1024);
        }
    }
}