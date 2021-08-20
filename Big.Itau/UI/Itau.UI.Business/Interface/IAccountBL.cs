using Mastercard.Common.Helpers;
using Mastercard.Common.Wrapper;
using System;

namespace Mastercard.UI.Business.Interface
{
    //Interface para loguear al cliente por Identificación
    public interface IAccountFirst
    {
        Response LoginByIdentification(string identification, bool acceptTermsAndConditions,string ipClient);

    }

    public interface ICatpchaValidate
    {
        bool Validate(string captchaResponse);
    }

    public interface IAccountBl
    {
        /// <summary>
        /// Método para loguear al usuario.
        /// </summary>
        /// <param name="username">dbo.MC_CLIENT_CARD.Username</param>
        /// <param name="code">dbo.MC_CLIENT_CARD.Code</param>
        /// <returns></returns>
        Response Login(string username, string code);

        void LogOut();
    }

    public interface IUserLogins
    {
        /// <summary>
        /// Cuenta  los ingresos a la plataforma a través del MC_CLIENT_LOGIN.CardLogin del cliente.
        /// </summary>
        /// <param name="cardNumberId"></param>
        /// <returns></returns>
        Response<int> CountLogins(string customerGuid);
    }

    
}