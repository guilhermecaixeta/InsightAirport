using AutoFixture;
using InsightAirport.Core.Entities;
using InsightAirport.Core.Interfaces;
using InsightAirport.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Threading.Tasks;

namespace InsightAirportTests
{
    public abstract class Setup : IDisposable, IAsyncDisposable
    {
        protected Fixture Fixture { get; set; }

        private readonly InsightAirportContext _airportContext;
        private bool disposedValue;

        protected Setup()
        {
            Fixture = new Fixture();
            _airportContext = GetDbContext();
        }

        public InsightAirportContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<InsightAirportContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}-InsightAirportContextTest")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;

            return new InsightAirportContext(options);
        }

        protected IFligthReadRepository GetReadRepository() => 
            new FlightRepository(_airportContext);

        protected IWriteRepository<Flight, int> GetWriteRepository() =>
            new FlightRepository(_airportContext);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _airportContext.Database.EnsureDeleted();
                    _airportContext.Dispose();
                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Setup()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public ValueTask DisposeAsync()
        {
            _airportContext.Database.EnsureDeleted();
            return _airportContext.DisposeAsync();
        }
    }
}