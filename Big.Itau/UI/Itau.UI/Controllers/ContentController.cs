using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using System;
using System.Web.Mvc;

namespace Mastercard.UI.Interface.Controllers
{
    public class ContentController : Controller
    {
        #region Properties

        private readonly IUserState _userState;

        #endregion Properties

        public ContentController(IUserState userState)
        {
            _userState = userState;
        }

        // GET: Content

        public ActionResult HowToParticipate()
        {
            return View();
        }

        public ActionResult Loading()
        {
            return View();
        }

        public ActionResult LoadGoal()
        {
            if (string.IsNullOrEmpty(SessionHelper.Token))
            {
                return RedirectToAction("LogOut", "Account");
            }
            var getGoalResponse = this._userState.GetGoal();

            if (getGoalResponse.IsSuccess) return PartialView($"~/Views/Content/_Goal.cshtml");
            if (!string.IsNullOrEmpty(getGoalResponse.RedirectTo))
            {
                TempData["StartUpScript"] = getGoalResponse.Message;
                return RedirectToRoute(getGoalResponse.RedirectTo);
            }

            Common.Diagnostics.ExceptionLogging.LogException(
                new Exception($"Error en LoadingGoal{getGoalResponse.Message}"));
            return PartialView($"~/Views/Shared/Page500.cshtml");
        } 
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult FaQ()
        {
            return View();
        }
    }
}