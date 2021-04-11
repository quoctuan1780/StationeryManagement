using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class DropTableWard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Wards_WardCode",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropIndex(
                name: "IX_Users_WardCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WardCode",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WardCode",
                table: "Users",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    WardCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DistrictID = table.Column<int>(type: "int", nullable: false),
                    WardName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.WardCode);
                    table.ForeignKey(
                        name: "FK_Wards_Districts_DistrictID",
                        column: x => x.DistrictID,
                        principalTable: "Districts",
                        principalColumn: "DistrictID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_WardCode",
                table: "Users",
                column: "WardCode",
                unique: true,
                filter: "[WardCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_DistrictID",
                table: "Wards",
                column: "DistrictID");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_WardCode",
                table: "Wards",
                column: "WardCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Wards_WardCode",
                table: "Users",
                column: "WardCode",
                principalTable: "Wards",
                principalColumn: "WardCode",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
