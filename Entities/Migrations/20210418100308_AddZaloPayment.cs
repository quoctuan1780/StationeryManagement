using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddZaloPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZalopayOrders",
                columns: table => new
                {
                    Apptransid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Zptransid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Channel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZalopayOrders", x => x.Apptransid);
                    table.ForeignKey(
                        name: "FK_ZalopayOrders_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZalopayRefunds",
                columns: table => new
                {
                    Mrefundid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Zptransid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZalopayRefunds", x => x.Mrefundid);
                    table.ForeignKey(
                        name: "FK_ZalopayRefunds_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZalopayOrders_Apptransid",
                table: "ZalopayOrders",
                column: "Apptransid");

            migrationBuilder.CreateIndex(
                name: "IX_ZalopayOrders_OrderId",
                table: "ZalopayOrders",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZalopayRefunds_Mrefundid",
                table: "ZalopayRefunds",
                column: "Mrefundid");

            migrationBuilder.CreateIndex(
                name: "IX_ZalopayRefunds_OrderId",
                table: "ZalopayRefunds",
                column: "OrderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZalopayOrders");

            migrationBuilder.DropTable(
                name: "ZalopayRefunds");
        }
    }
}
