using ServiceQuotes.Domain.Entities.Enums;

namespace ServiceQuotes.Application.Filters
{
    public class GetEmployeesFilter : SearchStringFilter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SpecializationId { get; set; }
        public Role? Role { get; set; }
    }
}
