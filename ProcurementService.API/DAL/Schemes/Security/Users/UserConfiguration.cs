using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProcurementService.API.DAL.Schemes.Security.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users", "security");

            builder.HasKey(x => x.Id).HasName("id").HasName("UsersPrimaryKey");

            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Login).HasColumnName("login").HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.Hash).HasColumnName("hash").IsRequired(true);
            builder.Property(x => x.CreatedAt).HasColumnName("created_at").ValueGeneratedOnAdd().IsRequired(true);
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").ValueGeneratedOnUpdate().IsRequired(true);
        }
    }
}
