namespace Itau.Common.DTO.Response
{
    public class LoginCustomerForIdentificationResponse
    {
        public CustomerDTO Customer { get; set; }
        public TokenDTO Token { get; set; }
    }
}