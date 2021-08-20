namespace Itau.Common.DTO.Response
{
    /// <summary>
    /// Clase abstracta para retornar la respuesta de una redención.
    /// </summary>
    public class RedemptionResponse
    {
        /// <summary>
        /// Identificador de la redención
        /// OrderGuid (Net), Id en 
        /// </summary>
        public  string Id{ get; set; }

        /// <summary>
        /// Link del PDF
        /// </summary>
        public string  LinkPdf{ get; set; }
        /// <summary>
        /// Expiración
        /// </summary>

        public string ExpirationDate{ get; set; }
        /// <summary>
        /// Código Digital
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Mensaje a mostrar al cliente
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Title{ get; set; }
        /// <summary>
        /// imagen del producto redimido
        /// </summary>
        public string ImageUrl { get; set; }
        
        public int Type { get; set; }
    }
}