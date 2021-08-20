using Mastercard.UI.Business.Providers.Redemptions;
using Mastercard.UI.Business.ViewModels;

[assembly: WebActivator.PostApplicationStartMethod(typeof(Mastercard.UI.Interface.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace Mastercard.UI.Interface.App_Start
{
    using Mastercard.Common.DTO.Request.MarketPlace;
    using Mastercard.Common.DTO.Request.NetCommerce;
    using Mastercard.Common.DTO.Response.MarketPlace;
    using Mastercard.Common.DTO.Response.NetCommerce;
    using Mastercard.Common.Wrapper;
    using Mastercard.UI.Business.BL;
    using Mastercard.UI.Business.Interface;
    using Mastercard.UI.Business.Interface.Providers;
    using Mastercard.UI.Business.Providers;
    using Mastercard.UI.Business.Providers.Catalog;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;

    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void InitializeContainer(Container container)
        {
            #region AccountBL

            container.Register<IUserLogins, UserLogins>(Lifestyle.Scoped);
            container.Register<IAccountBl, AccountBL>(Lifestyle.Scoped);
            container.Register<IAccountFirst, ClientFirstLogin>(Lifestyle.Scoped);
            container.Register<ICatpchaValidate, CatpchaValidate>(Lifestyle.Scoped);

            #endregion AccountBL

            #region Marketplace

            container.Register<IAuthentication, MarketPlaceAuthentication>(Lifestyle.Scoped);

            #endregion Marketplace

            #region CustomerState

            container.Register<IUserState, SegmentBL>(Lifestyle.Singleton);
            container.Register<IMisionBL, MisionBL>(Lifestyle.Singleton);

            #endregion CustomerState

            #region Awards

            container.Register<IAwardBL, AwardBL>(Lifestyle.Singleton);

            #endregion Awards

            #region Catalog

            container.Register<ICatalogFachade, CatalogFachade>(Lifestyle.Singleton);
            container.Register<ICatalogBL, CatalogBL>(Lifestyle.Singleton);
            container.Register<ICatalogProvider<GetCatalogMarketplaceRequest, Response<List<ProductMarketPlaceResponse>>>, CatalogMarketPlaceProvider>(Lifestyle.Singleton);
            container.Register<ICatalogProvider<CatalogNetCommerceRequest, Response<List<CatalogNetCommerceResponse>>>, CatalogNetCommerceProvider>(Lifestyle.Singleton);

            #endregion Catalog


            #region Redeem

            container.Register<IRedeemFachade, RedeemFachade>(Lifestyle.Singleton);
            container.Register<IRedemptionUIBl, RedemptionUIBl>(Lifestyle.Singleton);

            container.Register<IRedeemProvider<CreateOrderMarketPlaceRequest, Response<CreateOrderMarketPlaceResponse>>, RedeemQuantumProvider>(Lifestyle.Singleton);
            container.Register<IRedeemProvider<RedemptionViewModel, Response<RedemptionNetCommerceResponse>>, RedeemNetCommerceProvider>(Lifestyle.Singleton);

            #endregion Redeem
            #region Master

            container.Register<IMasterQuantumBl, MasterBl>(Lifestyle.Singleton);
            container.Register<IMasterBL, MasterBl>(Lifestyle.Singleton);

            #endregion Master

            #region Redemption



            #endregion Redemption
        }
    }
}