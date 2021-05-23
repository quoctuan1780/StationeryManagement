using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddNewColumnInCommentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ProductId",
                table: "Comment",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Product_ProductId",
                table: "Comment",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Product_ProductId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ProductId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Comment");
        }
    }
}
