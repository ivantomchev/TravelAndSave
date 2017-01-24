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
        public IHttpActionResult GetPenis()
        {
            var res = this.locationsService.GetAll();
            if (res.IsFailure)
            {
                return Content(HttpStatusCode.InternalServerError, "Fuck this shit");
            }

            return Ok(res.Value);
        }
    }
}