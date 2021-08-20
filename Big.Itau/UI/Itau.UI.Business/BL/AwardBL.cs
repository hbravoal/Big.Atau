using Mastercard.Business.Services;
using Mastercard.Common.DTO.Response;
using Mastercard.Common.Helpers;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Mastercard.UI.Business.BL
{
    public class AwardBL : IAwardBL
    {
        /// <summary>
        /// Obtiene
        /// </summary>
        /// <returns></returns>
        public Response GetAward()
        {
            var response = new Response();

            AwardsServices awardsServices = new AwardsServices(SessionHelper.Token);
            var awards = awardsServices.AwardSelections();

            if (!awards.IsSuccess)
            {
                switch (awards.ErrorCode)
                {

                    case Common.Enums.ErrorEnum.TokenExpired:
                        response.IsSuccess = false;
                        response.Message = "NoToken";
                        break;
                    default:
                        response.Message = $"Error al consultar los premios a seleccionar";
                        response.IsSuccess = awards.IsSuccess;
                        break;
                }
                return response;
            }
            Response<List<AwardDTO>> result = awards;
            response.Result = result.Result;
            response.Message = result.Message;
            response.IsSuccess = result.IsSuccess;
            return response;
        }

        public Response GetAwardById(short id)
        {
            var response = new Response();

            AwardsServices awardsServices = new AwardsServices(SessionHelper.Token);
            var awards = awardsServices.AwardSelectionsById(id);

            if (!awards.IsSuccess)
            {
                switch (awards.ErrorCode)
                {

                    case Common.Enums.ErrorEnum.TokenExpired:
                        response.IsSuccess = false;
                        response.Message = "NoToken";
                        break;
                    default:
                        response.Message = $"Error al consultar los premios a seleccionar";
                        response.IsSuccess = awards.IsSuccess;
                        break;
                }
                return response;
            }
            Response<List<AwardDTO>> result = awards;
            response.Result = result.Result;
            response.Message = result.Message;
            response.IsSuccess = result.IsSuccess;
            return response;
        }

        public Response SaveAwards(short awardID)
        {
            var response = new Response();

            if (string.IsNullOrEmpty(awardID.ToString()))
            {
                response.Message = $"Debe seleccionar un premio";
                response.IsSuccess = false;
                return response;
            }
            AwardsServices awardsServices = new AwardsServices(SessionHelper.Token);

            CustomerAwardDTO customerAward = new CustomerAwardDTO();
            customerAward.CustomerGuid = Guid.Parse(SessionHelper.Guid);
            customerAward.AwardId = awardID;
            customerAward.Programid = Convert.ToInt32(ConfigurationManager.AppSettings["Program"]);

            var awards = awardsServices.SaveAwardSelections(awardID);

            if (awards.IsSuccess)
            {
                response.Result = awards.Result;
                response.Message = awards.Message;
                response.IsSuccess = awards.IsSuccess;
                return response;
            }
            switch (awards.ErrorCode)
            {

                case Common.Enums.ErrorEnum.TokenExpired:
                    response.IsSuccess = false;
                    response.Message = "NoToken";
                    break;
                case Common.Enums.ErrorEnum.UserNotFound:
                    response.IsSuccess = false;
                    response.Message = "NoCustomer";
                    break;
                case Common.Enums.ErrorEnum.ChangeLock:
                    response.IsSuccess = false;
                    response.Message = "ChageLock";
                    break;
                case Common.Enums.ErrorEnum.DateLimitAward:
                    response.IsSuccess = false;
                    response.Message = "DateLimit";
                    break;
                default:
                    response.Message = $"Error al guardar el premio seleccionado";
                    response.IsSuccess = awards.IsSuccess;
                    Common.Diagnostics.ExceptionLogging
                      .LogInfo($"Ocurrio un error intentando traer awards IdMask:{SessionHelper.IdMask}| Token: {SessionHelper.Token} | Response: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Message)} | Response.Result: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Result)}");
                    break;
            }
            
            return response;
        }
    }
}