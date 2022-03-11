using InsightAirport.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InsightAirport.Infra.Models
{
    public class FlightEntityTypeConfiguration : IEntityTypeConfiguration<Flight>
    {
        public void Configure(EntityTypeBuilder<Flight> builder)
        {
            builder
                .HasKey(x => x.Id);
            builder
                .Property(x => x.FlightCode)
                .HasMaxLength(6)
                .IsRequired(true);
            builder
                .Property(x => x.Model)
                .HasMaxLength(25)
                .IsRequired(true);
            builder
                .Property(x => x.Status)
                .IsRequired(true)
                .HasConversion<string>();
            builder
                .Property(x => x.LastUpdate)
                .IsRequired(true);
        }
    }
}
