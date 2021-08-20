using System.Collections.Generic;

namespace Itau.Common.DTO.Request.MarketPlace
{
    public class CreateOrderMarketPlaceRequest
    {
        /// <summary>
        /// Siempre será Quantum
        /// </summary>
        public string Provider { get; set; }
        public string MarketPlaceToken { get; set; }
        public CreateOrderMarketPlaceDataRequest Data { get; set; }
    }

    public class CreateOrderMarketPlaceDataRequest{
        public string ClientGuid { get; set; }
        public string ProductId { get; set; }
        public int ProgramId { get; set; }
        public string CustomerIdentification { get; set; }
        /// <summary>
        /// Nombre del Usuario (Requerido para algunos produts=
        /// </summary>
        public string Name { get; set; }
        //únicamente el Número : 1
        public string Segment { get; set; }

        public string Source { get; set; }
        public string SiteId { get; set; }
        public List<OrderExtendedMarketPlaceProperty> OrderExtendedProperties { get; set; }
    }

   
}