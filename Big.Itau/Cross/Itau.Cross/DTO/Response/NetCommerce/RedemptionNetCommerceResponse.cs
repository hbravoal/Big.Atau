using System;

namespace Itau.Common.DTO.Response.NetCommerce
{
    /// <summary>
    /// DTO para redención netcommerce
    /// </summary>
    public class RedemptionNetCommerceResponse : RedemptionResponse
    {
        public string CustomerId { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }
        public string Approval { get; set; }
        public int Status { get; set; }
        public string Identification { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public RedemptionNetCommerceDetailResponse Detail { get; set; }
    }

    public class RedemptionNetCommerceDetailResponse
    {
        public string Guid { get; set; }
        public string ProductGuid { get; set; }
        public string ProductName { get; set; }
        public string Reference { get; set; }
        public int Quantity { get; set; }
        public string ReferenceGuid { get; set; }
        public int StatusId { get; set; }
        public string ImageUrl { get; set; }
        public string ProductCode { get; set; }
        public string Instructions { get; set; }
        public string Terms { get; set; }
        public DateTime Expiration { get; set; }
        public string OrderGuid { get; set; }
        public string LinkPdfRedemption { get; set; }
        public int Category_Id { get; set; }
    }
}