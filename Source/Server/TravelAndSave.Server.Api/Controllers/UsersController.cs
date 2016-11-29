namespace TravelAndSave.Server.Api.Controllers
{
    using Base;
    using Data.Repositories;
    using System.Linq;
    using System.Web.Http;

    [RoutePrefix("api/users")]
    public class UsersController : BaseController
    {
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var repo = new UsersRepository();

            var result = repo.All().ToList();

            return Ok(result);
        }
    }
}