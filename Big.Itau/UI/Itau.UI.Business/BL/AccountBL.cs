using Mastercard.Business.Services;
using Mastercard.Common.Diagnostics;
using Mastercard.Common.DTO.Response;
using Mastercard.Common.Helpers;
using Mastercard.Common.Utils;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using System;
using System.Configuration;
using System.Web.Security;

namespace Mastercard.UI.Business.BL
{
    public class AccountBL : IAccountBl
    {
        /// <summary>
        /// Método para loguar un cliente por complete
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Response Login(string username, string password)
        {
            var response = new Response();

            if (string.IsNullOrEmpty(username))
            {
                response.Message = $"Tu identificación es requerida.";
                return response;
            }

            if (string.IsNullOrEmpty(password))
            {
                response.Message = $"Password es requerido";
                return response;
            }
            CustomerService customer = new CustomerService(SessionHelper.Token);
            var loginIdentificationResponse = customer.LoginCustomer(username, password, Convert.ToInt32(ConfigurationManager.AppSettings["Program"]));

            if (!loginIdentificationResponse.IsSuccess)
            {
                switch (loginIdentificationResponse.ErrorCode)
                {
                    case Common.Enums.ErrorEnum.TokenExpired:
                        response.IsSuccess = false;
                        response.Message = "TokenInvalid";
                        break;
                    default:
                        response.IsSuccess = false;
                        response.Message = ConfigurationManager.AppSettings["UseYourCard.Messages.ClientNotExist"];
                        break;
                }
                return response;

            }

            LoginCustomerForIdentificationResponse responseLogin = loginIdentificationResponse.Result;
            if (responseLogin == null)
            {
                response.Message = UtilitiesCommon.GetKeyAppSettings("UseYourCard.Messages.ClientNotExist");
                return response;
            }

            //TODO: Buscar grupo.
            //SessionHelper.Token = responseLogin.Token.TokenX;
            SessionHelper.Identification = responseLogin.Customer.Identification;
            response.IsSuccess = true;
            //TODO: Buscar grupo.
            SessionHelper.LoginComplete = true;
            SessionHelper.Guid = Convert.ToString(responseLogin.Customer.Guid);
            SessionHelper.FirstLogin = false;

            
            response.IsSuccess = true;
            var s = SessionHelper.Guid;
            return response;
        }

        public void LogOut()
        {
            SessionHelper.LogOut();
        }
    }

    public class ClientFirstLogin : IAccountFirst
    {
        /// <summary>
        /// First login por un cliente.
        /// </summary>
        /// <param name="identification"></param>
        /// <param name="acceptTermsAndConditions"></param>
        /// <returns></returns>
        public Response LoginByIdentification(string identification, bool acceptTermsAndConditions, string ipClient)
        {
            Response response = new Response();

            if (string.IsNullOrEmpty(identification))
            {
                response.Message = UtilitiesCommon.GetKeyAppSettings("UseYourCard.Messages.IdentificationRequired");
                return response;
            }
            if (!acceptTermsAndConditions)
            {
                response.Message = UtilitiesCommon.GetKeyAppSettings("UseYourCard.Messages.TermsRequired"); ;
                return response;
            }
            string tokenExist = null;
            if (!string.IsNullOrEmpty(SessionHelper.Token))
                tokenExist = SessionHelper.Token;

            CustomerService customer = new CustomerService();
            var loginIdentificationResponse = customer.LoginCustomer(identification, Convert.ToInt32(ConfigurationManager.AppSettings["Program"]), ipClient, tokenExist);

            //si todo salió correcto
            if (loginIdentificationResponse.IsSuccess)
            {
                LoginCustomerForIdentificationResponse responseProfile = loginIdentificationResponse.Result;
                if (responseProfile == null)
                {
                    response.Message = UtilitiesCommon.GetKeyAppSettings("UseYourCard.Messages.ClientNotExist");
                    return response;
                }

                //TODO: Buscar grupo.
                SessionHelper.Guid = Convert.ToString(responseProfile.Customer.Guid);
                SessionHelper.Token = responseProfile.Token.TokenX;
                SessionHelper.Identification = identification;
                SessionHelper.SegmentId = responseProfile.Customer.ConfigurationId;
                SessionHelper.Name = responseProfile.Customer.Name;
                SessionHelper.ChellengeCheck = responseProfile.Customer.ChellengeCheck;
                response.IsSuccess = true;
                FormsAuthentication.SetAuthCookie(SessionHelper.Token, true);
                response.Result = responseProfile;
            }
            else
            {
                switch (loginIdentificationResponse.ErrorCode)
                {
                    case Common.Enums.ErrorEnum.UserNotFound:
                        response.Message = $"Esta campaña está dirigida únicamente a clientes Bancolombia con Tarjeta Débito y/o Crédito Mastercard. Si tienes inconvenientes con el ingreso comunícate con la línea de atención 01-800-0912345";
                        break;

                    default:
                        response.Message = $"Ha ocurrido un error, por favor intenta mas tarde.";
                        break;
                }
            }
            return response;
        }
    }

    public class UserLogins : IUserLogins
    {
        //Cuenta los loguins con el CardLogin del cliente.
        public Response<int> CountLogins(string customerGuid)
        {
            var response = new Response<int>();

            CustomerService customer = new CustomerService(SessionHelper.Token);
            if (!customer.TokenIsValid())
            {
                response.ErrorCode = Common.Enums.ErrorEnum.UnauthorizedRequest;
                return response;
            }

            var loginIdentificationResponse = customer.CountLoginForClient(customerGuid);

            //si todo salió correcto
            if (loginIdentificationResponse.IsSuccess)
            {
                int countLoginResult = loginIdentificationResponse.Result;

                response.IsSuccess = true;

                response.Result = countLoginResult;
            }
            else
            {
                response.Error = new MessageResult { Message = $"Ha ocurrido un error, por favor intenta mas tarde." };
            }
            return response;
        }
    }
}

public class CatpchaValidate : ICatpchaValidate
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

            var captchaValidate = ApiService.Get<ReCaptchaResponse>($"{UtilitiesCommon.GetKeyAppSettings("reCAPTCHA.ApiBase")}" +
            $"?secret={UtilitiesCommon.GetKeyAppSettings("reCAPTCHA.Key.Secret")}" +
            $"&response={captchaResponse}", string.Empty, string.Empty, string.Empty, false);

            if (captchaValidate == null || !captchaValidate.IsSuccess)
            {
                return false;
            }

            var castReCaptcha = (ReCaptchaResponse)captchaValidate.Result;

            if (!castReCaptcha.success)
            {
                return false;
            }

            return true;
        }
        catch (System.Exception ex)
        {
            throw new CatpchaException($"Error al consultar Capcha, error: {ex.InnerException}");
        }
    }
}