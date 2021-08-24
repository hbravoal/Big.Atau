using Itau.Common.DTO.Request.MarketPlace;
using System;
using System.Collections.Generic;

namespace Itau.Common.DTO.Response
{
    public class MyRedeptionOrderResponseDTO
    {
        public string Id { get; set; }
        public int StatusId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public List<OrderDetailViewModel> orderDetails { get; set; }
        public List<OrderExtendedPropertyViewModel> OrderExtendedProperties { get; set; }
    }

    public class OrderExtendedPropertyViewModel
    {
        public OrderExtentedPropertyTypeEnum Type { get; set; }
        public OrderExtentedPropertyKeyEnum Key { get; set; }
        public string Value { get; set; }
        public int OrderId { get; set; }
    }

    public class OrderDetailViewModel
    {
        public int Quantity { get; set; }
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int OrderId { get; set; }
        public int OrderDetailStatusId { get; set; }
        public bool Active { get; set; }
    }

    public class Product
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Conditions { get; set; }
        public string Image { get; set; }
        public string Instructions { get; set; }
    }
}