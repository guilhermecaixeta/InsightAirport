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
    public class UpdateFlighStatusUseCaseTest : Setup
    {
        [Theory]
        [InlineData(Status.RequestToLand, Status.ScheduledToLand)]
        //[InlineData(Status.RequestToDepart, Status.ScheduledToDepart)]
        //[InlineData(Status.ScheduledToDepart, Status.Departing)]
        //[InlineData(Status.ScheduledToLand, Status.Landing)]
        //[InlineData(Status.Departing, Status.HasDeparted)]
        //[InlineData(Status.Landing, Status.HasLanded)]
        //[InlineData(Status.HasDeparted, Status.RequestToLand)]
        //[InlineData(Status.HasLanded, Status.RequestToDepart)]
        public async Task UpdateFlighStatusUseCase_WhenTheInputIsValid_IsPersisted(Status currentStatus, Status nextStatus)
        {
            var mockContextStrategy = new Mock<IContextStrategy>();
            var writeRepository = GetWriteRepository();
            var readRepository = GetReadRepository();
            var updateFlightStatusUseCase = new UpdateFlightStatusUseCase(readRepository, writeRepository, mockContextStrategy.Object);

            mockContextStrategy
                .Setup(x => x.HandleAsync<Flight, bool>(It.IsAny<string>(), It.IsAny<Flight>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var input = new Flight(0, "in-1234", "input", currentStatus);
            _ = await writeRepository.AddAsync(input, default);

            var result = await updateFlightStatusUseCase.HandleAsync(input.Id, default);

            var updateFlight = await readRepository.SingleAsync(new QueryFlight { Id = input.Id}, default);

            result.Should().BeTrue();
            updateFlight.Status.Should().Be(nextStatus);
        }

        [Fact]
        public async Task UpdateFlighStatusUseCase_WhenTheInputIsInValid_ExceptionIsThrown()
        {
            var mockContextStrategy = new Mock<IContextStrategy>();
            var writeRepository = GetWriteRepository();
            var readRepository = GetReadRepository();
            var updateFlightStatusUseCase = new UpdateFlightStatusUseCase(readRepository, writeRepository, mockContextStrategy.Object);

            mockContextStrategy
                .Setup(x => x.HandleAsync<Flight, bool>(It.IsAny<string>(), It.IsAny<Flight>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            
            var input = new Flight(0, "in-1234", "input", Status.ScheduledToDepart);
            _ = await writeRepository.AddAsync(input, default);

            var result = await Assert.ThrowsAsync<Exception>(() => updateFlightStatusUseCase.HandleAsync(input.Id, default));

            result.Should().NotBeNull();
            result.Message.Should().Be($"Already exists an airplane in status '{Status.Departing}'");
        }

        [Fact]
        public async Task UpdateFlighStatusUseCase_WhenTheInputDoesntExist_ExceptionIsThrown()
        {
            var mockContextStrategy = new Mock<IContextStrategy>();
            var writeRepository = GetWriteRepository();
            var readRepository = GetReadRepository();
            var updateFlightStatusUseCase = new UpdateFlightStatusUseCase(readRepository, writeRepository, mockContextStrategy.Object);
            var input = new Flight(0, "in-1234", "input", Status.Departing);

            var result = await Assert.ThrowsAsync<InvalidOperationException>(() => updateFlightStatusUseCase.HandleAsync(1, default));

            result.Should().NotBeNull();
            result.Message.Should().Be($"Sequence contains no elements");
        }
    }
}
