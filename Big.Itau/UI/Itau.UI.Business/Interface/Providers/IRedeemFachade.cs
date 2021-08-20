using Mastercard.Common.DTO.Request.MarketPlace;
using Mastercard.Common.DTO.Response.MarketPlace;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mastercard.Common.DTO.Response.NetCommerce;

namespace Mastercard.UI.Business.Interface.Providers
{
    public interface IRedeemFachade
    {
        /// <summary>
        /// Redención con NetCommerce
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<RedemptionNetCommerceResponse>> RedeemNetCommerce(RedemptionViewModel model);

        Task<Response<CreateOrderMarketPlaceResponse>> RedeemMarketPlace(CreateOrderMarketPlaceRequest model);
    }
}