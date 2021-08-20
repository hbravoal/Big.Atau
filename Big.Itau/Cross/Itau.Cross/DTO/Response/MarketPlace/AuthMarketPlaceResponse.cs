namespace Itau.Common.DTO.Response.MarketPlace
{
    public class AuthMarketPlaceResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Errors { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public string Id { get; set; }
        public string Provider { get; set; }
        public string JWToken { get; set; }
    }
}