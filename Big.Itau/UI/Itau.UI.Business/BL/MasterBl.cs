using Mastercard.Business.Services;
using Mastercard.Common.DTO.Request.MarketPlace;
using Mastercard.Common.DTO.Response;
using Mastercard.Common.DTO.Response.MarketPlace;
using Mastercard.Common.Helpers;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Mastercard.UI.Business.BL
{
    /// <summary>
    /// Clase BL para Maestras
    /// </summary>
    public class MasterBl : IMasterQuantumBl, IMasterBL
    {
        private readonly MasterServices _masterServices = new MasterServices();

        public Response<List<CityDTO>> GetCitiesByDeparmentId(int departmentId) =>
            _masterServices.GetCitiesByDeparmentId((departmentId));

        public Response<List<DepartmentDTO>> GetDeparmentsByCountry(int countryId) =>
            _masterServices.GetDeparmentsByCountry((countryId));

        #region Quantum

        /// <summary>
        /// Obtiene departamentos en Quantum
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response<List<DepartmentQuantumResponse>>> GetDepartment(GetMasterRequest model)
        {
            try
            {
                var response = new Response<List<DepartmentQuantumResponse>>();

                var headers = new Dictionary<string, string>();
                headers.Add("user", ConfigurationManager.AppSettings["Quantum.Configuration.UserName"]);
                headers.Add("token", ConfigurationManager.AppSettings["Quantum.Configuration.Password"]);
                headers.Add("ContentType", "application/json");
                var CatalogResponse = await ApiService.PostAsync<QuantumDeparmentResponse>
                    (ConfigurationManager.AppSettings["Quantum.Proxy.Address"],
                    $"{ConfigurationManager.AppSettings["Quantum.Department.Proxy.Action"]}", headers,
                    model);

                if (CatalogResponse == null || CatalogResponse.Result == null || CatalogResponse.Result.response is null || CatalogResponse.Result.response.message is null)
                {
                    response.Result = new List<DepartmentQuantumResponse>();
                    return response;
                }
                response.Result = CatalogResponse.Result.response.message.ToList();
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("");
            }
        }

        public async Task<Response<List<CityQuantumResponse>>> GetCity(GetMasterRequest model)
        {
            try
            {
                var response = new Response<List<CityQuantumResponse>>();

                var headers = new Dictionary<string, string>();
                headers.Add("user", ConfigurationManager.AppSettings["Quantum.Configuration.UserName"]);
                headers.Add("token", ConfigurationManager.AppSettings["Quantum.Configuration.Password"]);
                headers.Add("ContentType", "application/json");
                var CatalogResponse = await ApiService.PostAsync<QuantumCityResponse>
                (ConfigurationManager.AppSettings["Quantum.Proxy.Address"],
                    $"{ConfigurationManager.AppSettings["Quantum.City.Proxy.Action"]}", headers,
                    model);

                if (CatalogResponse == null || CatalogResponse.Result == null || CatalogResponse.Result.response is null || CatalogResponse.Result.response.message is null)
                {
                    response.Result = new List<CityQuantumResponse>();
                    return response;
                }
                response.Result = CatalogResponse.Result.response.message.ToList();
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("");
            }
        }

        public async Task<Response<List<SiteQuantumResponse>>> GetSite(GetMasterRequest model)
        {
            try
            {
                var response = new Response<List<SiteQuantumResponse>>();

                var headers = new Dictionary<string, string>();
                headers.Add("user", ConfigurationManager.AppSettings["Quantum.Configuration.UserName"]);
                headers.Add("token", ConfigurationManager.AppSettings["Quantum.Configuration.Password"]);
                headers.Add("ContentType", "application/json");
                var catalogResponse = await ApiService.PostAsync<QuantumSiteResponse>
                (ConfigurationManager.AppSettings["Quantum.Proxy.Address"],
                    $"{ConfigurationManager.AppSettings["Quantum.Site.Proxy.Action"]}", headers,
                    model);

                if (catalogResponse == null || catalogResponse.Result?.response?.message is null)
                {
                    response.Result = new List<SiteQuantumResponse>();
                    return response;
                }
                response.Result = catalogResponse.Result.response.message.ToList();
                response.IsSuccess = true;
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Quantum
    }
}