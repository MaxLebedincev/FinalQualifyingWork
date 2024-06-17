using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcurementService.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Columns_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "summary_main",
                schema: "purchase",
                table: "requests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "summary_sub",
                schema: "purchase",
                table: "requests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "summary_main",
                schema: "purchase",
                table: "requests");

            migrationBuilder.DropColumn(
                name: "summary_sub",
                schema: "purchase",
                table: "requests");
        }
    }
}
