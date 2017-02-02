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
        //[MultipartFormDataValidation]
        [ImageValidation(MaxFileSize = 1000, MaxWidth = 1024, MaxHeight = 768, AllowedFileExtensions = "jpeg,png", AllowMultipleFiles = false)]
        public async Task<IHttpActionResult> GetData2()
        {

            var res = await this.Request.Content.ReadAsMultipartAsync();

            return Ok();

            var locationsResult = this.locationsService.Detete(1, true);
            if (locationsResult.IsFailure)
            {
                return Content(HttpStatusCode.InternalServerError, locationsResult.ErrorMessage);
            }

            return Ok();
        }
    }
}