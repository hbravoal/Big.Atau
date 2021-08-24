using Itau.Common.DTO.Response;

namespace Itau.Common.DTO.Request.NetCommerce
{
    public class RedemptionNetCommerceRequest
    {
        public CustomerDTO Customer { get; set; }

        /// <summary>
        /// Identificador del Programa para NetCommerce.
        /// </summary>
        public int NetCommerceId { get; set; }

        /// <summary>
        /// Nombre del Programa de donde viene la redención.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Identificador del GUID del producto.
        /// </summary>
        public string ProductGuid { get; set; }

        #region Information

        /// <summary>
        /// Dirección
        /// </summary>
        public string Address { get; set; }

        public int StateId { get; set; }
        public int CityId { get; set; }

        public int Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }

        public string IpClient { get; set; }

        #endregion Information
    }
}