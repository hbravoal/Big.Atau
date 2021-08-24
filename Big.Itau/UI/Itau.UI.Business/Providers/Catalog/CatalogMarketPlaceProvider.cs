using Itau.Common.DTO.Request.MarketPlace;
using Itau.Common.DTO.Response.MarketPlace;
using Itau.Common.Helpers;
using Itau.Common.Wrapper;
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
    /// Proveedor para obtener catálogo a través de Marketplace API
    /// </summary>
    public class CatalogMarketPlaceProvider
        : ICatalogProvider<GetCatalogMarketplaceRequest, Response<List<ProductMarketPlaceResponse>>>

    {
        async public Task<Response<List<ProductMarketPlaceResponse>>> Get(GetCatalogMarketplaceRequest model)
        {
            var response = new Response<List<ProductMarketPlaceResponse>>();

            var auth = new AuthenticationHeaderValue("Bearer", SessionHelper.MarketPlaceToken);
            var headers = new Dictionary<string, string>();

            var CatalogResponse = await ApiService.PostAsync<List<ProductMarketPlaceResponse>>
                (ConfigurationManager.AppSettings["Marketplace.Proxy.Address"],
                $"{ConfigurationManager.AppSettings["Marketplace.Product.Proxy.Base"]}/{ConfigurationManager.AppSettings["Marketplace.Product.Proxy.GetByFilters"]}", headers,
                model, auth);

            if (!CatalogResponse.IsSuccess)
            {
                Itau.Common.Diagnostics.ExceptionLogging.LogException(new Exception
                    ($"Ocurrió un error al intentar Refrescar Catálogo para el IdMask:{SessionHelper.IdMask}| UserIdentity{SessionHelper.Token}| Token: {SessionHelper.Token} | Response.Result: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Result)}"));

                return response;
            }
            response.Result = CatalogResponse.Result;
            response.IsSuccess = true;
            return response;
        }
    }
}