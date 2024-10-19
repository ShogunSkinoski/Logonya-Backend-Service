using Domain.Account.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration.AccountConfiguration;

public class ApiKeyEntityConfiguration : IEntityTypeConfiguration<ApiKey>
{
    public void Configure(EntityTypeBuilder<ApiKey> builder)
    {
        builder.ToTable("ApiKeys");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Key).IsRequired();
        builder.HasIndex(x => x.Key);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Description);
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}
