namespace Itau.Common.DTO.Request.NetCommerce
{
    /// <summary>
    /// Clas for Catalog Request  (NetCommerce).
    /// </summary>
    public class CatalogNetCommerceRequest
    {
        public string ProductName { get; set; }
        public string ProductGuid { get; set; }
        public int PageSize { get; set; }
        public int CategoryId { get; set; }
        public int PageIndex { get; set; }

        public bool ShowProductInvertoryCero { get; set; }
    }
}