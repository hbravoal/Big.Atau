using Mastercard.Business.Services;
using Mastercard.Common.DTO;
using Mastercard.Common.DTO.Request.MarketPlace;
using Mastercard.Common.DTO.Response;
using Mastercard.Common.DTO.Response.MarketPlace;
using Mastercard.Common.Enums;
using Mastercard.Common.Helpers;
using Mastercard.Common.Utils;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using Mastercard.UI.Business.Interface.Providers;
using Mastercard.UI.Business.Providers.Redemptions;
using Mastercard.UI.Business.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mastercard.Common.DTO.Response.NetCommerce;
using System.Configuration;

namespace Mastercard.UI.Business.BL
{
    /// <summary>
    /// Lógica UI para módulo de redención
    /// </summary>
    public class RedemptionUIBl : IRedemptionUIBl
    {
        private readonly IRedeemFachade _redeemFachade;

        #region Properties

        private readonly MyRedemptionMarketPlace _getRedemption = new MyRedemptionMarketPlace();
        private readonly MyRedemptionNetCommerceProviders _myRedemption = new MyRedemptionNetCommerceProviders();
        
        #endregion Properties

        public RedemptionUIBl(IRedeemFachade redeemFachade)
        {
            _redeemFachade = redeemFachade;
        }

        #region Implements

