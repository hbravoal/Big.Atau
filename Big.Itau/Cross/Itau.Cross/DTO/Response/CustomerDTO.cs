using System;

namespace Itau.Common.DTO.Response
{
    public class CustomerDTO
    {
        public Guid Guid { get; set; }

        public string Id { get; set; }
        public string Identification { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
        public int ProgramId { get; set; }
        public int GenderId { get; set; }
        public string Email { get; set; }
        public bool Locked { get; set; }
        public DateTime LockDate { get; set; }
        public int InvalidLogins { get; set; }

        public int ConfigurationId { get; set; }
        public int CityId { get; set; }
        public int NetCommerceId { get; set; }
        public bool ChellengeCheck { get; set; }
    }
}