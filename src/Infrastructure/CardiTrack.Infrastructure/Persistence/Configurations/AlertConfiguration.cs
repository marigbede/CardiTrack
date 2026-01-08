using CardiTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardiTrack.Infrastructure.Persistence.Configurations;

public class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.ToTable("Alerts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.CardiMemberId)
            .IsRequired();

        builder.Property(a => a.AlertType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.Severity)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(a => a.Title)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(a => a.Message)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(a => a.TriggeredDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(a => a.AcknowledgedDate);

        builder.Property(a => a.AcknowledgedByUserId);

        builder.Property(a => a.IsResolved)
            .IsRequired()
            .HasDefaultValue(false);

        // JSON field
        builder.Property(a => a.MetricValues)
            .HasMaxLength(2000);

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(a => a.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(a => a.UpdatedDate);

        // Indexes
        builder.HasIndex(a => a.CardiMemberId);
        builder.HasIndex(a => a.AlertType);
        builder.HasIndex(a => a.Severity);
        builder.HasIndex(a => a.TriggeredDate);
        builder.HasIndex(a => a.IsResolved);
        builder.HasIndex(a => new { a.CardiMemberId, a.IsResolved, a.TriggeredDate });
    }
}
