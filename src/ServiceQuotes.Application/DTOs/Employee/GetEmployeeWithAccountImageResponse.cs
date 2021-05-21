using System;

namespace ServiceQuotes.Application.DTOs.Employee
{
    public class GetEmployeeWithAccountImageResponse : GetEmployeeWithSpecializationsResponse
    {
        public byte[] Image { get; set; }
    }
}
