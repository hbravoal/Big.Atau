using Mastercard.Business.Services;
using Mastercard.Common.DTO.Request.NetCommerce;
using Mastercard.Common.DTO.Response.NetCommerce;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Mastercard.UI.Business.Providers.Catalog
{
    /// <summary>
    /// Proveedor para obtener catálogo a través de NetComemrce
    /// </summary>
    public class CatalogNetCommerceProvider : ICatalogProvider<CatalogNetCommerceRequest, Response<List<CatalogNetCommerceResponse>>>
    {
        async public Task<Response<List<CatalogNetCommerceResponse>>> Get(CatalogNetCommerceRequest model)
        {
            var response = new Response<List<CatalogNetCommerceResponse>>();
            var headers = new Dictionary<string, string>();

            var authenticationString = $"{ConfigurationManager.AppSettings["BasicAuthentication.UserName"]}:{ConfigurationManager.AppSettings["BasicAuthentication.Password"]}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            var auth = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            headers.Add("Token", SessionHelper.Token);

            CatalogNetCommerceService catalogNetCommerceServices = new CatalogNetCommerceService(SessionHelper.Token);

            var CatalogResponse = catalogNetCommerceServices.GetCodeCatalogProductByFilterAllProducts(model);

            if (!CatalogResponse.IsSuccess)
            {
                Common.Diagnostics.ExceptionLogging.LogException(new Exception
                    ($"Ocurrió un error al intentar Refrescar Catálogo para el IdMask:{SessionHelper.IdMask}| UserIdentity{SessionHelper.Token}| Token: {SessionHelper.Token} | Response.Result: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Result)}"));
                response.Error = new MessageResult
                {
                    Message = CatalogResponse.Message
                };
                return response;
            }
            if (!CatalogResponse.IsSuccess)
            {
                Common.Diagnostics.ExceptionLogging.LogException(new Exception($"Ha ocurrido un eror al intentar obtener Catalogo: {SessionHelper.Token}, API Result: {CatalogResponse.Result}"));
                response.IsSuccess = false;
                return response;
            }
            if (CatalogResponse.ErrorCode == Common.Enums.ErrorEnum.TokenExpired)
            {
                response.IsSuccess = false;
                response.Message = "NoToken";
                return response;
            }
            var catalogResult = CatalogResponse.Result;
            response.IsSuccess = true;
            response.Result = catalogResult;
            return response;
        }
    }
}