using System;

namespace ServiceQuotes.Application.DTOs.Employee
{
    public class GetEmployeeResponse
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
