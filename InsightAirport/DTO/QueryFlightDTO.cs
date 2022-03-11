using InsightAirport.Core;
using InsightAirport.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace InsightAirport.DTO
{
    public class QueryFlightDTO
    {
        public int? Id { get; set; }

        [MaxLength(6)]
        public string? FlightCode { get; set; }

        [MaxLength(25)]
        public string? Model { get; set; }

        public Status? Status { get; set; }

        public DateTimeOffset? LastUpdate { get; set; }


        public static explicit operator QueryFlight(QueryFlightDTO? query)
        {
            if (query == null)
            {
                return null;
            }

            if (query.Status.HasValue)
            {

                if (!Enum.TryParse(query.Status.ToString(), out Status status))
                {
                    return new QueryFlight
                    {
                        Id = query.Id,
                        FlightCode = query.FlightCode,
                        Model = query.Model,
                        Status = status,
                        LastUpdate = query.LastUpdate,
                    };
                }
            }

            return new QueryFlight
            {
                Id = query.Id,
                FlightCode = query.FlightCode,
                Model = query.Model,
                LastUpdate = query.LastUpdate,
            };
        }
    }
}
