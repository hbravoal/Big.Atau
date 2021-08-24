using Itau.Common.Diagnostics;
using Mastercard.UI.Business.Interface;

namespace Mastercard.UI.Business.BL
{
    internal class CatpchaValidate : ICatpcha
    {
        //Cuenta los loguins con el CardLogin del cliente.
        public bool Validate(string captchaResponse)
        {
            try
            {
                if (string.IsNullOrEmpty(captchaResponse))
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                throw new CatpchaException($"Error al consultar Capcha, error: {ex.InnerException}");
            }

            return false;
        }
    }
}