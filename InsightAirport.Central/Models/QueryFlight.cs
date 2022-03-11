using System.ComponentModel.DataAnnotations;

namespace InsightAirport.Central.Models
{
    public class QueryFlight
    {
        public int? Id { get; set; }

        [MaxLength(6)]
        public string? FlightCode { get; set; }

        [MaxLength(25)]
        public string? Model { get; set; }

        public Status? Status { get; set; }

        public DateTimeOffset? LastUpdate { get; set; }
    }
}
