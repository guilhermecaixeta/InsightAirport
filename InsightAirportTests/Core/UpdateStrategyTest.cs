using FluentAssertions;
using InsightAirport.Core;
using InsightAirport.Core.Entities;
using System.Threading.Tasks;
using Xunit;

namespace InsightAirportTests.Core
{
    public class UpdateStrategyTest : Setup
    {
        [Fact]
        public async Task UpdateStatusStrategy_WhenTheStatusIsNotAllowedAndExistsInDb_ReturnTrue()
        {
            // Arrange
            var readRepository = GetReadRepository();
            var writeRepository = GetWriteRepository();
            var updateStatusStrategy = new UpdateStatusStrategy(readRepository);

            var someFlight = new Flight(0, "so-1234", "some model", Status.Departing);

            await writeRepository.AddAsync(someFlight, default);

            var input = new Flight(0, "in-1234", "input", Status.Departing);

            // Act
            var result = await updateStatusStrategy.ExecuteAsync(input, default);

            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task UpdateStatusStrategy_WhenTheStatusIsNotAllowedAndNoExistsInDb_ReturnFalse()
        {
            // Arrange
            var readRepository = GetReadRepository();
            var writeRepository = GetWriteRepository();
            var UpdateStatusStrategy = new UpdateStatusStrategy(readRepository);

            var someFlight = new Flight(0, "so-1234", "some model", Status.Landing);

            await writeRepository.AddAsync(someFlight, default);

            var input = new Flight(0, "in-1234", "input", Status.Departing);

            // Act
            var result = await UpdateStatusStrategy.ExecuteAsync(input, default);

            // Assert
            result.Should().Be(false);
        }

        [Fact]
        public async Task UpdateStatusStrategy_WhenTheStatusIsAllowed_ReturnFalse()
        {
            // Arrange
            var readRepository = GetReadRepository();
            var writeRepository = GetWriteRepository();
            var UpdateStatusStrategy = new UpdateStatusStrategy(readRepository);

            var someFlight = new Flight(0, "so-1234", "some model", Status.Departing);

            await writeRepository.AddAsync(someFlight, default);

            var input = new Flight(0, "in-1234", "input", Status.RequestToDepart);

            // Act
            var result = await UpdateStatusStrategy.ExecuteAsync(input, default);

            // Assert
            result.Should().Be(false);
        }
    }
}
