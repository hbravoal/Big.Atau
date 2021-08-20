using System;

namespace Itau.Common.DTO.Response
{
    public class TokenDTO
    {
        public Guid CustomerGuid { get; set; }
        public string TokenX { get; set; }
        public DateTime TokenExpirationDate { get; set; }
    }
}