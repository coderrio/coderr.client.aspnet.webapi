using System.Web.Mvc;

namespace Coderr.Client.AspNet.WebApi.Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
