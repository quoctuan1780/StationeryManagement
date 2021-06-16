using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddNewColumnInWorkFlowHistoryTableV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "WorkflowHistory",
                type: "nvarchar(max)",
                defaultValue: "Đặt hàng",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserRole",
                table: "WorkflowHistory",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "WorkflowHistory");
        }
    }
}
