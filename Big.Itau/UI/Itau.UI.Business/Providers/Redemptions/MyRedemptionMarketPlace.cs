using Mastercard.Common.DTO.Request.MarketPlace;
using Mastercard.Common.Helpers;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Mastercard.Common.DTO.Response.MarketPlace;
using Mastercard.Common.DTO.Response;

namespace Mastercard.UI.Business.Providers.Redemptions
{
    /// <summary>
    /// Obtiene redenciones de Marketplace
    /// </summary>
    public class MyRedemptionMarketPlace
    {
        async public Task<Response<List<MyRedeptionOrderResponseDTO>>> MyRedemptions(string clientGuid)
        {
            Response<List<MyRedeptionOrderResponseDTO>> response = new Response<List<MyRedeptionOrderResponseDTO>>();

            var request = new MyRedemptionRequest()
            {
                CustomerGuid = clientGuid,
                ProgramId = Convert.ToInt32(ConfigurationManager.AppSettings["Marketplace.ProgramId"])
            };
            var auth = new AuthenticationHeaderValue("Bearer", SessionHelper.MarketPlaceToken);
            var headers = new Dictionary<string, string>() {
                { "Bearer", SessionHelper.MarketPlaceToken }
            };

            var MyRedemptions = await ApiService.PostAsync<List<MyRedeptionOrderResponseDTO>>
                 (ConfigurationManager.AppSettings["Marketplace.Proxy.Address"],
                 $"{ConfigurationManager.AppSettings["Marketplace.Order.Proxy.Base"]}/{ConfigurationManager.AppSettings["Marketplace.Order.Proxy.MyRedemptions"]}", headers,
                 request, auth);

            if (MyRedemptions == null || !MyRedemptions.IsSuccess)
            {
                response.Error = MyRedemptions.Error;
                return response;
            }
            response.Result = MyRedemptions.Result.FindAll(x=>x.OrderExtendedProperties.Count > 0);
            response.IsSuccess = true;
            return response;
        }
    }
}