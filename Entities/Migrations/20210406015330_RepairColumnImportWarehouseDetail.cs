using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class RepairColumnImportWarehouseDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportWarehouseDetail_ImportWarehouse_ImportWarehouseDetailId",
                table: "ImportWarehouseDetail");

            migrationBuilder.RenameColumn(
                name: "ImportWarehouseDetailId",
                table: "ImportWarehouseDetail",
                newName: "ImportWarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_ImportWarehouseDetail_ImportWarehouseDetailId",
                table: "ImportWarehouseDetail",
                newName: "IX_ImportWarehouseDetail_ImportWarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportWarehouseDetail_ImportWarehouse_ImportWarehouseId",
                table: "ImportWarehouseDetail",
                column: "ImportWarehouseId",
                principalTable: "ImportWarehouse",
                principalColumn: "ImportWarehouseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportWarehouseDetail_ImportWarehouse_ImportWarehouseId",
                table: "ImportWarehouseDetail");

            migrationBuilder.RenameColumn(
                name: "ImportWarehouseId",
                table: "ImportWarehouseDetail",
                newName: "ImportWarehouseDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_ImportWarehouseDetail_ImportWarehouseId",
                table: "ImportWarehouseDetail",
                newName: "IX_ImportWarehouseDetail_ImportWarehouseDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportWarehouseDetail_ImportWarehouse_ImportWarehouseDetailId",
                table: "ImportWarehouseDetail",
                column: "ImportWarehouseDetailId",
                principalTable: "ImportWarehouse",
                principalColumn: "ImportWarehouseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
