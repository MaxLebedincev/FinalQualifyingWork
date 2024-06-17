using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcurementService.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "type",
                schema: "purchase",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                schema: "purchase",
                table: "products");
        }
    }
}
