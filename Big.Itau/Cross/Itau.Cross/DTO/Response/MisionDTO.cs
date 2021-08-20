using System;

namespace Itau.Common.DTO.Response
{
    public class MisionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Display { get; set; }
        public string Detail { get; set; }
        public string ShortDescription { get; set; }
        public decimal Point { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public short TypeId { get; set; }
        public byte ProgramId { get; set; }
        public bool Active { get; set; }
    }
}