using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class RepairNameColumnOfAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Provinces_ProvinceID",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Wards_Districts_DistrictID",
                table: "Wards");

            migrationBuilder.RenameColumn(
                name: "DistrictID",
                table: "Wards",
                newName: "DistrictId");

            migrationBuilder.RenameIndex(
                name: "IX_Wards_DistrictID",
                table: "Wards",
                newName: "IX_Wards_DistrictId");

            migrationBuilder.RenameColumn(
                name: "ProvinceID",
                table: "Provinces",
                newName: "ProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_Provinces_ProvinceID",
                table: "Provinces",
                newName: "IX_Provinces_ProvinceId");

            migrationBuilder.RenameColumn(
                name: "ProvinceID",
                table: "Districts",
                newName: "ProvinceId");

            migrationBuilder.RenameColumn(
                name: "DistrictID",
                table: "Districts",
                newName: "DistrictId");

            migrationBuilder.RenameIndex(
                name: "IX_Districts_ProvinceID",
                table: "Districts",
                newName: "IX_Districts_ProvinceId");

            migrationBuilder.RenameIndex(
                name: "IX_Districts_DistrictID",
                table: "Districts",
                newName: "IX_Districts_DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Provinces_ProvinceId",
                table: "Districts",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "ProvinceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wards_Districts_DistrictId",
                table: "Wards",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "DistrictId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Districts_Provinces_ProvinceId",
                table: "Districts");

            migrationBuilder.DropForeignKey(
                name: "FK_Wards_Districts_DistrictId",
                table: "Wards");

            migrationBuilder.RenameColumn(
                name: "DistrictId",
                table: "Wards",
                newName: "DistrictID");

            migrationBuilder.RenameIndex(
                name: "IX_Wards_DistrictId",
                table: "Wards",
                newName: "IX_Wards_DistrictID");

            migrationBuilder.RenameColumn(
                name: "ProvinceId",
                table: "Provinces",
                newName: "ProvinceID");

            migrationBuilder.RenameIndex(
                name: "IX_Provinces_ProvinceId",
                table: "Provinces",
                newName: "IX_Provinces_ProvinceID");

            migrationBuilder.RenameColumn(
                name: "ProvinceId",
                table: "Districts",
                newName: "ProvinceID");

            migrationBuilder.RenameColumn(
                name: "DistrictId",
                table: "Districts",
                newName: "DistrictID");

            migrationBuilder.RenameIndex(
                name: "IX_Districts_ProvinceId",
                table: "Districts",
                newName: "IX_Districts_ProvinceID");

            migrationBuilder.RenameIndex(
                name: "IX_Districts_DistrictId",
                table: "Districts",
                newName: "IX_Districts_DistrictID");

            migrationBuilder.AddForeignKey(
                name: "FK_Districts_Provinces_ProvinceID",
                table: "Districts",
                column: "ProvinceID",
                principalTable: "Provinces",
                principalColumn: "ProvinceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wards_Districts_DistrictID",
                table: "Wards",
                column: "DistrictID",
                principalTable: "Districts",
                principalColumn: "DistrictID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
