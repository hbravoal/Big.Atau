using Mastercard.Common.DTO;
using Mastercard.Common.Helpers;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using System;

namespace Mastercard.UI.Business.BL
{
    public class SegmentBL : IUserState
    {
        public Response GetGoal()
        {
            var response = new Response();
            var token = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(SessionHelper.Token))
                {
                    var ps = token;
                    return null;
                }

                var customerProviderService = new Mastercard.Business.Services.CustomerService();
                var result = customerProviderService.GetCustomerByToken(SessionHelper.Token);
                if (result is null)
                {
                    response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    return response;
                }

                var s = customerProviderService.GetSegment(result); ;
                var Segment = (SegmentDTO)customerProviderService.GetSegment(result);
                if (Segment is null)
                {
                    //TODO: error obteniendo meta
                    return null;
                }

                SessionHelper.BillingGroupGoal = Segment.BillingGroupGoal;
                SessionHelper.CanRedeem = Segment.CanRedeem;
                SessionHelper.RedemptionLimitReached = Segment.RedemptionLimitReached;
                response.Result = Segment;
                response.IsSuccess = true;
                response.Message = "Segment Exist";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }
    }
}