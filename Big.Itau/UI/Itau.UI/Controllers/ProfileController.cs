using Mastercard.Common.Helpers;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using Mastercard.UI.Interface.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace Mastercard.UI.Interface.Controllers
{
    public class ProfileController : Controller
    {
        #region Properties

        private readonly IUserState _userState;
        private readonly IAccountBl _clientLogin;

        #endregion Properties

        #region Constructor

        public ProfileController(IUserState userState, IAccountBl clientLogin)
        {
            _userState = userState;
            _clientLogin = clientLogin;
            //_userStateDetail = userStateDetail;
            //_missionBl = missionBl;
            //_clientPfm = clientPfm;
        }

        #endregion Constructor

        [Authorize]
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(SessionHelper.Token))
            {
                return RedirectToAction("LogOut", "Account");
            }
            
            Response response;
            try
            {
                if (string.IsNullOrEmpty(User.Identity.Name)|| string.IsNullOrEmpty(SessionHelper.Token))
                {
                    
                    return RedirectToAction("LogOut","Account");
                }
                response = _userState.GetGoal();

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Common.Diagnostics.ExceptionLogging
                        .LogInfo($"Se Cerró sesión para el IdMask:{SessionHelper.IdMask}| UserIdentity{User.Identity.Name}| Token: {SessionHelper.Token} | Response: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Message)} | Response.Result: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Result)}");
                    return Redirect("~/Account/FirstLogin");
                }
                if (!response.IsSuccess)
                {
                    Common.Diagnostics.ExceptionLogging.LogException(new Exception
                        ($"Ocurrió un error al intentar Refrescar Profile para el IdMask:{SessionHelper.IdMask}| UserIdentity{User.Identity.Name}| Token: {SessionHelper.Token} | Response: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Message)} | Response.Result: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Result)}"));
                    return Redirect("~/Error");
                }
            }
            catch (Exception ex)
            {
                Common.Diagnostics.ExceptionLogging.LogException(new Exception($"Ocurrió un error al intentar Gestionar(Refrescar) Profile Ex:{ex}")); ;
                return Redirect("~/Error");
            }

            if (TempData["StartUpScript"] != null)
                ViewBag.StartUpScript = TempData["StartUpScript"].ToString();

            return View(response.Result);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SecondLoginCard(CodeViewModel model, string page)
        {
            try
            {
                if (string.IsNullOrEmpty(SessionHelper.Token))
                {
                    return RedirectToAction("LogOut", "Account");
                }
                string returnUrl = "";

                var catpchaResponse = Request.Form["g-recaptcha-response"];
                if (string.IsNullOrEmpty(catpchaResponse))
                {
                    ModelState.AddModelError("Recaptcha", "El captcha es requerido.");
                }

                string cardNumber = model.Number1 + model.Number2 + model.Number3 + model.Number4;

                if (cardNumber == null || string.IsNullOrEmpty(cardNumber))
                {
                    ModelState.AddModelError("CardNumber", "Si cambiaste recientemente tu tarjeta, ingresa con los últimos 4 dígitos de la anterior o espera 15 días mientras actualizamos tus datos.");
                    SessionHelper.Page = page;
                }

                if (!ModelState.IsValid)
                {
                    return PartialView("~/Views/Shared/Modals/SecondLogin.cshtml", model);
                }
                var loginNResponse = _clientLogin.Login(SessionHelper.Identification, cardNumber);
                if (!loginNResponse.IsSuccess)
                {
                    if (loginNResponse.Message == "TokenInvalid") {
                        var scriptOut = "window.location.href = '" + Url.Action("LogOut", "Account") + "'";
                        return new JavaScriptResult() { Script = scriptOut };
                    }
                    
                    ModelState.AddModelError("CardNumber", $"Si tienes inconvenientes con el ingreso, comunícate con la línea de atención 01-800-0912345, o consulta en Preguntas frecuentes para conocer si eres un participante.");
                    model = null;
                    SessionHelper.Page = page;
                    return PartialView("~/Views/Shared/Modals/SecondLogin.cshtml", model);
                    
                }
                var script = "";
                if (page == "R" || SessionHelper.Page == "R")
                {
                     script = "window.location.href = '" + Url.Action("MyRedemption", "Redemption") + "'";
                }
                if (page == "C" || SessionHelper.Page == "C")
                {
                    script = "window.location.href = '" + Url.Action("Index", "Catalog") + "'";
                }
                if (page == "A" || SessionHelper.Page== "A")
                {
                    script = "window.location.href = '" + Url.Action("Index", "Awards") + "'";
                }
                if (page == "P" || SessionHelper.Page == "P")
                {
                    script = "window.location.href = '" + Url.Action("Index", "Profile") + "'";
                }


                return new JavaScriptResult() { Script = script };
            }
            catch (Exception ex)
            {
                Common.Diagnostics.ExceptionLogging.LogException(ex);
                return Redirect("~/Error");
            }
            
        }
    }
}