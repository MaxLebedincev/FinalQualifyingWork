using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ProcurementService.API.DAL.Schemes.Purchase.Requests;

namespace ProcurementService.API.DAL.Schemes.Purchase.Files
{
    public class ServerFileConfiguration : IEntityTypeConfiguration<ServerFile>
    {
        public void Configure(EntityTypeBuilder<ServerFile> builder)
        {
            builder.ToTable("files", "purchase");

            builder.HasKey(x => x.Id).HasName("FilesPrimaryKey");

            builder
                .HasOne(x => x.Request)
                .WithOne(x => x.File)
                .HasPrincipalKey<Request>(x => x.Id)
                .HasForeignKey<ServerFile>(m => m.Id);

            builder.Property(x => x.Id).HasColumnName("id").IsRequired(true);
            builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.CreateAt).HasColumnName("create_at").ValueGeneratedOnAdd().IsRequired(true);
            builder.Property(x => x.UpdateAt).HasColumnName("update_at").ValueGeneratedOnUpdate().IsRequired(true);
            builder.Property(x => x.IdCreate).HasColumnName("id_create").IsRequired(true);
            builder.Property(x => x.IdUpdate).HasColumnName("id_update").IsRequired(true);
            builder.Property(x => x.Size).HasColumnName("size").IsRequired(true);
        }
    }
}
