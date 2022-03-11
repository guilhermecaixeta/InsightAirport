using FluentAssertions;
using InsightAirport.Core;
using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace InsightAirportTests.Core
{
    public class ContextStrategyTest : Setup
    {
        [Fact]
        public async Task ContextStrategy_WhenFindTheStrategy_ReturnValidResult()
        {
            var contextStrategy = new ContextStrategy(GetServiceProvider());

            var input = new Flight(0, "in-1234", "input", Status.Departing);

            var result = await contextStrategy.HandleAsync<Flight, bool>(nameof(AddStatusStrategy), input, default);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task ContextStrategy_WhenDontFindTheStrategy_ThrowException()
        {
            var contextStrategy = new ContextStrategy(GetServiceProvider());

            var input = new Flight(0, "in-1234", "input", Status.Departing);
            var strategyName = "SomeStrategy";
            var result = await Assert.ThrowsAsync<NullReferenceException>(() => contextStrategy.HandleAsync<Flight, bool>(strategyName, input, default));

            result.Message.Should().Be($"Strategy {strategyName} not found");
        }

        private IServiceProvider GetServiceProvider()
        {
            var readRepository = GetReadRepository();
            var addStatusStrategy = new AddStatusStrategy(readRepository);
            var updateStatusStrategy = new UpdateStatusStrategy(readRepository);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IStrategy<Flight, bool>>(provider => addStatusStrategy);
            serviceCollection.AddScoped<IStrategy<Flight, bool>>(provider => updateStatusStrategy);
            return serviceCollection.BuildServiceProvider();
        }
    }
}
