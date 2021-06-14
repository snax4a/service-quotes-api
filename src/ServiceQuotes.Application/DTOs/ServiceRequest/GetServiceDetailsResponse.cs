using ServiceQuotes.Application.DTOs.Customer;
using ServiceQuotes.Application.DTOs.CustomerAddress;
using ServiceQuotes.Application.DTOs.Employee;
using System.Collections.Generic;

namespace ServiceQuotes.Application.DTOs.ServiceRequest
{
    public class GetServiceDetailsResponse : GetServiceResponse
    {
        public GetCustomerWithImageResponse Customer { get; set; }
        public GetCustomerAddressResponse CustomerAddress { get; set; }
        public ICollection<GetEmployeeWithAccountImageResponse> AssignedEmployees { get; set; }
    }
}
