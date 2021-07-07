using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class UpdateColumnNameInTableSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SateStartDate",
                table: "Sale",
                newName: "SaleStartDate");

            migrationBuilder.RenameColumn(
                name: "SateEndDate",
                table: "Sale",
                newName: "SaleEndDate");

            migrationBuilder.AddColumn<decimal>(
                name: "FromOrderPrice",
                table: "Sale",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromOrderPrice",
                table: "Sale");

            migrationBuilder.RenameColumn(
                name: "SaleStartDate",
                table: "Sale",
                newName: "SateStartDate");

            migrationBuilder.RenameColumn(
                name: "SaleEndDate",
                table: "Sale",
                newName: "SateEndDate");
        }
    }
}
