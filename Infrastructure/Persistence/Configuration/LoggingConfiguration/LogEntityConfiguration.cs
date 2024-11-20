using Domain.Logging.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.LoggingConfiguration;

internal class LogEntityConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.ToTable("Logs");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Action).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x => x.Level).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Source).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Environment).HasMaxLength(50).IsRequired();
        builder.Property(x => x.IpAddress).HasMaxLength(50).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        
        builder.Property(x => x.UserName).HasMaxLength(200);
        builder.Property(x => x.Exception);
        builder.Property(x => x.StackTrace);

        builder.Property("_metadata")
            .HasColumnName("Metadata")
            .HasColumnType("jsonb");

        builder.HasOne(x => x.User)
            .WithMany(u => u.Logs)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.SetNull);
            
        builder.HasOne(x => x.ApiKey)
            .WithMany(a => a.Logs)
            .HasForeignKey(x => x.ApiKeyId)
            .OnDelete(DeleteBehavior.SetNull);
            
        builder.HasIndex(x => x.CreatedAt);
        builder.HasIndex(x => x.Level);
        builder.HasIndex(x => x.Source);
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.ApiKeyId);
    }
}
