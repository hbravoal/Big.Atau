using Mastercard.Business.Services;
using Mastercard.Common.DTO;
using Mastercard.Common.DTO.Request;
using Mastercard.Common.DTO.Response;
using Mastercard.Common.Helpers;
using Mastercard.Common.Utils;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using System;
using System.Configuration;

namespace Mastercard.UI.Business.BL
{
    public class MisionBL : IMisionBL
    {
        public Response GetMisionCustomer()
        {
            var response = new Response();
            MisionServices misionServices = new MisionServices(SessionHelper.Token);

            var Misions = misionServices.GetMisions();
            if (Misions.IsSuccess)
            {
                MisionPointDTO ListMisionsCustomer = (MisionPointDTO)Misions.Result;
                if (ListMisionsCustomer.Misions.Count > 0)
                {
                    response.Result = ListMisionsCustomer;
                    response.IsSuccess = true;
                    return response;
                }
            }
            switch (Misions.ErrorCode)
            {
               
                case Common.Enums.ErrorEnum.TokenExpired:
                    response.IsSuccess = false;
                    response.Message = "NoToken";
                    break;
                default:
                    response.IsSuccess = false;
                    response.Message = $"Ocurrio un error intentando traer misiones";
                    Common.Diagnostics.ExceptionLogging
                      .LogInfo($"Ocurrio un error intentando traer misiones IdMask:{SessionHelper.IdMask}| Token: {SessionHelper.Token} | Response: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Message)} | Response.Result: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Result)}");
                    break;
            }
            return response;
        }

        public Response GetMisionById(int Id)
        {
            Response response = new Response();

            MisionServices misionServices = new MisionServices(SessionHelper.Token);
            var Misions = misionServices.GetMisionsCustomerPointById(Id);
            if (Misions.IsSuccess)
            {
                MisionPointDTO ListMisionsCustomer = (MisionPointDTO)Misions.Result;
                if (ListMisionsCustomer.Misions.Count > 0)
                {
                    response.Result = ListMisionsCustomer;
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = $"No exinten misiones en la maestra";
                    Common.Diagnostics.ExceptionLogging
                      .LogInfo($"No exinten misiones en la maestra:{SessionHelper.IdMask}| Token: {SessionHelper.Token} | Response: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Message)} | Response.Result: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Result)}");
                    return response;
                }
            }
            response.IsSuccess = Misions.IsSuccess;
            response.Message = $"Ocurrio un error intentando traer misiones";
            Common.Diagnostics.ExceptionLogging
                     .LogInfo($"Ocurrio un error intentando traer misiones IdMask:{SessionHelper.IdMask}| Token: {SessionHelper.Token} | Response: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Message)} | Response.Result: {Newtonsoft.Json.JsonConvert.SerializeObject(response.Result)}");
            return response;
        }

        public Response RedemptionPoint()
        {
            var response = new Response();
            RedemptionPointServices RedemptionPointServices = new RedemptionPointServices(SessionHelper.Token);
            
            if (SessionHelper.CanRedeem) {
                var redemptionPoint = RedemptionPointServices.RedemptionPoint(Convert.ToInt32(UtilitiesCommon.GetKeyAppSettings("MisionRedemtion")));

                RedemptionPointInformation redemptionPointInfo = redemptionPoint.Result;
                if (redemptionPoint.IsSuccess)
                {
                    response.Result = redemptionPointInfo;
                    response.IsSuccess = redemptionPoint.IsSuccess;
                    response.Message = redemptionPoint.Message;
                    return response;
                } 
                response.IsSuccess = false;
                response.Message = "Ocurrio un error intentando redimir cupones";
                return response;
            }
            response.IsSuccess = false;
            response.Message = "NoCanRedeem";

            return response;
        }
    }
}