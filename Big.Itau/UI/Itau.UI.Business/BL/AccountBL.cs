using Itau.Common.Wrapper;
using Mastercard.Common.DTO.Response;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;

namespace Mastercard.UI.Business.BL
{
    public class AccountBL : IAccountBL
    {
        public void LogOut()
        {
            SessionHelper.LogOut();
        }

        public Response<LoginResponse> LoginByIdentification(string identification, bool acceptTermsAndConditions, string ipClient)
        {
            throw new System.NotImplementedException();
        }
    }
}