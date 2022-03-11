using InsightAirport.Core;
using InsightAirport.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace InsightAirport.DTO
{
    public class FlightDTO
    {
        public FlightDTO(int id, string flightCode, string model, StatusDTO status)
        {
            Id = id;
            FlightCode = flightCode;
            Model = model;
            Status = status;
        }

        public int Id { get; set; }

        [Required, MaxLength(6)]
        public string FlightCode { get; private set; }

        [Required, MaxLength(25)]
        public string Model { get; private set; }

        [Required]
        public StatusDTO Status { get; private set; }

        public static explicit operator Flight(FlightDTO flightDTO)
        {
            if (flightDTO == null)
            {
                throw new ArgumentNullException(nameof(FlightDTO));
            }

            if (!Enum.TryParse(flightDTO.Status.ToString(), out Status status))
            {
                throw new InvalidCastException($"Cannot convert status {flightDTO.Status}");
            }

            return new Flight(
                flightDTO.Id,
                flightDTO.FlightCode,
                flightDTO.Model,
                status);
        }

        public static explicit operator FlightDTO(Flight flight)
        {
            if (flight == null)
            {
                throw new ArgumentNullException(nameof(Flight));
            }

            if (!Enum.TryParse(flight.Status.ToString(), out StatusDTO status))
            {
                throw new InvalidCastException($"Cannot convert status {flight.Status}");
            }

            return new FlightDTO(flight.Id, flight.FlightCode, flight.Model, status);
        }
    }
}
