using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config;

internal class ProductDbConfig : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Name).HasMaxLength(128).IsRequired();
        builder.Property(p => p.ManufacturePhone).HasMaxLength(32);
        builder.Property(p => p.ManufactureEmail).HasMaxLength(128).IsRequired();
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("GetDate()");
        builder.Property(p => p.Description).HasMaxLength(512);

        builder.HasIndex(p => new { p.ProduceDate, p.ManufactureEmail }).IsUnique();

        builder.HasOne(p => p.Creator)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
