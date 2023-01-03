using BOTY.Models.zabezpečení;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BOTY.Controllers
{
    public class BaseController : Controller
    {
        public UsersService usersService = new UsersService();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.ViewBag.Authenticated = this.HttpContext.Session.GetString("login") != null;

            base.OnActionExecuting(context);
        }
    }
}
