using System.Web.Mvc;

namespace Mastercard.UI.Interface.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}