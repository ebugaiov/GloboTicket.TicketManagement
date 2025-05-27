using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GloboTicket.TicketManagement.Domain.Entities;

namespace GloboTicket.TicketManagement.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);
    }
}