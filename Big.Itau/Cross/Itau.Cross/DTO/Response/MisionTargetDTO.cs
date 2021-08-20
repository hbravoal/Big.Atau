using System;

namespace Itau.Common.DTO.Response
{
    public class MisionTargetDTO
    {
        public int Id { get; set; }
        public Guid CustomerGuid { get; set; }
        public int MisionId { get; set; }
        public decimal? Value { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ReadDate { get; set; }
        public DateTime CompleteDate { get; set; }
    }
}