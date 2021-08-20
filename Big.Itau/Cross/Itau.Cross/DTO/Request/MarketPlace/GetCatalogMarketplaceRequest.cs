namespace Itau.Common.DTO.Request.MarketPlace
{
    /// <summary>
    /// Request for Quantum.
    /// </summary>
    public class GetCatalogMarketplaceRequest
    {
        public DataProductRequest Data { get; set; }
    }

    /// <summary>
    /// Product Request
    /// </summary>
    public class DataProductRequest
    {
        public int Id { get; set; }
        public string Segment { get; set; }
        public int ProgramId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}