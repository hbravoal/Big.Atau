using System;
using System.Collections.Generic;

namespace Itau.Common.DTO.Response
{
    public class CustomerAwardDTO
    {
        public Guid Guid { get; set; }
        public short AwardId { get; set; }
        public Guid CustomerGuid { get; set; }
        public DateTime LastChange { get; set; }
        public DateTime CreateDate { get; set; }
        public bool? Active { get; set; }
        public int Programid { get; set; }

    }
}