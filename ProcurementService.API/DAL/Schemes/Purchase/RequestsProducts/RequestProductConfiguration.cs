using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ProcurementService.API.DAL.Schemes.Purchase.RequestsProducts
{
    public class RequestProductConfiguration : IEntityTypeConfiguration<RequestProduct>
    {
        public void Configure(EntityTypeBuilder<RequestProduct> builder)
        {
            {
                builder.ToTable(" requests_products", "purchase");

                builder
                    .HasKey(rp => new { rp.RequestId, rp.ProductId });

                builder
                    .HasOne(rp => rp.Request)
                    .WithMany(r => r.RequestsProducts)
                    .HasForeignKey(rp => rp.RequestId);

                builder
                    .HasOne(rp => rp.Product)
                    .WithMany(p => p.RequestsProducts)
                    .HasForeignKey(rp => rp.ProductId);

                builder.Property(x => x.RequestId).HasColumnName("request_id").IsRequired(true);
                builder.Property(x => x.ProductId).HasColumnName("product_id").IsRequired(true);
                builder.Property(x => x.Count).HasColumnName("count").IsRequired(true);
            }
        }
    }
}
