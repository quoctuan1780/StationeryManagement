using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class receiptRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationDetail_Product_ProductId",
                table: "RecommendationDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecommendationDetail",
                table: "RecommendationDetail");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "RecommendationDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductDetailId",
                table: "RecommendationDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ReceiptRequest",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecommendationDetail",
                table: "RecommendationDetail",
                columns: new[] { "ProductDetailId", "RecommendationId" });

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationDetail_ProductDetailId",
                table: "RecommendationDetail",
                column: "ProductDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptRequest_UserId",
                table: "ReceiptRequest",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptRequest_Users_UserId",
                table: "ReceiptRequest",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationDetail_Product_ProductId",
                table: "RecommendationDetail",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationDetail_ProductDetails_ProductDetailId",
                table: "RecommendationDetail",
                column: "ProductDetailId",
                principalTable: "ProductDetails",
                principalColumn: "ProductDetailId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptRequest_Users_UserId",
                table: "ReceiptRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationDetail_Product_ProductId",
                table: "RecommendationDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationDetail_ProductDetails_ProductDetailId",
                table: "RecommendationDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecommendationDetail",
                table: "RecommendationDetail");

            migrationBuilder.DropIndex(
                name: "IX_RecommendationDetail_ProductDetailId",
                table: "RecommendationDetail");

            migrationBuilder.DropIndex(
                name: "IX_ReceiptRequest_UserId",
                table: "ReceiptRequest");

            migrationBuilder.DropColumn(
                name: "ProductDetailId",
                table: "RecommendationDetail");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "RecommendationDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ReceiptRequest",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecommendationDetail",
                table: "RecommendationDetail",
                columns: new[] { "ProductId", "RecommendationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationDetail_Product_ProductId",
                table: "RecommendationDetail",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
