using System;

namespace Itau.Common.DTO.Request
{
    public class RedemptionPointInformation
    {
        public int MisionId { get; set; }
        public int ProgramId { get; set; }
        public Guid CustomerGuid { get; set; }
        public decimal? PointMision { get; set; }
        public string MessageRequest { get; set; }
        public decimal? PointAccumulation { get; set; }
    }
}