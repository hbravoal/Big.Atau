using Mastercard.Common.DTO.Request.MarketPlace;
using Mastercard.Common.DTO.Request.NetCommerce;
using Mastercard.Common.DTO.Response.MarketPlace;
using Mastercard.Common.DTO.Response.NetCommerce;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.Interface.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mastercard.UI.Business.Providers.Catalog
{
    /// <summary>
    /// Fachada que contiene ambas formas de obtener catálogo
    /// </summary>

    public class CatalogFachade : ICatalogFachade
    {
        private readonly ICatalogProvider<GetCatalogMarketplaceRequest, Response<List<ProductMarketPlaceResponse>>> catalogMarketPlaceProvider;
        private readonly ICatalogProvider<CatalogNetCommerceRequest, Response<List<CatalogNetCommerceResponse>>> _catalogNetCommerceProvider;

        #region Constructor

        public CatalogFachade(ICatalogProvider<GetCatalogMarketplaceRequest, Response<List<ProductMarketPlaceResponse>>> catalogProvider,
           ICatalogProvider<CatalogNetCommerceRequest, Response<List<CatalogNetCommerceResponse>>> catalogNetCommerceProvider
           )
        {
            catalogMarketPlaceProvider = catalogProvider;
            _catalogNetCommerceProvider = catalogNetCommerceProvider;
        }

        #endregion Constructor

        /// <summary>
        /// Obtiene Catalogo de Marketplace
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<Response<List<ProductMarketPlaceResponse>>> GetMarketPlace(GetCatalogMarketplaceRequest model) => catalogMarketPlaceProvider.Get(model);

        /// <summary>
        /// Obtiene catalogo de NetCommerce
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public Task<Response<List<CatalogNetCommerceResponse>>> GetNetCommerce(CatalogNetCommerceRequest model) => _catalogNetCommerceProvider.Get(model);
    }
}