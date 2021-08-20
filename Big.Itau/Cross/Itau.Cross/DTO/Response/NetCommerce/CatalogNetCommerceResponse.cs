using Itau.Common.Enums;

namespace Itau.Common.DTO.Response.NetCommerce
{
    public class CatalogNetCommerceResponse
    {
        public string ProductGuid { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string ProductImage { get; set; }
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public int Inventory { get; set; }
        public int SegmentId { get; set; }
        public string ReferenceId { get; set; }
        public string Instructions { get; set; }
        public string Terms { get; set; }
        public int PageSize { get; set; }

        public int PageIndex { get; set; }
        public int ConfigutarionId { get; set; }
        public int TotalPages { get; set; }
        public int ProgramId { get; set; }

        public bool IsDigitalProduct { get; set; }

        public bool ShowProductInvertoryCero { get; set; }
        public int? GroupId { get; set; }
        public EnumCatalogNetCommerceType CatalogType { get; set; }
    }
}