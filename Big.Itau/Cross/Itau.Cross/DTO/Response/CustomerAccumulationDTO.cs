using System;

namespace Itau.Common.DTO.Response
{
    public class CustomerAccumulationDTO
    {
        public string Guid { get; set; }
        public string ClientGuid { get; set; }
        public decimal Available { get; set; }
        public decimal? ValueX { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastChange { get; set; }
    }
}