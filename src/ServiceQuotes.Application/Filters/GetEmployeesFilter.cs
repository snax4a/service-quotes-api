namespace ServiceQuotes.Application.Filters
{
    public class GetEmployeesFilter : SearchStringFilter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
