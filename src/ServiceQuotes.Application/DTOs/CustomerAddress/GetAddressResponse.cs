using System;

namespace ServiceQuotes.Application.DTOs.CustomerAddress
{
    public class GetAddressResponse
    {
        public Guid Id { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }

    }
}
