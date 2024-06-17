using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcurementService.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Columns_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_requests_request_id",
                schema: "purchase",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_request_id",
                schema: "purchase",
                table: "products");

            migrationBuilder.DropColumn(
                name: "count",
                schema: "purchase",
                table: "products");

            migrationBuilder.DropColumn(
                name: "request_id",
                schema: "purchase",
                table: "products");

            migrationBuilder.CreateTable(
                name: " requests_products",
                schema: "purchase",
                columns: table => new
                {
                    request_id = table.Column<int>(type: "int", nullable: false),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ requests_products", x => new { x.request_id, x.product_id });
                    table.ForeignKey(
                        name: "FK_ requests_products_products_product_id",
                        column: x => x.product_id,
                        principalSchema: "purchase",
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ requests_products_requests_request_id",
                        column: x => x.request_id,
                        principalSchema: "purchase",
                        principalTable: "requests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ requests_products_product_id",
                schema: "purchase",
                table: " requests_products",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: " requests_products",
                schema: "purchase");

            migrationBuilder.AddColumn<int>(
                name: "count",
                schema: "purchase",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "request_id",
                schema: "purchase",
                table: "products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_request_id",
                schema: "purchase",
                table: "products",
                column: "request_id");

            migrationBuilder.AddForeignKey(
                name: "FK_products_requests_request_id",
                schema: "purchase",
                table: "products",
                column: "request_id",
                principalSchema: "purchase",
                principalTable: "requests",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
