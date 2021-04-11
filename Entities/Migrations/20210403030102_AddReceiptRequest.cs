using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddReceiptRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ImportWarehouseDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceiptRequestId",
                table: "ImportWarehouse",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ReceiptRequest",
                columns: table => new
                {
                    ReceiptRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptRequest", x => x.ReceiptRequestId);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptRequestDetail",
                columns: table => new
                {
                    ReceiptRequestId = table.Column<int>(type: "int", nullable: false),
                    ProductDetailId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptRequestDetail", x => new { x.ProductDetailId, x.ReceiptRequestId });
                    table.ForeignKey(
                        name: "FK_ReceiptRequestDetail_ProductDetails_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalTable: "ProductDetails",
                        principalColumn: "ProductDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptRequestDetail_ReceiptRequest_ReceiptRequestId",
                        column: x => x.ReceiptRequestId,
                        principalTable: "ReceiptRequest",
                        principalColumn: "ReceiptRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportWarehouse_ReceiptRequestId",
                table: "ImportWarehouse",
                column: "ReceiptRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptRequest_ReceiptRequestId",
                table: "ReceiptRequest",
                column: "ReceiptRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptRequestDetail_ProductDetailId",
                table: "ReceiptRequestDetail",
                column: "ProductDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptRequestDetail_ReceiptRequestId",
                table: "ReceiptRequestDetail",
                column: "ReceiptRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportWarehouse_ReceiptRequest_ReceiptRequestId",
                table: "ImportWarehouse",
                column: "ReceiptRequestId",
                principalTable: "ReceiptRequest",
                principalColumn: "ReceiptRequestId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportWarehouseDetail_ImportWarehouse_ImportWarehouseDetailId",
                table: "ImportWarehouseDetail",
                column: "ImportWarehouseDetailId",
                principalTable: "ImportWarehouse",
                principalColumn: "ImportWarehouseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportWarehouse_ReceiptRequest_ReceiptRequestId",
                table: "ImportWarehouse");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportWarehouseDetail_ImportWarehouse_ImportWarehouseDetailId",
                table: "ImportWarehouseDetail");

            migrationBuilder.DropTable(
                name: "ReceiptRequestDetail");

            migrationBuilder.DropTable(
                name: "ReceiptRequest");

            migrationBuilder.DropIndex(
                name: "IX_ImportWarehouse_ReceiptRequestId",
                table: "ImportWarehouse");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ImportWarehouseDetail");

            migrationBuilder.DropColumn(
                name: "ReceiptRequestId",
                table: "ImportWarehouse");

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
    }
}
