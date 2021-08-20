namespace Itau.Common.DTO.Request.MarketPlace
{
    /// <summary>
    /// Request para obtener maestras Quantum
    /// </summary>
    public class GetMasterRequest
    {
        public string brand_id { get; set; }
        public string dep_id { get; set; }
        public string city_id { get; set; }
    }
}