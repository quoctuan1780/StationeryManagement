using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddNewTableWorkflowHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "WorkflowHistory",
                columns: table => new
                {
                    WorkflowHostoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CurrentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowHistory", x => x.WorkflowHostoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowHistory_CreatedBy",
                table: "WorkflowHistory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowHistory_RecordId",
                table: "WorkflowHistory",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowHistory_WorkflowHostoryId",
                table: "WorkflowHistory",
                column: "WorkflowHostoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkflowHistory");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Order");
        }
    }
}
