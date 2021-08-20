using System.Collections.Generic;
using Itau.Common.DTO.Request.MarketPlace;

namespace Itau.Common.DTO.Response.MarketPlace
{
   

    public class CreateOrderMarketPlaceResponse: RedemptionResponse
    {
        public bool Succeeded { get; set; }
        public object Message { get; set; }
        public object Errors { get; set; }
        public DataContainer Data { get; set; }
        public MarketPlace product { get; set; }
    }

    public class DataContainer
    {
        public string id { get; set; }
        public List<OrderExtendedMarketPlaceProperty> OrderExtendedProperties { get; set; }
    }

    public class Orderextendedproperty
    {
        public int type { get; set; }
        public int key { get; set; }
        public string value { get; set; }
        public int orderId { get; set; }
    }
    public class MarketPlace
    {
        public string productCode { get; set; }
        public string name { get; set; }
        public string shortDescription { get; set; }
        public string conditions { get; set; }
        public string image { get; set; }
    }

}