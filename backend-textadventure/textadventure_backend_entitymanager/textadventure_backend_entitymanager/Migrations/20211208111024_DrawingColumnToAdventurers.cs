using Microsoft.EntityFrameworkCore.Migrations;

namespace textadventure_backend_entitymanager.Migrations
{
    public partial class DrawingColumnToAdventurers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NPCs_Items",
                table: "NPCs");

            migrationBuilder.DropForeignKey(
                name: "FK_NPCs_Weapons",
                table: "NPCs");

            migrationBuilder.DropIndex(
                name: "IX_NPCs_ItemId",
                table: "NPCs");

            migrationBuilder.DropIndex(
                name: "IX_NPCs_WeaponId",
                table: "NPCs");

            migrationBuilder.AddColumn<string>(
                name: "drawing",
                table: "Adventurers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "drawing",
                table: "Adventurers");

            migrationBuilder.CreateIndex(
                name: "IX_NPCs_ItemId",
                table: "NPCs",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCs_WeaponId",
                table: "NPCs",
                column: "WeaponId");

            migrationBuilder.AddForeignKey(
                name: "FK_NPCs_Items",
                table: "NPCs",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NPCs_Weapons",
                table: "NPCs",
                column: "WeaponId",
                principalTable: "Weapons",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
