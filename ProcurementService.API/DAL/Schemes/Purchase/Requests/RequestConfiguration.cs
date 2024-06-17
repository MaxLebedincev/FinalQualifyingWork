using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ProcurementService.API.DAL.Schemes.Purchase.Requests
{
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.ToTable("requests", "purchase");

            builder.HasKey(x => x.Id).HasName("RequestsPrimaryKey");

            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.CreateAt).HasColumnName("create_at").ValueGeneratedOnAdd().IsRequired(true);
            builder.Property(x => x.UpdateAt).HasColumnName("update_at").ValueGeneratedOnUpdate().IsRequired(true);
            builder.Property(x => x.IdCreate).HasColumnName("id_create").IsRequired(true);
            builder.Property(x => x.IdUpdate).HasColumnName("id_update").IsRequired(true);
            builder.Property(x => x.IsConfirmed).HasColumnName("is_confirmed").IsRequired(true);
            builder.Property(x => x.IdConfirmed).HasColumnName("id_confirmed").IsRequired(true);
            builder.Property(x => x.SummaryMain).HasColumnName("summary_main").IsRequired(true);
            builder.Property(x => x.SummarySub).HasColumnName("summary_sub").IsRequired(true);

        }
    }
}
