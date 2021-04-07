using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class RepairRelationShipWardUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_WardCode",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_WardCode",
                table: "Users",
                column: "WardCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_WardCode",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_WardCode",
                table: "Users",
                column: "WardCode",
                unique: true,
                filter: "[WardCode] IS NOT NULL");
        }
    }
}
