using System.Collections.Generic;

namespace Itau.Common.DTO.Response.MarketPlace
{
    /// <summary>
    /// Response from Quantum Integration
    /// </summary>
    public class ProductMarketPlaceResponse
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Segment { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Conditions { get; set; }
        public string Image { get; set; }
        public string Instructions { get; set; }
        public List<ProductReference> ProductReferences { get; set; }
        public Brand Brand { get; set; }
        public int ProductTypeId { get; set; }
        public int ProgramId { get; set; }
        public int BrandId { get; set; }
        public bool Active { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class ProductReference
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public int Inventory { get; set; }
        public bool Active { get; set; }
    }

    public class Brand
    {
        public int Id { get; set; }
        public string BrandCode { get; set; }
        public string Description { get; set; }
        public bool Flag { get; set; }

        public bool additionalData { get; set; }

    }
}