using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddNewColumnInSaleDetailTableV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SaleProduct",
                table: "SaleProduct");

            migrationBuilder.AddColumn<int>(
                name: "SaleDetailId",
                table: "SaleProduct",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaleProduct",
                table: "SaleProduct",
                column: "SaleDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleProduct_SaleDetailId",
                table: "SaleProduct",
                column: "SaleDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SaleProduct",
                table: "SaleProduct");

            migrationBuilder.DropIndex(
                name: "IX_SaleProduct_SaleDetailId",
                table: "SaleProduct");

            migrationBuilder.DropColumn(
                name: "SaleDetailId",
                table: "SaleProduct");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SaleProduct",
                table: "SaleProduct",
                columns: new[] { "SaleId", "ProductId" });
        }
    }
}
