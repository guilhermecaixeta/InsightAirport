using FluentAssertions;
using InsightAirport.Core;
using InsightAirport.Core.Entities;
using System.Threading.Tasks;
using Xunit;

namespace InsightAirportTests.Core
{
    public class AddStatusStrategyTest : Setup
    {
        [Fact]
        public async Task AddStatusStrategy_WhenTheStatusIsNotAllowedAndExistsInDb_ReturnTrue()
        {
            // Arrange
            var readRepository = GetReadRepository();
            var writeRepository = GetWriteRepository();
            var addStatusStrategy = new AddStatusStrategy(readRepository);

            var someFlight = new Flight(0, "so-1234", "some model", Status.Departing);

            await writeRepository.AddAsync(someFlight, default);

            var input = new Flight(0, "in-1234", "input", Status.Departing);

            // Act
            var result = await addStatusStrategy.ExecuteAsync(input, default);

            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task AddStatusStrategy_WhenTheStatusIsNotAllowedAndNoExistsInDb_ReturnFalse()
        {
            // Arrange
            var readRepository = GetReadRepository();
            var writeRepository = GetWriteRepository();
            var addStatusStrategy = new AddStatusStrategy(readRepository);

            var someFlight = new Flight(0, "so-1234", "some model", Status.Landing);

            await writeRepository.AddAsync(someFlight, default);

            var input = new Flight(0, "in-1234", "input", Status.Departing);

            // Act
            var result = await addStatusStrategy.ExecuteAsync(input, default);

            // Assert
            result.Should().Be(false);
        }

        [Fact]
        public async Task AddStatusStrategy_WhenTheStatusIsAllowed_ReturnFalse()
        {
            // Arrange
            var readRepository = GetReadRepository();
            var writeRepository = GetWriteRepository();
            var addStatusStrategy = new AddStatusStrategy(readRepository);

            var someFlight = new Flight(0, "so-1234", "some model", Status.Departing);

            await writeRepository.AddAsync(someFlight, default);

            var input = new Flight(0, "in-1234", "input", Status.RequestToDepart);

            // Act
            var result = await addStatusStrategy.ExecuteAsync(input, default);

            // Assert
            result.Should().Be(false);
        }
    }
}
