using InsightAirport.Core.Entities;
using InsightAirport.Infra.Models;
using Microsoft.EntityFrameworkCore;

namespace InsightAirport.Infra
{
    public class InsightAirportContext : DbContext
    {
        public InsightAirportContext(DbContextOptions<InsightAirportContext> options)
        : base(options)
        { }

        public DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new FlightEntityTypeConfiguration().Configure(modelBuilder.Entity<Flight>());
        }
    }
}
