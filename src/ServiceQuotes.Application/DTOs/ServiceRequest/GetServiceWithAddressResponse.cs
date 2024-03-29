﻿using ServiceQuotes.Application.DTOs.CustomerAddress;

namespace ServiceQuotes.Application.DTOs.ServiceRequest
{
    public class GetServiceWithAddressResponse : GetServiceResponse
    {
        public GetCustomerAddressResponse CustomerAddress { get; set; }
    }
}
