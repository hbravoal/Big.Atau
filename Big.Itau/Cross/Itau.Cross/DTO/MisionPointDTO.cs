using Itau.Common.DTO.Response;
using System;
using System.Collections.Generic;

namespace Itau.Common.DTO
{
    public class MisionPointDTO
    {
        public List<MisionDTO> Misions { get; set; }
        public List<MisionTargetDTO> MisionsTarget { get; set; }
        public int PointAcumulate { get; set; }
        public DateTime ReadDate { get; set; }
    }
}