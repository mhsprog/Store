using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config;

internal class UserDbConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(p => p.FirstName).HasMaxLength(64).IsRequired();
        builder.Property(p => p.LastName).HasMaxLength(128);
        builder.Property(x => x.UserName).HasMaxLength(128);
        builder.Property(x => x.NormalizedUserName).HasMaxLength(128);
        builder.Property(x => x.PhoneNumber).HasMaxLength(32);
        builder.Property(x => x.Email).HasMaxLength(128).IsRequired();
        builder.Property(x => x.NormalizedEmail).HasMaxLength(128);
    }
}
