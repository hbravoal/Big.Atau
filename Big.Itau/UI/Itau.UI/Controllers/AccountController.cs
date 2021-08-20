using Mastercard.Common.DTO.Response;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using Mastercard.UI.Business.ViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace Mastercard.UI.Interface.Controllers
{
    public class AccountController : Controller
    {
        #region Properties

        private readonly IAccountBl _clientLogin;
        private readonly IAccountFirst _accountFirst;
        private readonly IUserLogins _userLogins;
        private readonly ICatpchaValidate _catpchaValidate;
        private readonly IAuthentication _authentication;

        #endregion Properties

        #region Constructor

        public AccountController(IAccountBl clientLogin, IUserLogins userLogins, IAccountFirst accountFirst,
            ICatpchaValidate catpchaValidate, IAuthentication authentication)
        {
            _clientLogin = clientLogin;
            _accountFirst = accountFirst;
            _catpchaValidate = catpchaValidate;
            _authentication = authentication;
            _userLogins = userLogins;
        }

        #endregion Constructor

        #region Views

        public ActionResult FirstLogin()
        {
            _authentication.Login();

            ViewBag.Error = TempData["StartUpScript"];
            SessionHelper.LogOut();
            return View();
        }

        #endregion Views

        /// <summary>
        /// Login del sitio.
        /// V16: Solo los usuarios de algunos segmentos:AppSettings podrán ingresar al sitio, de lo contrario serán redireccionado a otra página (Black).
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ReturnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<System.Web.Mvc.ActionResult> FirstLogin(ProfileModelViewModel model, string ReturnUrl)
        {
            try
            {
                var catpchaResponse = Request.Form["g-recaptcha-response"];
                if (string.IsNullOrEmpty(catpchaResponse))
                {
                    ModelState.AddModelError("Recaptcha", "El captcha es requerido.");
                }   
                if (string.IsNullOrEmpty(model.Identification))
                {
                    ModelState.AddModelError("Identification", "Tu identificación es requerida.");
                }
                if (model.AcepTyc == false)
                {
                    ModelState.AddModelError("Terminos", "Debes aceptar los términos y condiciones.");
                }
                string ipclient = HttpContext.Request.UserHostAddress;
                if (ModelState.IsValid)
                {
                    var tokenResponseCend = _accountFirst.LoginByIdentification(model.Identification, model.AcepTyc, ipclient);
                    try
                    {
                        await _authentication.Login();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    if (!tokenResponseCend.IsSuccess)
                    {
                        ModelState.AddModelError("Identification", $"Esta campaña está dirigida únicamente a clientes Bancolombia con Tarjetas Maestro o Mastercard, débito o crédito, que hayan recibido la comunicación de bienvenida. Si tienes inconvenientes con el ingreso comunícate con la línea de atención 01-800-0912345.");
                        return View(model);
                    }

                    LoginCustomerForIdentificationResponse response = (LoginCustomerForIdentificationResponse)tokenResponseCend.Result;
                    var countLoginResponse = _userLogins.CountLogins(SessionHelper.Guid);
                    int countLogins = 0;
                    if (countLoginResponse != null || countLoginResponse.IsSuccess)
                    {
                        countLogins = countLoginResponse.Result;
                    }

                    //try
                    //{
                    // marketPlaceTask.Wait(); // Esperamos la tarea.
                    //}
                    //catch (Exception ex)
                    //{
                    //    Common.Diagnostics.ExceptionLogging.LogException(ex);
                    //}
                    FormsAuthentication.SetAuthCookie(SessionHelper.Token, true);

                    var returnUrl = ReturnUrl;
                    bool fisrtLogin = false;
                    if (Convert.ToInt32(countLogins) <= 1)
                    {
                        fisrtLogin = true;
                        return RedirectToAction($"Loading", $"Content");
                    }
                    SessionHelper.FirstLogin = fisrtLogin;
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Common.Diagnostics.ExceptionLogging.LogException(ex);
                throw;
            }
        }

        public ActionResult LogOut()
        {
            SessionHelper.LogOut();
            return RedirectToAction("FirstLogin", "Account");
        }
    }
}