namespace Mastercard.UI.Business.ViewModels
{
    /// <summary>
    /// Model Cross between NetCommerce and MarketPlace (Quantum)
    /// </summary>
    public class CatalogViewModel
    {
        /// <summary>
        /// Id (Tipo entero en Quantum) y Guid (NetCommerce)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Código product quantum y NetCommerce
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Nombre  product quantum y NetCommerce ProductName
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Segmento real del Cliente
        /// </summary>
        public string RealSegment { get; set; }

        /// <summary>
        /// Descripción corta
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Descripción larga
        /// </summary>
        public string LongDescription { get; set; }

        /// <summary>
        /// Image quantum y NetCommerce ProductImage
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Instrucciones de uso
        /// </summary>
        public string Instructions { get; set; }

        /// <summary>
        /// Marca del Producto| Quantum:BrandCode y NetCommerce BrandId
        /// </summary>
        public string BrandId { get; set; }

        /// <summary>
        /// Programa
        /// </summary>
        public string ProgramId { get; set; }

        /// <summary>
        /// Si el producto está activo
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Número de Página
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Tamaño de Página
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total de páginas
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Inventario: Quantum se toma de References
        /// </summary>
        public int Inventory { get; set; }

        /// <summary>
        /// Terminos y condiciones del producto
        /// </summary>
        public string TermsAndConditions { get; set; }

        /// <summary>
        /// Qué tipo de Producto es.
        /// </summary>
        public string CatalogType { get; set; }

        /// <summary>
        /// Si tiene location
        /// </summary>
        public bool Location { get; set; }

        /// <summary>
        /// Si cuenta con información aidicional (Válido para Quantum).
        /// </summary>
        public bool AditionalInformation { get; set; }

        public string Brand { get; set; }
    }
}