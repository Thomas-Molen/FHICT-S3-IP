using Microsoft.EntityFrameworkCore.Migrations;

namespace textadventure_backend_entitymanager.Migrations
{
    public partial class MadeAllRelationsCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdventurerMaps_Adventurers",
                table: "AdventurerMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_AdventurerMaps_Rooms",
                table: "AdventurerMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_Adventurers_Dungeons",
                table: "Adventurers");

            migrationBuilder.DropForeignKey(
                name: "FK_Adventurers_Rooms",
                table: "Adventurers");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Adventurers",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Dungeons",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Adventurers",
                table: "Weapons");

            migrationBuilder.AddForeignKey(
                name: "FK_AdventurerMaps_Adventurers",
                table: "AdventurerMaps",
                column: "AdventurerId",
                principalTable: "Adventurers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AdventurerMaps_Rooms",
                table: "AdventurerMaps",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Adventurers_Dungeons",
                table: "Adventurers",
                column: "DungeonId",
                principalTable: "Dungeons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Adventurers_Rooms",
                table: "Adventurers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Adventurers",
                table: "Items",
                column: "AdventurerId",
                principalTable: "Adventurers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Dungeons",
                table: "Rooms",
                column: "DungeonId",
                principalTable: "Dungeons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Adventurers",
                table: "Weapons",
                column: "AdventurerId",
                principalTable: "Adventurers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdventurerMaps_Adventurers",
                table: "AdventurerMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_AdventurerMaps_Rooms",
                table: "AdventurerMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_Adventurers_Dungeons",
                table: "Adventurers");

            migrationBuilder.DropForeignKey(
                name: "FK_Adventurers_Rooms",
                table: "Adventurers");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_Adventurers",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Dungeons",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Adventurers",
                table: "Weapons");

            migrationBuilder.AddForeignKey(
                name: "FK_AdventurerMaps_Adventurers",
                table: "AdventurerMaps",
                column: "AdventurerId",
                principalTable: "Adventurers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AdventurerMaps_Rooms",
                table: "AdventurerMaps",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Adventurers_Dungeons",
                table: "Adventurers",
                column: "DungeonId",
                principalTable: "Dungeons",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Adventurers_Rooms",
                table: "Adventurers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Adventurers",
                table: "Items",
                column: "AdventurerId",
                principalTable: "Adventurers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Dungeons",
                table: "Rooms",
                column: "DungeonId",
                principalTable: "Dungeons",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Weapons_Adventurers",
                table: "Weapons",
                column: "AdventurerId",
                principalTable: "Adventurers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
