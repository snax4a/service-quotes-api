namespace ServiceQuotes.Application.Filters
{
    public class GetServiceRequestsFilter : SearchStringFilter
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
