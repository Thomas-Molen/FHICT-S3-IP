using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace textadventure_backend.Migrations
{
    public partial class ChangedRoomsColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PositionY",
                table: "Rooms",
                newName: "position_x");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RevokedAt",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "position_x",
                table: "Rooms",
                newName: "PositionY");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RevokedAt",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
