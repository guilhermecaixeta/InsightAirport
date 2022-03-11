using FluentAssertions;
using InsightAirport.Core;
using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;
using InsightAirport.Core.UseCases;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InsightAirportTests.Core.UseCases
{
    public class AddFlighUseCaseTest : Setup
    {
        [Fact]
        public async Task AddFlighUseCase_WhenTheInputIsValid_IsPersisted()
        {
            var mockContextStrategy = new Mock<IContextStrategy>();
            var writeRepository = GetWriteRepository();
            var addFlightUseCase = new AddFlightUseCase(writeRepository, mockContextStrategy.Object);
            var input = new Flight(0, "in-1234", "input", Status.Departing);

            mockContextStrategy
                .Setup(x => x.HandleAsync<Flight, bool>(It.IsAny<string>(), It.IsAny<Flight>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await addFlightUseCase.HandleAsync(input, default);

            result.Id.Should().NotBe(0);
            result.Model.Should().Be(input.Model);
            result.Status.Should().Be(input.Status);
            result.FlightCode.Should().Be(input.FlightCode);
        }

        [Fact]
        public async Task AddFlighUseCase_WhenTheInputIsInValid_ExceptionIsThrown()
        {
            var mockContextStrategy = new Mock<IContextStrategy>();
            var writeRepository = GetWriteRepository();
            var addFlightUseCase = new AddFlightUseCase(writeRepository, mockContextStrategy.Object);
            var input = new Flight(0, "in-1234", "input", Status.Departing);

            mockContextStrategy
                .Setup(x => x.HandleAsync<Flight, bool>(It.IsAny<string>(), It.IsAny<Flight>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await Assert.ThrowsAsync<Exception>(() => addFlightUseCase.HandleAsync(input, default));

            result.Should().NotBeNull();
            result.Message.Should().Be($"Already exists an airplane in status '{input.Status}'");
        }
    }
}
