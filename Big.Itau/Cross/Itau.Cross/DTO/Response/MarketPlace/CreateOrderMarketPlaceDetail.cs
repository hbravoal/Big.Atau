using Itau.Common.DTO.Response.MarketPlace;

namespace Itau.Common.DTO.Request.MarketPlace
{
    /// <summary>
    /// Detalle de la redención
    /// </summary>
    public class CreateOrderMarketPlaceDetail
    {
        public int Quantity { get; set; }
        public string ProductId { get; set; }
        public virtual ProductMarketPlaceResponse Product { get; set; }
        public int OrderId { get; set; }
        public int OrderDetailStatusId { get; set; }
        public bool Active { get; set; }
    }
}