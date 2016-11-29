namespace TravelAndSave.Server.Infrastructure.Attributes
{
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.Any(p => p.Value == null && Nullable.GetUnderlyingType(p.GetType()) != null))
            {
                actionContext.ModelState.AddModelError(string.Empty, "Request cannot be empty.");
            }

            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "**VALIDATION ERROR**: " + JsonConvert.SerializeObject(modelState));
            }
        }
    }
}
