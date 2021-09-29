using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace textadventure_backend.Migrations
{
    public partial class ChangedRefreshTokensColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevokedAt",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "RefreshTokens",
                newName: "created_at");

            migrationBuilder.AlterColumn<DateTime>(
                name: "revoked_at",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "expires_at",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expires_at",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "RefreshTokens",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "revoked_at",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedAt",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);
        }
    }
}
