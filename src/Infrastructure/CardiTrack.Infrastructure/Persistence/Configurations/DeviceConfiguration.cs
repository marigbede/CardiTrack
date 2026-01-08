using CardiTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardiTrack.Infrastructure.Persistence.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("Devices");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.DeviceType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(d => d.Manufacturer)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.ModelName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(d => d.DisplayName)
            .HasMaxLength(150)
            .IsRequired();

        // JSON fields
        builder.Property(d => d.Capabilities)
            .HasMaxLength(2000)
            .HasDefaultValue("{}");

        builder.Property(d => d.ApiEndpoint)
            .HasMaxLength(500);

        builder.Property(d => d.OAuthConfig)
            .HasMaxLength(2000);

        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(d => d.SortOrder)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(d => d.IconUrl)
            .HasMaxLength(500);

        builder.Property(d => d.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(d => d.UpdatedDate);

        // Indexes
        builder.HasIndex(d => d.DeviceType);
        builder.HasIndex(d => d.IsActive);
        builder.HasIndex(d => d.SortOrder);
    }
}
