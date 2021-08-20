using Mastercard.Common.DTO.Request.MarketPlace;
using Mastercard.UI.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mastercard.Common.DTO.Response.MarketPlace;
using Mastercard.Common.Wrapper;

namespace Mastercard.UI.Interface.Controllers
{
    public class MasterController : Controller
    {
        private readonly IMasterBL _masterBl;
        private readonly IMasterQuantumBl _masterQuantumBl;

        public MasterController(IMasterBL masterBl, IMasterQuantumBl masterQuantumBl)
        {
            _masterBl = masterBl;
            _masterQuantumBl = masterQuantumBl;
        }

        #region Master

        /// <summary>
        /// Obtiene departamentos por Country. (PasalaGanando db).
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDepartments()
        {
            var listData = new List<SelectListItem>();
            var departments = _masterBl.GetDeparmentsByCountry(1);
            if (departments is null || !departments.IsSuccess || departments.Result is null) return Json(listData);

            listData.Add(new SelectListItem() { Text = "-Seleccione-", Value = "-1" });
            listData.AddRange(departments.Result.Select(item => new SelectListItem
            {
                Text = item.Name.ToString(),
                Value = item.Id.ToString()
            }).OrderBy(c => c.Text));
            return Json(listData);
        }

        /// <summary>
        /// Obtiene departamentos por Country. (PasalaGanando db).
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCityPasalaGanandoByDepartmentId(string departmentId)
        {
            var listData = new List<SelectListItem>();
            var objDataResult = _masterBl.GetCitiesByDeparmentId(Convert.ToInt32(departmentId));
            if (objDataResult is null || !objDataResult.IsSuccess || objDataResult.Result is null) return Json(listData);

            listData.Add(new SelectListItem() { Text = "-Seleccione-", Value = "-1" });
            listData.AddRange(objDataResult.Result.Select(item => new SelectListItem
            {
                Text = item.Name.ToString(),
                Value = item.Id.ToString()
            }).OrderBy(c => c.Text));
            return Json(listData);
        }

        /// <summary>
        /// Obtiene ciudades para quantum
        /// </summary>
        /// <param name="depto"></param>
        /// <param name="brandId"></param>
        /// <returns></returns>
        [HttpPost]
        public  JsonResult GetCityByDepartmentId(string depto, string brandId)
        {

            var request = new GetMasterRequest
            {
                brand_id = brandId.ToString(),
                dep_id = depto
            };
            var citiesResponse = new Response<List<CityQuantumResponse>>();
            Task.Run(async () =>
            {
                citiesResponse = await _masterQuantumBl.GetCity(request);
            }).GetAwaiter().GetResult();

            var listCity = new List<SelectListItem>();
            var cities = citiesResponse.Result;

            if (cities is null) return Json(listCity);


            listCity.Add(new SelectListItem() { Text = "-Seleccione-", Value = "-1" });
            listCity.AddRange(cities.Select(city => new SelectListItem
            {
                Text = city.city_name.ToString(),
                Value = city.city_id.ToString()
            }).OrderBy(c => c.Text));
            return Json(listCity);
        }

        [HttpPost]
        public  JsonResult GetSiteByCityId(string deptoId, string brandId, string cityId)
        {
            var request = new GetMasterRequest
            {
                brand_id = brandId.ToString(),
                dep_id = deptoId,
                city_id = cityId
            };
            var listSite = new List<SelectListItem>();
            var sitesResponse = new Response<List<SiteQuantumResponse>>();
            Task.Run(async () =>
            {
                sitesResponse = await _masterQuantumBl.GetSite(request);
            }).GetAwaiter().GetResult();

            var sites = sitesResponse.Result;
            if (sites is null) return Json(listSite);
            listSite.Add(new SelectListItem() { Text = "--Establecimiento--", Value = "-1" });
            listSite.AddRange( sites.Select(site => new SelectListItem
            {
                Text = site.Site_name.ToString(),
                Value = site.Site_id.ToString()
            }).OrderBy(c => c.Text));
            return Json(listSite);
        }

        #endregion Master
    }
    internal static class AsyncHelper
    {
        private static readonly TaskFactory _myTaskFactory = new
          TaskFactory(CancellationToken.None,
                      TaskCreationOptions.None,
                      TaskContinuationOptions.None,
                      TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncHelper._myTaskFactory
              .StartNew<Task<TResult>>(func)
              .Unwrap<TResult>()
              .GetAwaiter()
              .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            AsyncHelper._myTaskFactory
              .StartNew<Task>(func)
              .Unwrap()
              .GetAwaiter()
              .GetResult();
        }
    }


}