        public async Task<Response<RedemptionResponse>> Redemption(RedemptionViewModel request)
        {
            var response = new Response<RedemptionResponse>();
            response.Result = new RedemptionResponse();
            response.Result.Title = "¡Ya redimiste tu bono del catálogo!";
            try
            {
                var type = request.CatalogType;
                if (type == EnumCatalogNetCommerceType.External)
                {
                    var marketPlaceRequest = new CreateOrderMarketPlaceRequest()
                    {
                        Provider = "Quantum",
                        MarketPlaceToken = request.MarketPlaceToken,
                        Data = new CreateOrderMarketPlaceDataRequest()
                        {
                            ClientGuid = request.UserGuid,
                            ProgramId = Convert.ToInt32(
                                   UtilitiesCommon.GetKeyAppSettings("Marketplace.ProgramId")),
                            Source = UtilitiesCommon.GetKeyAppSettings("ProgramName"),
                            ProductId = UtilitiesCommon.Cryptrography.Decrypt(request.Id),
                            Segment = Convert.ToString(request.SegmentId),
                            OrderExtendedProperties = new List<OrderExtendedMarketPlaceProperty>()
                        }
                    };

                    if (request.Location)
                    {
                        marketPlaceRequest.Data.SiteId = Convert.ToString(request.SiteId);

                        ///REQUIERE  SITEID
                    }

                    if (request.AditionalInformation)
                    {
                        marketPlaceRequest.Data.Name = request.Name + request.LastName;
                        marketPlaceRequest.Data.CustomerIdentification = request.Identification;
                        //REQUIERE NAME y Identification
                    }

                    
                    var resultMarketPlace = await _redeemFachade.RedeemMarketPlace(marketPlaceRequest);
                    if (!resultMarketPlace.IsSuccess)
                    {
                        response.Message = "Error al intentar redimir, intentar mas tarde.";
                        return response;
                        //TODO: ERror.
                    }

                    if (resultMarketPlace == null || !resultMarketPlace.IsSuccess)
                    {
                        string message =
                            $"Error Redimiendo: Cliente: Request: {JsonConvert.SerializeObject(request)}";
                        response.Message = "Error al intentar redimir, intentar mas tarde.";
                        return response;
                    }

                    if (resultMarketPlace.Result == null || resultMarketPlace.Result.Data.OrderExtendedProperties == null ||
                        resultMarketPlace.Result.Data.OrderExtendedProperties.Count == 0)
                    {
                        response.Message = "Error al intentar redimir, intentar mas tarde.";
                        return response;
                    }

                    //Sacamos linkPdf
                    var link = resultMarketPlace.Result.Data.OrderExtendedProperties.FirstOrDefault(c => c.Key == OrderExtentedPropertyKeyEnum.LinkPdf);

                    if (link == null || string.IsNullOrEmpty(link.Value))
                    {
                        //TODO
                        response.Message = "Error al intentar redimir, intentar mas tarde.";
                        return response;
                    }

                    CustomerService customer = new CustomerService(request.UserToken);
                    var result =customer.ResetGoal(customer.GetCustomer(), RedemptionTypeEnum.MarketPlaceRedemption, "");
                    if (result == null || !result.IsSuccess)
                    {
                        Common.Diagnostics.ExceptionLogging.LogException(new Exception($"Error al intentar resetear la redención del cliente {request.UserToken}"));
                        return response;
                    }


                    response.Result = new RedemptionResponse();
                    response.Result.LinkPdf = link.Value;
                    response.IsSuccess = true;
                    response.Result.Title = $"Redención exitosa";
                    
                }
                else
                {
                    
                    var resultRedeemNetCommerce = await _redeemFachade.RedeemNetCommerce(request);
                    if (resultRedeemNetCommerce == null || !resultRedeemNetCommerce.IsSuccess)
                    {
                        response.Message = resultRedeemNetCommerce.Message;
                        return response;
                    }

                    if (type == EnumCatalogNetCommerceType.PCO)
                    {
                        response.IsSuccess = true;
                        response.Result.Message = $"En {UtilitiesCommon.GetKeyAppSettings("PasalaGanando.PCO.Days")} días hábiles verás reflejados los puntos redimidos en la página de Puntos Colombia.";

                    }
                    else
                    {
                        //Sería digital
                        response.Result.Message = $"Disfrútalo con este código:";
                        response.Result.Title = $"¡Ya redimiste tu bono del catálogo!";
                        response.Result.LinkPdf = resultRedeemNetCommerce.Result ==null ?  "": resultRedeemNetCommerce.Result.Detail.LinkPdfRedemption;
                        response.Result.ExpirationDate = resultRedeemNetCommerce.Result == null ? "" : resultRedeemNetCommerce.Result.Detail.Expiration.ToString();
                        response.Result.Code = resultRedeemNetCommerce.Result == null ? "" : resultRedeemNetCommerce.Result.Detail.ProductCode;
                    }
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                response.ErrorCode = ErrorEnum.InternatlError;
                response.Message = $" Ocurrió un error al intentar redimir";
                Common.Diagnostics.ExceptionLogging.LogException(new Exception($"Ocurrió un error al intentar redimir", ex));
            }

            return response;
        }

        /// <summary>
        /// Obtiene redención de un usuario. Quantum y NetCommerce.
        /// </summary>
        /// TODO: No capturar errores.
        /// <returns></returns>
        public async Task<Response> GetRedemption()
        {
            
            Response response = new Response();
            MyRedemptionsViewModel MyRedemptionModel = new MyRedemptionsViewModel();
           
            var NetCommerceRedemptions = await _myRedemption.GetRedemption();
            //TODO: Validar-.
            if (NetCommerceRedemptions.Message == "TokenExpired")
            {
                response.Message = "TokenExpired";
                response.IsSuccess = false;
                return response;
            }
            var netcomer = (List<RedemptionResponse>)NetCommerceRedemptions.Result;

            var MarketPlaceRedemptions = await _getRedemption.MyRedemptions(SessionHelper.Guid.ToString());
            var marketplace = MarketPlaceRedemptions.Result;

            if (netcomer.Count>0)
            {
                MyRedemptionModel =  ConvertToViewModel(netcomer);
            }
            if (marketplace != null && marketplace.Count > 0 )
            {
                MyRedemptionModel = ConvertToViewModel(marketplace);
            }
            if ((netcomer ==null || netcomer.Count == 0) && (marketplace == null || marketplace.Count == 0))
            {
                MyRedemptionModel.MisionView = GetMision();
            }
            response.Result = MyRedemptionModel;
            response.IsSuccess = true;
            return response;
        }

        #endregion Implements

        #region Casting

        public MyRedemptionsViewModel ConvertToViewModel(List<RedemptionResponse> response)
        {
            
            var myRedemotion = new MyRedemptionsViewModel();
            if (response.Count > 0)
            {
                if (response.FirstOrDefault().Type == Convert.ToInt32(ConfigurationManager.AppSettings["Category"]))
                {
                    myRedemotion.productName = response.FirstOrDefault().Title;
                    myRedemotion.imageUrl = response.FirstOrDefault().ImageUrl;
                }
                else
                {
                    myRedemotion.productName = response.FirstOrDefault().Title;
                    myRedemotion.imageUrl = response.FirstOrDefault().ImageUrl;
                    myRedemotion.linkPdfRedemption = response.FirstOrDefault().LinkPdf;
                    myRedemotion.Date = Convert.ToDateTime(response.FirstOrDefault().ExpirationDate);
                    myRedemotion.productCode = response.FirstOrDefault().Code;
                    myRedemotion.type = response.FirstOrDefault().Type;
                    
                }
                myRedemotion.MisionView = GetMision();
            }
            return myRedemotion;
        }

        public MyRedemptionsViewModel ConvertToViewModel(List<MyRedeptionOrderResponseDTO> response)
        {
            var redemptionsViewModels = new MyRedemptionsViewModel();

            if (response.FirstOrDefault().OrderExtendedProperties.Count > 0)
            {
                var link = response.FirstOrDefault(x=>x.orderDetails.Count>0).OrderExtendedProperties.FirstOrDefault(c => c.Key == OrderExtentedPropertyKeyEnum.LinkPdf);

                redemptionsViewModels.imageUrl = response.FirstOrDefault(x => x.orderDetails.Count > 0).orderDetails.FirstOrDefault().Product.Image;
                    redemptionsViewModels.linkPdfRedemption = link.Value.ToString();
                redemptionsViewModels.productName = response.FirstOrDefault(x => x.orderDetails.Count > 0).orderDetails.FirstOrDefault().Product.Name;
                redemptionsViewModels.MisionView = GetMision();

               
            }
                return redemptionsViewModels;
        }
        public MisionPointDTO GetMision()
        {
            MisionServices misionServices = new MisionServices(SessionHelper.Token);
            var Misiones = misionServices.GetMisions();
            var mionPoint = Misiones.Result;
            return mionPoint;
        }
        

        #endregion Casting
    }
}