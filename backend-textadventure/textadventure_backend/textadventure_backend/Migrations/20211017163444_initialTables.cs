using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace textadventure_backend.Migrations
{
    public partial class initialTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dungeons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dungeons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    username = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    admin = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    password = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    token = table.Column<string>(type: "text", nullable: false),
                    expires_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "datetime", nullable: true),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdventurerMaps",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EventCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AdventurerId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdventurerMaps", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    content = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    AdventurerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    attack = table.Column<int>(type: "int", nullable: false),
                    durability = table.Column<int>(type: "int", maxLength: 101, nullable: false),
                    AdventurerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "NPCs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    conversation = table.Column<string>(type: "text", maxLength: 5000, nullable: false),
                    risk = table.Column<int>(type: "int", maxLength: 101, nullable: false),
                    WeaponId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NPCs", x => x.id);
                    table.ForeignKey(
                        name: "FK_NPCs_Items",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NPCs_Weapons",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Interactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    NPCId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Interactions_NPCs",
                        column: x => x.NPCId,
                        principalTable: "NPCs",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DungeonId = table.Column<int>(type: "int", nullable: false),
                    position_x = table.Column<int>(type: "int", nullable: false),
                    position_y = table.Column<int>(type: "int", nullable: false),
                    @event = table.Column<string>(name: "event", type: "varchar(100)", maxLength: 100, nullable: false),
                    NorthInteractionId = table.Column<int>(type: "int", nullable: false),
                    EastInteractionId = table.Column<int>(type: "int", nullable: false),
                    SouthInteractionId = table.Column<int>(type: "int", nullable: false),
                    WestInteractionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.id);
                    table.ForeignKey(
                        name: "FK_Rooms_Dungeons",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rooms_EastInteractions",
                        column: x => x.EastInteractionId,
                        principalTable: "Interactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rooms_NorthInteractions",
                        column: x => x.NorthInteractionId,
                        principalTable: "Interactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rooms_SouthInteractions",
                        column: x => x.SouthInteractionId,
                        principalTable: "Interactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rooms_WestInteractions",
                        column: x => x.WestInteractionId,
                        principalTable: "Interactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Adventurers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    experience = table.Column<int>(type: "int", nullable: false),
                    health = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DungeonId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adventurers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Adventurers_Dungeons",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Adventurers_Rooms",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Adventurers_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_Interactions_NPCId",
                table: "Interactions",
                column: "NPCId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_AdventurerId",
                table: "Items",
                column: "AdventurerId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCs_ItemId",
                table: "NPCs",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_NPCs_WeaponId",
                table: "NPCs",
                column: "WeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DungeonId",
                table: "Rooms",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_EastInteractionId",
                table: "Rooms",
                column: "EastInteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_NorthInteractionId",
                table: "Rooms",
                column: "NorthInteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_SouthInteractionId",
                table: "Rooms",
                column: "SouthInteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_WestInteractionId",
                table: "Rooms",
                column: "WestInteractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_AdventurerId",
                table: "Weapons",
                column: "AdventurerId");

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
                name: "FK_Items_Adventurers",
                table: "Items",
                column: "AdventurerId",
                principalTable: "Adventurers",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Adventurers",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Weapons_Adventurers",
                table: "Weapons");

            migrationBuilder.DropTable(
                name: "AdventurerMaps");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Adventurers");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Dungeons");

            migrationBuilder.DropTable(
                name: "Interactions");

            migrationBuilder.DropTable(
                name: "NPCs");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Weapons");
        }
    }
}
