using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddTableDeliveryAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryAddress",
                columns: table => new
                {
                    DeliveryAddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WardCode = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StreetName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAddress", x => x.DeliveryAddressId);
                    table.ForeignKey(
                        name: "FK_DeliveryAddress_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryAddress_Wards_WardCode",
                        column: x => x.WardCode,
                        principalTable: "Wards",
                        principalColumn: "WardCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddress_DeliveryAddressId",
                table: "DeliveryAddress",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddress_UserId",
                table: "DeliveryAddress",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAddress_WardCode",
                table: "DeliveryAddress",
                column: "WardCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryAddress");
        }
    }
}
