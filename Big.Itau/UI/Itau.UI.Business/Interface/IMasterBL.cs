using Itau.Common.DTO.Request.MarketPlace;
using Itau.Common.DTO.Response;
using Itau.Common.DTO.Response.MarketPlace;
using Itau.Common.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mastercard.UI.Business.Interface
{
    /// <summary>
    ///  Master para PasalaGanando
    /// </summary>
    public interface IMasterBL
    {
        Response<List<CityDTO>> GetCitiesByDeparmentId(int departmentId);

        Response<List<DepartmentDTO>> GetDeparmentsByCountry(int countryId);
    }

    /// <summary>
    /// Master por quantum
    /// </summary>
    public interface IMasterQuantumBl
    {
        Task<Response<List<DepartmentQuantumResponse>>> GetDepartment(GetMasterRequest model);

        Task<Response<List<CityQuantumResponse>>> GetCity(GetMasterRequest model);

        Task<Response<List<SiteQuantumResponse>>> GetSite(GetMasterRequest model);
    }
}