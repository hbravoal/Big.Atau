using Itau.Common.DTO.Response;
using Itau.Common.Wrapper;
using Mastercard.UI.Business.ViewModels;
using System.Threading.Tasks;

namespace Mastercard.UI.Business.Interface
{
    /// <summary>
    /// Modulo para obtener redenciones del cliente
    /// </summary>
    public interface IRedemptionUIBl
    {
        Task<Response<bool>> GetRedemption();

        /// <summary>
        /// Método para hacer redención
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response<RedemptionResponse>> Redemption(RedemptionViewModel model);
    }
}