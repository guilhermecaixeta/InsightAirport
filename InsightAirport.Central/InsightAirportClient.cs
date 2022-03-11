using InsightAirport.Central.Models;
using Refit;

namespace InsightAirport.Central
{
    public interface InsightAirportClient
    {
        [Get("/Flight/{id}")]
        Task<Flight> GetFlight(int id);

        [Get("/Flight")]
        Task<IEnumerable<Flight>> GetFlights([Query] QueryFlight? query);

        [Post("/Flight")]
        Task PostFlight([Body] Flight flight);

        [Put("/Flight/{id}")]
        Task PutFlight([AliasAs("id")] int id);
    }
}
