using System;

namespace ServiceQuotes.Application.DTOs.Account
{
    public class GetAccountWithEmployeeResponse : GetAccountResponse
    {
        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
