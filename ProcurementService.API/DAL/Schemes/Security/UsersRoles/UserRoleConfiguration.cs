using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProcurementService.API.DAL.Schemes.Security.UsersRoles
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            {
                builder.ToTable("users_roles", "security");

                builder
                    .HasKey(ur => new { ur.UserId, ur.RoleId });

                builder
                    .HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(u => u.UserId);

                builder
                    .HasOne(ur => ur.Role)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(u => u.RoleId);

                builder.Property(x => x.UserId).HasColumnName("user_id").IsRequired(true);
                builder.Property(x => x.RoleId).HasColumnName("role_id").IsRequired(true);
            }
        }
    }
}
