using Mastercard.Common.DTO.Request.MarketPlace;
using Mastercard.Common.DTO.Request.NetCommerce;
using Mastercard.Common.DTO.Response.MarketPlace;
using Mastercard.Common.Enums;
using Mastercard.Common.Helpers;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using Mastercard.UI.Business.Interface.Providers;
using Mastercard.UI.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static Mastercard.Common.Utils.UtilitiesCommon;

namespace Mastercard.UI.Business.BL
{
    public class CatalogBL : ICatalogBL
    {
        #region Properties

        private readonly ICatalogFachade _customerCatalog;

        #endregion Properties

        #region Constructor

        public CatalogBL(ICatalogFachade customerCatalog)
        {
            _customerCatalog = customerCatalog;
        }

        #endregion Constructor

        async public Task<Response<CatalogViewModel>> GetProduct(string productGuid, EnumCatalogNetCommerceType type)
        {
            Response<CatalogViewModel> response = new Response<CatalogViewModel>();
            try
            {
                //Quantum
                if (type == EnumCatalogNetCommerceType.External)
                {
                    var marketPlaceRequest = new GetCatalogMarketplaceRequest()
                    {
                        Data = new DataProductRequest()
                        {
                            //TODO: Quitar el machete.
                            Segment = Convert.ToString(SessionHelper.SegmentId),
                            ProgramId = Convert.ToInt32(ConfigurationManager.AppSettings["Marketplace.ProgramId"]),
                            PageNumber = Convert.ToInt32(ConfigurationManager.AppSettings["Marketplace.Configuration.PageNumber"]),
                            PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["Marketplace.Configuration.PageSize"]),
                            Id = Convert.ToInt32(productGuid)
                        }
                    };
                    var result = await _customerCatalog.GetMarketPlace(marketPlaceRequest);
                    if (result == null || !result.IsSuccess || result.Result == null || result.Result.Count == 0)
                    {
                        return null;
                    }

                    var cast = ConvertToViewModel(result.Result);
                    if (cast == null)
                        return null;

                    response.Result = ConvertToViewModel(result.Result).FirstOrDefault();

                    response.IsSuccess = true;
                }
                else
                {
                    //NetCommerce.

                    CatalogNetCommerceRequest modelNetCommerceRequest = new CatalogNetCommerceRequest
                    {
                        PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PasalaGanando.NetCommerce.Catalog.PageSize"]),
                        PageIndex = Convert.ToInt32(1),
                        ProductGuid = (productGuid)
                    };
                    var catalogNetResult = await _customerCatalog.GetNetCommerce(modelNetCommerceRequest);
                    if (catalogNetResult == null || !catalogNetResult.IsSuccess || catalogNetResult.Result == null || catalogNetResult.Result.Count == 0)
                    {
                        return null;
                    }
                    var catalogNetCommerce = catalogNetResult.Result;
                    response.Result = ConvertToViewModel(catalogNetCommerce).FirstOrDefault();
                    response.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return response;
        }

        async public Task<Response<List<CatalogViewModel>>> GetCatalogProducts()
        {
            var response = new Response<List<CatalogViewModel>>();
            var marketPlaceRequest = new GetCatalogMarketplaceRequest()
            {
                Data = new DataProductRequest()
                {
                    //TODO: Quitar el machete.
                    Segment = Convert.ToString(SessionHelper.SegmentId),
                    ProgramId = Convert.ToInt32(ConfigurationManager.AppSettings["Marketplace.ProgramId"]),
                    PageNumber = Convert.ToInt32(ConfigurationManager.AppSettings["Marketplace.Configuration.PageNumber"]),
                    PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["Marketplace.Configuration.PageSize"]),
                }
            };

            

            //Tiramos Quantum en otro Hilo y continuamos con NetCommerce.
            //var catalogMarketPlace = _customerCatalog.GetMarketPlace(marketPlaceRequest);
            var catalogMarketPlaceResultTask = _customerCatalog.GetMarketPlace(marketPlaceRequest);


            CatalogNetCommerceRequest modelNetCommerceRequest = new CatalogNetCommerceRequest
            {
                PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PasalaGanando.NetCommerce.Catalog.PageSize"]),
                PageIndex = Convert.ToInt32(1),
            };

            List<CatalogViewModel> cataloViewModel = new List<CatalogViewModel>();


            var catalogNetResultTask =  _customerCatalog.GetNetCommerce(modelNetCommerceRequest);

            await catalogMarketPlaceResultTask;

            var catalogMarketPlaceResult = catalogMarketPlaceResultTask.Result;
            await catalogNetResultTask;

            var catalogNetResult = catalogNetResultTask.Result;
            if (catalogNetResult == null || !catalogNetResult.IsSuccess)
            {
                //TODO: Nose pudo obtener NetCommerce
            }
            //sacamos el Catalogo de ambos.
            var catalogNetCommerce = catalogNetResult.Result;
            var catalogMarketPlace = catalogMarketPlaceResult.Result;

            if (catalogNetCommerce != null)
            {
                cataloViewModel.AddRange(ConvertToViewModel(catalogNetCommerce));
            }

            if (catalogMarketPlace != null)
            {
                cataloViewModel.AddRange(ConvertToViewModel(catalogMarketPlace));
            }

            //catalogMarketPlace.Wait();
            //var catalogMarketPlaceResult = catalogMarketPlace.Result;
            if (catalogMarketPlaceResult == null || catalogMarketPlaceResult.IsSuccess)
            {
                //TODO: No se pudo obtener
            }

            response.Result = cataloViewModel;
            response.IsSuccess = true;
            return response;
        }

