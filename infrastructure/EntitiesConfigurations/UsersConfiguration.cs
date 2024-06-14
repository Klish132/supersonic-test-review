using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.EntitiesConfigurations;

internal class UsersConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(e => e.Email)
            .IsRequired();
        
        builder.Property(e => e.UserName)
            .IsRequired();
    }
}