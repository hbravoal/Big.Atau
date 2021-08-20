namespace Itau.Common.DTO.Request.MarketPlace
{
    public class OrderExtendedMarketPlaceProperty
    {
        public OrderExtentedPropertyTypeEnum Type { get; set; }
        public OrderExtentedPropertyKeyEnum Key { get; set; }
        public string Value { get; set; }
        public int OrderId { get; set; }
    }

    /// <summary>
    ///Markertplace Enuim
    /// </summary>
    public enum OrderExtentedPropertyKeyEnum
    {
        LinkPdf = 1, //Guarda link del Pdf de redención
        Identifier, //Identificador.
        Error
    }

    ///Markertplace Enuim
    public enum OrderExtentedPropertyTypeEnum
    {
        String = 1,
        Integer
    }
}