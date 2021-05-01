using ServiceQuotes.Application.DTOs.Specialization;
using System;
using System.Collections.Generic;

namespace ServiceQuotes.Application.DTOs.Employee
{
    public class GetEmployeeWithSpecializations
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<GetSpecializationResponse> Specializations { get; set; }
    }
}
