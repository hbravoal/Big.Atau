using Mastercard.Common.DTO.Request.MarketPlace;
using Mastercard.Common.DTO.Request.NetCommerce;
using Mastercard.Common.DTO.Response.MarketPlace;
using Mastercard.Common.DTO.Response.NetCommerce;
using Mastercard.Common.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mastercard.UI.Business.Interface.Providers
{
    public interface ICatalogFachade
    {
        Task<Response<List<CatalogNetCommerceResponse>>> GetNetCommerce(CatalogNetCommerceRequest model);

        Task<Response<List<ProductMarketPlaceResponse>>> GetMarketPlace(GetCatalogMarketplaceRequest model);
    }
}