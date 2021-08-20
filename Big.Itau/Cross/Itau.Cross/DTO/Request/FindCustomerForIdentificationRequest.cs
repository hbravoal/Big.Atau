namespace Itau.Common.DTO.Request
{
    public class FindCustomerForIdentificationRequest
    {
        public int ProgramId { get; set; }
        public string Identification { get; set; }
        public string Password { get; set; }
    }
}