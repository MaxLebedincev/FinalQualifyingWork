using Microsoft.EntityFrameworkCore.Migrations;
using ProcurementService.API.Tools;

#nullable disable

namespace ProcurementService.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Fill_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("roles", "name", new[] { "admin", "signatory", "applicant" }, "security");

            var dateGlobalAdmin = DateTime.Now;
            var hashGlobalAdmin = "1111";
            migrationBuilder.InsertData
            (
                "users", 
                new[] 
                { 
                    "login", 
                    "email", 
                    "hash", 
                    "created_at",
                    "updated_at" 
                },
                new[] 
                { 
                    "GlobalAdmin", 
                    "GlobalAdmin@mail.ru", 
                    Security.GetHash($"{dateGlobalAdmin}{hashGlobalAdmin}{dateGlobalAdmin}"), 
                    $"{dateGlobalAdmin}", 
                    $"{dateGlobalAdmin}" 
                },
                "security"
            );

            migrationBuilder.InsertData("users_roles", new[] { "user_id", "role_id" }, new[] { "1", "1" }, "security");
            migrationBuilder.InsertData("users_roles", new[] { "user_id", "role_id" }, new[] { "1", "2" }, "security");
            migrationBuilder.InsertData("users_roles", new[] { "user_id", "role_id" }, new[] { "1", "3" }, "security");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("users_roles", "security");
            migrationBuilder.DropTable("users", "security");
            migrationBuilder.DropTable("roles", "security");
        }
    }
}
