using Mastercard.Common.Helpers;
using System.Threading.Tasks;
using Mastercard.Common.DTO.Response;
using Mastercard.Common.DTO.Response.NetCommerce;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.ViewModels;

namespace Mastercard.UI.Business.Interface
{
    /// <summary>
    /// Modulo para obtener redenciones del cliente
    /// </summary>
    public interface IRedemptionUIBl
    {
        Task<Response> GetRedemption();

        /// <summary>
        /// Método para hacer redención
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<RedemptionResponse>> Redemption(RedemptionViewModel model);

    }
}