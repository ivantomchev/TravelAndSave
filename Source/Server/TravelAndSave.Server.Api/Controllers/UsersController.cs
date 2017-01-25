namespace TravelAndSave.Server.Api.Controllers
{
    using Base;
    using System.Web.Http;
    using Services.Data.Interfaces;
    using Services.Data;
    using System.Net;

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
    }
}