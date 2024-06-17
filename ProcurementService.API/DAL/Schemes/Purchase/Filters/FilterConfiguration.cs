using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProcurementService.API.DAL.Schemes.Purchase.Products;

namespace ProcurementService.API.DAL.Schemes.Purchase.Filters
{
    public class FilterConfiguration : IEntityTypeConfiguration<Filter>
    {
        public void Configure(EntityTypeBuilder<Filter> builder)
        {
            builder.ToTable("filters", "purchase");

            builder.HasAlternateKey(x => x.Id).HasName("FiltersPrimaryKey");

            builder
                .HasOne(x => x.Product)
                .WithOne(x => x.Filter)
                .HasPrincipalKey<Product>(x => x.Id)
                .HasForeignKey<Filter>(m => m.Id);

            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Manufacturer).HasColumnName("manufacturer").HasMaxLength(150).IsRequired(true);
            builder.Property(x => x.VRAM).HasColumnName("vram").HasDefaultValue(null);
            builder.Property(x => x.RAM).HasColumnName("ram").HasDefaultValue(null);
            builder.Property(x => x.SizeDisk).HasColumnName("size_disk").HasDefaultValue(null);
            builder.Property(x => x.TypeDisk).HasColumnName("type_disk").HasMaxLength(100).IsRequired(false);
            builder.Property(x => x.CountCors).HasColumnName("count_cors").HasDefaultValue(null);
            builder.Property(x => x.Diagonal).HasColumnName("diagonal").HasDefaultValue(null);
        }
    }
}
