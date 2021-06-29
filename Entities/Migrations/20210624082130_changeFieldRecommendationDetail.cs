using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class changeFieldRecommendationDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationDetail_ProductDetails_ProductDetailId",
                table: "RecommendationDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecommendationDetail",
                table: "RecommendationDetail");

            migrationBuilder.AlterColumn<int>(
                name: "ProductDetailId",
                table: "RecommendationDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RecommandationDetailId",
                table: "RecommendationDetail",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Input",
                table: "RecommendationDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Output",
                table: "RecommendationDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecommendationDetail",
                table: "RecommendationDetail",
                column: "RecommandationDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationDetail_ProductDetails_ProductDetailId",
                table: "RecommendationDetail",
                column: "ProductDetailId",
                principalTable: "ProductDetails",
                principalColumn: "ProductDetailId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationDetail_ProductDetails_ProductDetailId",
                table: "RecommendationDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecommendationDetail",
                table: "RecommendationDetail");

            migrationBuilder.DropColumn(
                name: "RecommandationDetailId",
                table: "RecommendationDetail");

            migrationBuilder.DropColumn(
                name: "Input",
                table: "RecommendationDetail");

            migrationBuilder.DropColumn(
                name: "Output",
                table: "RecommendationDetail");

            migrationBuilder.AlterColumn<int>(
                name: "ProductDetailId",
                table: "RecommendationDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecommendationDetail",
                table: "RecommendationDetail",
                columns: new[] { "ProductDetailId", "RecommendationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationDetail_ProductDetails_ProductDetailId",
                table: "RecommendationDetail",
                column: "ProductDetailId",
                principalTable: "ProductDetails",
                principalColumn: "ProductDetailId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
