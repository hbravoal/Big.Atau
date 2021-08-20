using Mastercard.Common.Enums;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Mastercard.Common.Utils.UtilitiesCommon;

namespace Mastercard.UI.Interface.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        #region Properties

        private readonly ICatalogBL _customerCatalog;

        #endregion Properties

        #region Constructor

        public CatalogController(ICatalogBL customerCatalog)
        {
            _customerCatalog = customerCatalog;
        }

        async public Task<ActionResult> Index()
        {
            if (string.IsNullOrEmpty(User.Identity.Name) || string.IsNullOrEmpty(SessionHelper.Token))
            {
                return RedirectToAction("LogOut", "Account");
            }
            if (TempData["StartUpScript"] != null)
            {
                string solve = TempData["StartUpScript"].ToString();
                ViewBag.StartUpScript = solve;
            }
            if (SessionHelper.Token != null)
            {
                var result = await _customerCatalog.GetCatalogProducts();

                return View(result.Result);
            }
            else
                return RedirectToAction("LogOut", "Account");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a">Será el Guid del Producto encriptado.</param>
        /// <param name="h">Será el tipo del Producto. Enumerable</param>
        /// <returns></returns>
        async public Task<ActionResult> Detail(string a, string h)
        {
            if (TempData["StartUpScript"] != null)
            {
                string solve = TempData["StartUpScript"].ToString();
                ViewBag.StartUpScript = solve;
            }
            if (SessionHelper.Token != null)
            {
                try
                {
                    var type = (EnumCatalogNetCommerceType)Convert.ToInt32(Cryptrography.Decrypt(h));
                    var productGuid = Cryptrography.Decrypt(a);
                    var result = await _customerCatalog.GetProduct(productGuid, type);
                    if (result == null)
                        return RedirectToAction(nameof(Index), "Catalog");

                    return View(result.Result);
                }
                catch (System.Exception)
                {
                    return RedirectToAction(nameof(Index), "Catalog");
                }
            }
            else
                return RedirectToAction("LogOut", "Account");
        }

        #endregion Constructor
    }
}