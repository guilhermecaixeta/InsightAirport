using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;
using InsightAirport.DTO;
using Microsoft.AspNetCore.Mvc;

namespace InsightAirport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FlightController : ControllerBase
    {
        private readonly IUseCaseFactory useCaseFactory;

        public FlightController(IUseCaseFactory useCaseFactory)
        {
            this.useCaseFactory = useCaseFactory ?? throw new ArgumentNullException(nameof(useCaseFactory));
        }

        /// <summary>Gets the specified query.</summary>
        /// <param name="query">The query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FlightDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get([FromQuery] QueryFlightDTO query, CancellationToken cancellationToken)
        {
            var response = await useCaseFactory.CreateAsync<QueryFlight, IReadOnlyList<Flight>>("GetFlightsUseCase", (QueryFlight)query, cancellationToken);

            if (response == null || !response.Any())
            {
                return NoContent();
            }

            var responseConverted = response.Select(flight => (FlightDTO)flight);
            return Ok(responseConverted);
        }

        /// <summary>Gets the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(FlightDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            if(id < 1)
            {
                return NoContent();
            }

            var response = await useCaseFactory.CreateAsync<int, Flight?>("GetFlightUseCase", id, cancellationToken);

            if (response is null)
            {
                return NoContent();
            }

            return Ok((FlightDTO)response);
        }

        /// <summary>Posts the specified flight.</summary>
        /// <param name="flight">The flight.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        [ProducesResponseType(typeof(FlightDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] FlightDTO flight, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await useCaseFactory.CreateAsync<Flight, Flight>("AddFlightUseCase", (Flight)flight, cancellationToken);

            return Created($"/flights/{response.Id}", (FlightDTO)response);
        }


        /// <summary>Update flight to the next status.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, CancellationToken cancellationToken)
        {
            var query = new QueryFlight { Id = id };
            var existFlight = await useCaseFactory.CreateAsync<QueryFlight?, bool>("ExistsFlightUseCase", query, cancellationToken);

            if (!existFlight)
            {
                NoContent();
            }

            _ = await useCaseFactory.CreateAsync<int, bool>("UpdateFlightStatusUseCase", id, cancellationToken);

            return Ok();
        }

        /// <summary>Deletes the specified identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var query = new QueryFlight { Id = id };
            var existFlight = await useCaseFactory.CreateAsync<QueryFlight?, bool>("ExistsFlightUseCase", query, cancellationToken);

            if (!existFlight)
            {
                NoContent();
            }
            _ = await useCaseFactory.CreateAsync<int, bool>("DeleteFlightUseCase", id, cancellationToken);

            return Ok();
        }
    }
}
