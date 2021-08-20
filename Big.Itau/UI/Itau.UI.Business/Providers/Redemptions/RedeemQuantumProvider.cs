using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Mastercard.Common.DTO.Request.MarketPlace;
using Mastercard.Common.DTO.Response.MarketPlace;
using Mastercard.Common.Helpers;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface.Providers;

namespace Mastercard.UI.Business.Providers.Redemptions
{
    public class RedeemQuantumProvider
     : IRedeemProvider<CreateOrderMarketPlaceRequest, Response<CreateOrderMarketPlaceResponse>>
    {
        public async Task<Response<CreateOrderMarketPlaceResponse>> Execute(CreateOrderMarketPlaceRequest model)
        {
            var response = new Response<CreateOrderMarketPlaceResponse>();

            var auth = new AuthenticationHeaderValue("Bearer", model.MarketPlaceToken);
            var headers = new Dictionary<string, string>();

            var catalogResponse = await ApiService.PostAsync<CreateOrderMarketPlaceResponse>
            (ConfigurationManager.AppSettings["Marketplace.Proxy.Address"],
                $"{ConfigurationManager.AppSettings["Marketplace.Order.Proxy.Base"]}/{ConfigurationManager.AppSettings["Marketplace.Order.Proxy.Create"]}", headers,
                model, auth);

            if (!catalogResponse.IsSuccess)
            {
                Common.Diagnostics.ExceptionLogging.LogException(new Exception
                    ($"Ocurrió un error al intentar Refrescar Catálogo para el IdMask:{model.Data.ClientGuid}| Response.Result: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Result)}"));
                response.Error = new MessageResult
                {
                    Message = catalogResponse.Message
                };
                return response;
            }
            response.Result = catalogResponse.Result;
            response.IsSuccess = true;
            return response;
        }
    }
}
