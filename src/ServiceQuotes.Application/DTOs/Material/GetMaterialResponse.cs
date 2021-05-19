using System;

namespace ServiceQuotes.Application.DTOs.Material
{
    public class GetMaterialResponse
    {
        public Guid Id { get; set; }
        public Guid ServiceRequestId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
