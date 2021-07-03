using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddNewColumnToSaleTableV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sale",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Sale",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleType",
                table: "Sale",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SateEndDate",
                table: "Sale",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SateStartDate",
                table: "Sale",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "StatusSale",
                table: "Sale",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "SaleType",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "SateEndDate",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "SateStartDate",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "StatusSale",
                table: "Sale");
        }
    }
}