        public List<CatalogViewModel> ConvertToViewModel(List<Common.DTO.Response.NetCommerce.CatalogNetCommerceResponse> response)
        {
            List<CatalogViewModel> cataloViewModel = new List<CatalogViewModel>();
            if (response != null)
            {
                foreach (var item in response)
                {
                    cataloViewModel.Add(new CatalogViewModel
                    {
                        Id = Cryptrography.Encrypt(item.ProductGuid),
                        Active = true,
                        TermsAndConditions = item.Terms,
                        Image = item.ProductImage.Replace("~", ConfigurationManager.AppSettings["PasalaGanando.NetCommerce.Image.Url"]),
                        CatalogType = Cryptrography.Encrypt(Convert.ToString(item.CategoryId)),
                        Instructions = item.Instructions,
                        Inventory = item.Inventory,
                        ProductName = item.ProductName,
                        LongDescription = item.LongDescription,
                        ShortDescription = item.ShortDescription,
                        ProductCode = item.ProductCode,
                        ProgramId = Convert.ToString(item.ProgramId),
                        RealSegment = item.ReferenceId
                    });
                }
            }
            return cataloViewModel;
        }

        public List<CatalogViewModel> ConvertToViewModel(List<Common.DTO.Response.MarketPlace.ProductMarketPlaceResponse> response)
        {
            List<CatalogViewModel> cataloViewModel = new List<CatalogViewModel>();
            if (response != null)
            {
                foreach (var item in response)
                {
                    cataloViewModel.Add(new CatalogViewModel
                    {
                        Id = Cryptrography.Encrypt(Convert.ToString(item.Id)),
                        Active = true,
                        TermsAndConditions = item.Conditions,
                        Image = item.Image,
                        CatalogType = Cryptrography.Encrypt(Convert.ToString((int)EnumCatalogNetCommerceType.External)),
                        Instructions = item.Instructions,
                        Inventory = item.ProductReferences.FirstOrDefault().Inventory,
                        ProductName = item.Name,
                        LongDescription = item.LongDescription,
                        ShortDescription = item.ShortDescription,
                        ProductCode = item.ProductCode,
                        ProgramId = Convert.ToString(item.ProgramId),
                        RealSegment = item.Segment,
                        BrandId = item.Brand.BrandCode,
                        Location = item.Brand.Flag,
                        Brand= item.Brand.Description,
                        AditionalInformation = item.Brand.additionalData


                    });
                }
            }
            return cataloViewModel;
        }

     
    }
}