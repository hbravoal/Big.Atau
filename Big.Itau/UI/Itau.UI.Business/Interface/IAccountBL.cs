using Itau.Common.Wrapper;
using Mastercard.Common.DTO.Response;

namespace Mastercard.UI.Business.Interface
{
    /// <summary>
    /// Interface para loguear al cliente por Identificación
    /// </summary>
    public interface IAccountBL
    {
        Response<LoginResponse> LoginByIdentification(string identification, bool acceptTermsAndConditions, string ipClient);
    }
}