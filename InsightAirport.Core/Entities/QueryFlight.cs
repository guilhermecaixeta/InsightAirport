namespace InsightAirport.Core.Entities
{
    public class QueryFlight
    {
        public int? Id { get; set; }
        public string? FlightCode { get; set; }
        public string? Model { get; set; }
        public Status? Status { get; set; }
        public DateTimeOffset? LastUpdate { get; set; }
    }
}
