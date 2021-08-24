using Itau.Common.Enums;
using Itau.Common.Wrapper;
using Mastercard.UI.Business.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mastercard.UI.Business.Interface
{
    public interface ICatalogBL
    {
        Task<Response<List<CatalogViewModel>>> GetCatalogProducts();

        Task<Response<CatalogViewModel>> GetProduct(string productGuid, EnumCatalogNetCommerceType type);
    }
}