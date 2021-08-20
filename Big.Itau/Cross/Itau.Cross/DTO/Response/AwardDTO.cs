using System;

namespace Itau.Common.DTO.Response
{
    public class AwardDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public short Inventory { get; set; }
        public int QuantityDesired { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProgramId { get; set; }
        public bool? Active { get; set; }
        public string Description { get; set; }
        public CustomerAwardDTO CustomerAwar { get; set; }

    }
}