namespace ServiceQuotes.Application.Filters
{
    public class GetCustomersFilter : SearchStringFilter
    {
        public string CompanyName { get; set; }
        public string VatNumber { get; set; }
    }
}
