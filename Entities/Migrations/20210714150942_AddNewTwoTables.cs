using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddNewTwoTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserConnection",
                columns: table => new
                {
                    UserConnectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstAccessedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastAccessedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConnection", x => x.UserConnectionId);
                });

            migrationBuilder.CreateTable(
                name: "UserConnectionDetail",
                columns: table => new
                {
                    UserConnectionDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserConnectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductViewed = table.Column<int>(type: "int", nullable: false),
                    DateViewed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConnectionDetail", x => x.UserConnectionDetailId);
                    table.ForeignKey(
                        name: "FK_UserConnectionDetail_UserConnection_UserConnectionId",
                        column: x => x.UserConnectionId,
                        principalTable: "UserConnection",
                        principalColumn: "UserConnectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserConnection_UserConnectionId",
                table: "UserConnection",
                column: "UserConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnectionDetail_UserConnectionDetailId",
                table: "UserConnectionDetail",
                column: "UserConnectionDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_UserConnectionDetail_UserConnectionId",
                table: "UserConnectionDetail",
                column: "UserConnectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserConnectionDetail");

            migrationBuilder.DropTable(
                name: "UserConnection");
        }
    }
}
