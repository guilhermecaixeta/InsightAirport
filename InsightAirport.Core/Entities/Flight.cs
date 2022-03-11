namespace InsightAirport.Core.Entities
{
    public class Flight
    {
        public Flight(int id, string flightCode, string model, Status status)
        {
            Id = id;
            FlightCode = flightCode;
            Model = model;
            Status = status;
            LastUpdate = DateTimeOffset.UtcNow;
        }

        public int Id { get; private set; }
        public string FlightCode { get; private set; }
        public string Model { get; private set; }
        public Status Status { get; private set; }
        public DateTimeOffset LastUpdate { get; private set; }

        public void ChangeStatus()
        {
            Status = Status switch
            {
                Status.RequestToDepart => Status.ScheduledToDepart,
                Status.RequestToLand => Status.ScheduledToLand,
                Status.ScheduledToDepart => Status.Departing,
                Status.ScheduledToLand => Status.Landing,
                Status.Departing => Status.HasDeparted,
                Status.Landing => Status.HasLanded,
                Status.HasDeparted => Status.RequestToLand,
                Status.HasLanded => Status.RequestToDepart,
                _ => throw new InvalidDataException($"Current status is unknown {Status}")
            };
        }
    }
}
