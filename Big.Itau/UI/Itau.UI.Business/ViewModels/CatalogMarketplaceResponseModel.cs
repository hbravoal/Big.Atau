using System.Collections.Generic;

namespace Mastercard.UI.Business.ViewModels
{
    public class CatalogMarketplaceResponseModel
    {
        public string Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Conditions { get; set; }
        public string Image { get; set; }
        public string Instructions { get; set; }
        public int Inventory { get; set; }
        public int ProductTypeId { get; set; }
        public int ProgramId { get; set; }
        public int BrandId { get; set; }
        public bool Location { get; set; }
    }

    public class CatalogViewModelMarket
    {
        public bool CanRedeem { get; set; }
        public bool InventoryZero { get; set; }
        public bool HaveRedemptions { get; set; }
        public List<CatalogMarketplaceResponseModel> Products { get; set; }
    }
}