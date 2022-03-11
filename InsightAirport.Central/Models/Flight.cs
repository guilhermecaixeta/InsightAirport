namespace InsightAirport.Central.Models
{
    public class Flight
    {
        public Flight(int id, string flightCode, string model, Status status)
        {
            Id = id;
            FlightCode = flightCode;
            Model = model;
            Status = status;
        }

        public int Id { get; set; }

        public string FlightCode { get; set; }

        public string Model { get; set; }

        public Status Status { get; set; }
    }
}
