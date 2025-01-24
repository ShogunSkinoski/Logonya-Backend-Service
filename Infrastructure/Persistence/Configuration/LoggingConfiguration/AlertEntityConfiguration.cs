using Domain.Logging.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.LoggingConfiguration
{
    public class AlertEntityConfiguration : IEntityTypeConfiguration<Alert>
    {
        public void Configure(EntityTypeBuilder<Alert> builder)
        {
            builder.ToTable("Alerts");
            
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Name)
                .HasMaxLength(200)
                .IsRequired();
                
            builder.Property(x => x.Description)
                .HasMaxLength(500);
                
            builder.Property(x => x.Condition)
                .HasColumnType("jsonb")
                .IsRequired();
                
            builder.Property(x => x.Channel)
                .HasMaxLength(50)
                .IsRequired();
                
            builder.Property(x => x.Target)
                .HasMaxLength(500)
                .IsRequired();
                
            builder.Property(x => x.IsActive)
                .IsRequired();
                
            builder.Property(x => x.CreatedAt)
                .IsRequired();
                
            builder.Property(x => x.LastTriggeredAt);

            // Indexes
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.IsActive);

        }
    }
} 