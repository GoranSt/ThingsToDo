using System.Web.Mvc;

namespace ThingsToDo.App.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("~/Account/Login");
            }

            return View();
        }
    }
}