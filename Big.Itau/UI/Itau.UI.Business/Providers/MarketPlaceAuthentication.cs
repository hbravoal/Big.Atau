using Itau.Common.DTO.Request.MarketPlace;
using Itau.Common.DTO.Response.MarketPlace;
using Itau.Common.Helpers;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using System.Configuration;
using System.Threading.Tasks;

namespace Mastercard.UI.Business.Providers
{
    /// <summary>
    /// Autenticación en Marketplace.
    /// </summary>
    public class MarketPlaceAuthentication : IAuthentication
    {
        public async Task Login()
        {
            var request = new AuthMarketPlaceRequest();
            request.email = ConfigurationManager.AppSettings["Marketplace.Authentication.Email"];
            request.password = ConfigurationManager.AppSettings["Marketplace.Authentication.Password"];
            var result = await ApiService.PostAsync<AuthMarketPlaceResponse>
                (ConfigurationManager.AppSettings["Marketplace.Proxy.Address"],
                $"{ConfigurationManager.AppSettings["Marketplace.Account.Proxy.Base"]}/{ConfigurationManager.AppSettings["Marketplace.Account.Proxy.Authenticate"]}",
                null,
                request);
            if (result == null || !result.IsSuccess || result.Result == null || result.Result.Data == null)
            {
                //TODO: Error
            }
            SessionHelper.MarketPlaceToken = result.Result.Data.JWToken;
        }
    }
}