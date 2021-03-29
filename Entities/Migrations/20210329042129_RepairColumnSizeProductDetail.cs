using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class RepairColumnSizeProductDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "ProductDetails");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "ProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "ProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "ProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "ProductDetails");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ProductDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
