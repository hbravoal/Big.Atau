using Mastercard.Common.DTO;
using Mastercard.Common.DTO.Request;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using Mastercard.UI.Business.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Mastercard.UI.Interface.Controllers
{
    [Authorize]
    public class MisionController : Controller
    {
        private readonly IMisionBL _misionBL;

        public MisionController(IMisionBL misionBL)
        {
            _misionBL = misionBL;
        }

        // GET: Mision  
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                if (string.IsNullOrEmpty(User.Identity.Name) || string.IsNullOrEmpty(SessionHelper.Token))
                {
                    return RedirectToAction("LogOut", "Account");
                }
                var misiones = _misionBL.GetMisionCustomer();

                if (misiones.IsSuccess)
                {
                    var misionsViewModels = new MisionViewModels
                    {
                        MisionPoint = (MisionPointDTO)misiones.Result
                    };
                    if (misionsViewModels.MisionPoint.MisionsTarget.Count() > 0)
                    {
                        SessionHelper.ChellengeCheck = true; 
                    }
                    return View(misionsViewModels);
                }
                if (misiones.Message == "NoToken")
                {
                    return RedirectToAction("LogOut","Account");
                }
                return Redirect("~/Error");


            }
            catch (Exception ex)
            {
                Common.Diagnostics.ExceptionLogging.LogException(ex);
                return Redirect("~/Error");
            }
        }


        [Authorize]
        public ActionResult Detail(int IdMsion)
        {
            if (string.IsNullOrEmpty(SessionHelper.Token))
            {
                return RedirectToAction("LogOut", "Account");
            }
           
            var misiones = _misionBL.GetMisionById(IdMsion);

            if (misiones.IsSuccess)
            {
                var misionsViewModels = new MisionViewModels
                {
                    MisionPoint = (MisionPointDTO)misiones.Result
                };

                return View(misionsViewModels);
            }
            return Redirect("~/Error");
        }
        [Authorize]
        [HttpPost]
        public ActionResult RedemptionCupon(int misisonId = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(SessionHelper.Token))
                {
                    return RedirectToAction("LogOut","Account");
                }
                var misiones = _misionBL.RedemptionPoint();

                if (misiones.IsSuccess)
                {
                    RedemptionPointInformation redemption = new RedemptionPointInformation();
                    
                    redemption = (RedemptionPointInformation)misiones.Result;
                    var misionsViewModels = new MisionViewModels
                    {
                        CuponsAcumulate = redemption.PointAccumulation,
                        MisionPoint2 = redemption.PointMision
                       
                    };

                    return PartialView("~/Views/Shared/Modals/_ModalRedemCupon.cshtml", misionsViewModels);
                }
                if (misiones.Message == "NoCanRedeem")
                {
                    return PartialView("~/Views/Shared/Modals/_ModalNoRedeem.cshtml");
                }
                return PartialView("~/Views/Shared/Modals/_ModalError.cshtml");


            }
            catch (Exception ex)
            {
                Common.Diagnostics.ExceptionLogging.LogException(ex);
                return Redirect("~/Error");
            }
        }
    }
}