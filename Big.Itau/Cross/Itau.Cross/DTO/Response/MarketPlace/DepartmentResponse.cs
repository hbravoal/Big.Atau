namespace Itau.Common.DTO.Response.MarketPlace
{
    /// <summary>
    /// Response para Obtener departamentso de quantum
    /// </summary>
    public class QuantumDeparmentResponse
    {
        public QuantumDepartmentMessage response { get; set; }
    }

    public class QuantumDepartmentMessage
    {
        public DepartmentQuantumResponse[] message { get; set; }
        public bool error { get; set; }
    }

    public class DepartmentQuantumResponse
    {
        public int dep_id { get; set; }
        public string dep_name { get; set; }
    }



    public class QuantumCityResponse
    {
        public QuantumCityMessage response { get; set; }
    }
    public class CityQuantumResponse
    {
        public int city_id { get; set; }
        public string city_name { get; set; }
    }

    public class QuantumCityMessage
    {
        public CityQuantumResponse[] message { get; set; }
        public bool error { get; set; }
    }

    public class QuantumSiteResponse
    {
        public QuantumSiteMessage response { get; set; }
    }
    public class SiteQuantumResponse
    {
        public int Site_id { get; set; }
        public string Site_name { get; set; }
    }

    public class QuantumSiteMessage
    {
        public SiteQuantumResponse[] message { get; set; }
        public bool error { get; set; }
    }
}