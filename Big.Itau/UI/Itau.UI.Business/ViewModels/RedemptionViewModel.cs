using Itau.Common.Enums;
using System.Collections.Generic;

namespace Mastercard.UI.Business.ViewModels
{
    public class RedemptionViewModel
    {
        public string UserGuid { get; set; }
        public string UserToken { get; set; }
        public string Message { get; set; }
        public string MarketPlaceToken { get; set; }
        public int SegmentId { get; set; }

        /// <summary>
        /// Producto Id (Tipo entero en Quantum) y Guid (NetCommerce)
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Código product quantum y NetCommerce
        /// </summary>
        public string ProductCode { get; set; }

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
        /// Inventario: Quantum se toma de References
        /// </summary>
        public int Inventory { get; set; }

        /// <summary>
        /// Qué tipo de Producto es. [Encriptado]
        /// </summary>
        public EnumCatalogNetCommerceType CatalogType { get; set; }

        /// <summary>
        /// USO: PCO
        /// Identificación
        /// </summary>
        public string Identification { get; set; }

        /// <summary>
        /// USO: PCO
        ///
        /// </summary>
        public int IdentificationType { get; set; }

        /// <summary>
        /// USO:Net-Commerce
        /// Celular
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// USO: PCO
        /// Nombre
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// USO: PCO
        /// Apellido
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// USO:Net-Commerce
        /// Dirección
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// USO:Net-Commerce
        /// Correo del Cliente
        /// </summary>
        public string Email { get; set; }

        #region Quantum

        /// <summary>
        /// Si cuenta con información aidicional (Válido para Quantum).
        /// </summary>
        public bool AditionalInformation { get; set; }

        /// <summary>
        /// Si tiene location (Quantum)
        /// </summary>
        public bool Location { get; set; }

        /// <summary>
        /// Sitio para quantum (Necesario para redención).
        /// </summary>
        public List<Site> Site { get; set; }

        #endregion Quantum

        /// <summary>
        /// USO:Net-Commerce
        /// Lista de ciudades si aplica
        /// </summary>
        public List<City> City { get; set; }

        /// <summary>
        /// USO:Net-Commerce
        /// Departamentos
        /// </summary>
        //public List<Department> Department { get; set; }

        /// <summary>
        /// USO:Net-Commerce
        ///
        /// </summary>
        public int CityId { get; set; }

        public int SiteId { get; set; }
        public int DepartmentId { get; set; }

        /// <summary>
        /// Método para inicializar todo.
        /// </summary>
        /// <returns></returns>
        public RedemptionViewModel Inicializate()
        {
            this.Site = new List<Site>();
            this.City = new List<City>();
            return this;
        }
    }

    #region Quantum

    public class City
    {
        public int City_id { get; set; }
        public string City_name { get; set; }
        public string Dep_id { get; set; }
        public string Dep_name { get; set; }
    }

    public class Department
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
    }

    public class Site
    {
        public int Site_id { get; set; }
        public string Site_name { get; set; }
        public string Dep_id { get; set; }
        public string Dep_name { get; set; }
        public string City_id { get; set; }
        public string City_name { get; set; }
    }

    #endregion Quantum
}