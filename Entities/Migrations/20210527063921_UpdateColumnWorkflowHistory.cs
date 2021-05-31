using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class UpdateColumnWorkflowHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkflowHostoryId",
                table: "WorkflowHistory",
                newName: "WorkflowHistoryId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowHistory_WorkflowHostoryId",
                table: "WorkflowHistory",
                newName: "IX_WorkflowHistory_WorkflowHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkflowHistoryId",
                table: "WorkflowHistory",
                newName: "WorkflowHostoryId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowHistory_WorkflowHistoryId",
                table: "WorkflowHistory",
                newName: "IX_WorkflowHistory_WorkflowHostoryId");
        }
    }
}
