using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class RepairRelationShipImportWarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ImportWarehouse",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ImportWarehouse",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ImportWarehouse");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ImportWarehouse");
        }
    }
}
