using CardiTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardiTrack.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.UserId)
            .IsRequired();

        builder.Property(a => a.CardiMemberId);

        builder.Property(a => a.Action)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.EntityType)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.EntityId);

        builder.Property(a => a.Timestamp)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(a => a.IpAddress)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.UserAgent)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(a => a.RequestPath)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(a => a.HttpMethod)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(a => a.ResponseStatus)
            .IsRequired();

        // JSON fields
        builder.Property(a => a.DataAccessed)
            .HasMaxLength(2000);

        builder.Property(a => a.ChangedFields)
            .HasMaxLength(2000);

        builder.Property(a => a.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(a => a.UpdatedDate);

        // Indexes for HIPAA compliance queries
        builder.HasIndex(a => a.UserId);
        builder.HasIndex(a => a.CardiMemberId);
        builder.HasIndex(a => a.Timestamp);
        builder.HasIndex(a => new { a.CardiMemberId, a.Timestamp });
        builder.HasIndex(a => new { a.UserId, a.Timestamp });
        builder.HasIndex(a => a.EntityType);
    }
}
