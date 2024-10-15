using Domain.User.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Data.UserConfiguration;

internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserName).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => x.UserName);
        builder.Property(x=> x.Email).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => x.Email);
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}
