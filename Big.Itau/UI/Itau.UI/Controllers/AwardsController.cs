using Mastercard.Common.DTO.Response;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using Mastercard.UI.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Mastercard.UI.Interface.Controllers
{
    [Authorize]
    public class AwardsController : Controller
    {
        private readonly IAwardBL _awardBL;

        public AwardsController(IAwardBL awardBL)
        {
            _awardBL = awardBL;
        }

        // GET: Awarads
        [Authorize]
        public ActionResult Index(int flag = 0)
        {
            if (string.IsNullOrEmpty(SessionHelper.Token))
            {
                return RedirectToAction("LogOut", "Account");
            }
 
            AwardViewModel awardView = new AwardViewModel();
            try
            {
                if (string.IsNullOrEmpty(User.Identity.Name) || string.IsNullOrEmpty(SessionHelper.Token))
                {

                    return RedirectToAction("LogOut", "Account");
                }
                var Award = _awardBL.GetAward();
                if (Award.IsSuccess)
                {
                    awardView.Awards = (List<AwardDTO>)Award.Result;

                    if (flag == 1)
                    {
                        ViewBag.StartUpScript = $"showMaskError('Ten presente que solo podrás hacer este cambio una vez al día y podrás cambiar la selección del premio por el que estás participando hasta el {awardView.Awards.FirstOrDefault().EndDate.ToString("dd/MM/yyyy")}.')";
                    }
                    if (flag == 2)
                    {
                        ViewBag.StartUpScript = $"showMaskError('Ya pasó la fecha límite para cambiar la selección del premio final por el que quieres participar.')";
                    }

                    return View(awardView);
                }
                if (Award.Message == "NoToken")
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
        public ActionResult Detail(short awardId = 0)
         {
            AwardViewModel awardView = new AwardViewModel();
            if (string.IsNullOrEmpty(SessionHelper.Token))
            {
                return RedirectToAction("LogOut", "Account");
            }
            if (!SessionHelper.LoginComplete)
            {
                return RedirectToAction("Index", "Profile");

            }
            

            if (awardId == 0)
            {
                awardView.message = $"Debe seleccionar un producto";
                return View(awardView);
            }
            var Award = _awardBL.GetAwardById(awardId);
            if (Award.IsSuccess)
            {
                awardView.Awards = (List<AwardDTO>)Award.Result;

                return View(awardView);
            }
            return Redirect("~/error");
        }
        
        public ActionResult SaveSelection(short awardId = 0)
        {
            AwardViewModel awardView = new AwardViewModel();
            if (string.IsNullOrEmpty(SessionHelper.Token))
            {
                return RedirectToAction("LogOut", "Account");
            }
            if (awardId == 0)
            {
                awardView.message = $"Debe seleccionar un producto";

                return View(awardView);
            }
            var saveAward = _awardBL.SaveAwards(awardId);
            if (saveAward.IsSuccess)
            {
                return RedirectToAction("Index", "Awards");
                
            }
            if (saveAward.Message == "NoToken")
            {
                return RedirectToAction("LogOut", "Account");
            }
            if (saveAward.Message == "ChageLock")
            {
                return RedirectToAction("Index", "Awards", new {flag = 1 });
            }
            if (saveAward.Message == "DateLimit")
            {
                return RedirectToAction("Index", "Awards", new { flag = 2 });
            }


            return Redirect("~/error");
        }
    }
}