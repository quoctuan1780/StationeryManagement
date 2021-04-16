using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddTablesAndCustomBillAndAddColumnProductDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Order_OrderId",
                table: "Bill");

            migrationBuilder.DropIndex(
                name: "IX_Bill_OrderId",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Bill");

            migrationBuilder.AddColumn<int>(
                name: "QuantityOrdered",
                table: "ProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RemainingQuantity",
                table: "ProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Bill",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BillDetail",
                columns: table => new
                {
                    BillId = table.Column<int>(type: "int", nullable: false),
                    ProductDetailId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDetail", x => new { x.BillId, x.ProductDetailId });
                    table.ForeignKey(
                        name: "FK_BillDetail_Bill_BillId",
                        column: x => x.BillId,
                        principalTable: "Bill",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillDetail_ProductDetails_ProductDetailId",
                        column: x => x.ProductDetailId,
                        principalTable: "ProductDetails",
                        principalColumn: "ProductDetailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoMoPayment",
                columns: table => new
                {
                    MoMoPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    MoMoOrderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PayType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoMoPayment", x => x.MoMoPaymentId);
                    table.ForeignKey(
                        name: "FK_MoMoPayment_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayPalPayment",
                columns: table => new
                {
                    PayPalPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LinkDetail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayPalPayment", x => x.PayPalPaymentId);
                    table.ForeignKey(
                        name: "FK_PayPalPayment_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bill_UserId",
                table: "Bill",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetail_BillId",
                table: "BillDetail",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetail_ProductDetailId",
                table: "BillDetail",
                column: "ProductDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_MoMoPayment_MoMoOrderId",
                table: "MoMoPayment",
                column: "MoMoOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MoMoPayment_MoMoPaymentId",
                table: "MoMoPayment",
                column: "MoMoPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_MoMoPayment_OrderId",
                table: "MoMoPayment",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayPalPayment_OrderId",
                table: "PayPalPayment",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayPalPayment_PayerId",
                table: "PayPalPayment",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PayPalPayment_PayPalPaymentId",
                table: "PayPalPayment",
                column: "PayPalPaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Users_UserId",
                table: "Bill",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Users_UserId",
                table: "Bill");

            migrationBuilder.DropTable(
                name: "BillDetail");

            migrationBuilder.DropTable(
                name: "MoMoPayment");

            migrationBuilder.DropTable(
                name: "PayPalPayment");

            migrationBuilder.DropIndex(
                name: "IX_Bill_UserId",
                table: "Bill");

            migrationBuilder.DropColumn(
                name: "QuantityOrdered",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "RemainingQuantity",
                table: "ProductDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bill");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Bill",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bill_OrderId",
                table: "Bill",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Order_OrderId",
                table: "Bill",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
