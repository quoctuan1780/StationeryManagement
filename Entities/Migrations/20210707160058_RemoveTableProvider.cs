using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class RemoveTableProvider : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportWarehouseDetail_Provider_ProviderId",
                table: "ImportWarehouseDetail");

            migrationBuilder.DropTable(
                name: "Provider");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImportWarehouseDetail",
                table: "ImportWarehouseDetail");

            migrationBuilder.DropIndex(
                name: "IX_ImportWarehouseDetail_ProviderId",
                table: "ImportWarehouseDetail");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "ImportWarehouseDetail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImportWarehouseDetail",
                table: "ImportWarehouseDetail",
                columns: new[] { "ProductDetailId", "ImportWarehouseId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ImportWarehouseDetail",
                table: "ImportWarehouseDetail");

            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "ImportWarehouseDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImportWarehouseDetail",
                table: "ImportWarehouseDetail",
                columns: new[] { "ProductDetailId", "ImportWarehouseId", "ProviderId" });

            migrationBuilder.CreateTable(
                name: "Provider",
                columns: table => new
                {
                    ProviderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.ProviderId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportWarehouseDetail_ProviderId",
                table: "ImportWarehouseDetail",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Provider_ProviderId",
                table: "Provider",
                column: "ProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportWarehouseDetail_Provider_ProviderId",
                table: "ImportWarehouseDetail",
                column: "ProviderId",
                principalTable: "Provider",
                principalColumn: "ProviderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
