using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Entities.Migrations
{
    public partial class AddNewFieldInOrderTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Users_UserId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_UserId",
                table: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "Order",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ShipperId",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ShipperPickOrderDate",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "WarehouseId",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ModifiedBy",
                table: "Order",
                column: "ModifiedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Users_ModifiedBy",
                table: "Order",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Users_ModifiedBy",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ModifiedBy",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ReceivedDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShipperId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShipperPickOrderDate",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "Order");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Order",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Users_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
