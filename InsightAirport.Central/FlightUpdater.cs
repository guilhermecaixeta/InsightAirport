using InsightAirport.Central.Models;
using Refit;
using System.Net;

namespace InsightAirport.Central
{
    internal class FlightUpdater : IFlightUpdater
    {
        private readonly InsightAirportClient insightAirportClient;
        private readonly ILogger<FlightsGenerator> _logger;

        public FlightUpdater(InsightAirportClient insightAirportClient, ILogger<FlightsGenerator> logger)
        {
            this.insightAirportClient = insightAirportClient;
            _logger = logger;
        }

        public async Task UpdateAsync(CancellationToken cancellationToken)
        {
            var taskRequestTo = UpdateFlightsTo(5, Status.RequestToLand, Status.RequestToDepart, cancellationToken);
            var taskScheduleTo = UpdateFlightsTo(5, Status.ScheduledToLand, Status.ScheduledToDepart, cancellationToken);
            var taskInTo = UpdateFlightsTo(5, Status.Landing, Status.Departing, cancellationToken);
            var taskHasTo = UpdateFlightsTo(5, Status.HasLanded, Status.HasDeparted, cancellationToken);

            await Task.WhenAll(taskRequestTo, taskScheduleTo, taskInTo, taskHasTo);
        }

        private async Task UpdateFlightsTo(int timeInSeconds, Status toLand, Status toDepart, CancellationToken cancellationToken)
        {
            var taskToLand = UppdateAccordingStatus(timeInSeconds, toLand, cancellationToken);
            var taskToDepart = UppdateAccordingStatus(timeInSeconds, toDepart, cancellationToken);

            await Task.WhenAll(taskToDepart, taskToLand);
        }

        private async Task UppdateAccordingStatus(int timeInSeconds, Status status, CancellationToken cancellationToken)
        {

            var flightsScheduledToLand = await SafeRequest(() => insightAirportClient.GetFlights(new QueryFlight { Status = status }));

            if (flightsScheduledToLand == null || !flightsScheduledToLand.Any())
            {
                return;
            }

            foreach (var flight in flightsScheduledToLand)
            {
                try
                {
                    await insightAirportClient.PutFlight(flight.Id);
                    await Task.Delay(TimeSpan.FromSeconds(timeInSeconds), cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        private async Task<IEnumerable<Flight>> SafeRequest(Func<Task<IEnumerable<Flight>>> request)
        {
            try
            {
                return await request();
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);

                return new List<Flight>();
            }
        }
    }
}
