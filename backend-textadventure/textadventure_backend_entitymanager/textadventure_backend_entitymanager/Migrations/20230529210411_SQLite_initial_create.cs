using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace textadventure_backend_entitymanager.Migrations
{
    public partial class SQLite_initial_create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dungeons",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dungeons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "NPCs",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    conversation = table.Column<string>(type: "TEXT", maxLength: 5000, nullable: false),
                    risk = table.Column<int>(type: "INTEGER", maxLength: 101, nullable: false),
                    WeaponId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    username = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    admin = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    password = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DungeonId = table.Column<int>(type: "INTEGER", nullable: false),
                    position_x = table.Column<int>(type: "INTEGER", nullable: false),
                    position_y = table.Column<int>(type: "INTEGER", nullable: false),
                    @event = table.Column<string>(name: "event", type: "TEXT", maxLength: 100, nullable: false),
                    north = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    east = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    south = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    west = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.id);
                    table.ForeignKey(
                        name: "FK_Rooms_Dungeons",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    token = table.Column<string>(type: "TEXT", nullable: false),
                    expires_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    active = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Adventurers",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    experience = table.Column<int>(type: "INTEGER", nullable: false),
                    health = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    drawing = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    DungeonId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adventurers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Adventurers_Dungeons",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Adventurers_Rooms",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Adventurers_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AdventurerMaps",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    AdventurerId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoomId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdventurerMaps", x => x.id);
                    table.ForeignKey(
                        name: "FK_AdventurerMaps_Adventurers",
                        column: x => x.AdventurerId,
                        principalTable: "Adventurers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdventurerMaps_Rooms",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    content = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AdventurerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.id);
                    table.ForeignKey(
                        name: "FK_Items_Adventurers",
                        column: x => x.AdventurerId,
                        principalTable: "Adventurers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    attack = table.Column<int>(type: "INTEGER", nullable: false),
                    durability = table.Column<int>(type: "INTEGER", maxLength: 101, nullable: false),
                    AdventurerId = table.Column<int>(type: "INTEGER", nullable: false),
                    equiped = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.id);
                    table.ForeignKey(
                        name: "FK_Weapons_Adventurers",
                        column: x => x.AdventurerId,
                        principalTable: "Adventurers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdventurerMaps_AdventurerId",
                table: "AdventurerMaps",
                column: "AdventurerId");

            migrationBuilder.CreateIndex(
                name: "IX_AdventurerMaps_RoomId",
                table: "AdventurerMaps",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Adventurers_DungeonId",
                table: "Adventurers",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_Adventurers_RoomId",
                table: "Adventurers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Adventurers_UserId",
                table: "Adventurers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_AdventurerId",
                table: "Items",
                column: "AdventurerId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DungeonId",
                table: "Rooms",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_AdventurerId",
                table: "Weapons",
                column: "AdventurerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdventurerMaps");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "NPCs");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "Adventurers");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Dungeons");
        }
    }
}
