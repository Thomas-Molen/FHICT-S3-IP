using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace textadventure_backend_entitymanager.Migrations
{
    public partial class ChangedDirectionalInteractionsInRoomsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_EastInteractions",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_NorthInteractions",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_SouthInteractions",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_WestInteractions",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "Interactions");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_EastInteractionId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_NorthInteractionId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_SouthInteractionId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_WestInteractionId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "EastInteractionId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NorthInteractionId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "SouthInteractionId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "WestInteractionId",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "east",
                table: "Rooms",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "north",
                table: "Rooms",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "south",
                table: "Rooms",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "west",
                table: "Rooms",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "east",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "north",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "south",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "west",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "EastInteractionId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NorthInteractionId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SouthInteractionId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WestInteractionId",
                table: "Rooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Interactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NPCId = table.Column<int>(type: "int", nullable: true),
                    type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
                name: "IX_Interactions_NPCId",
                table: "Interactions",
                column: "NPCId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_EastInteractions",
                table: "Rooms",
                column: "EastInteractionId",
                principalTable: "Interactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_NorthInteractions",
                table: "Rooms",
                column: "NorthInteractionId",
                principalTable: "Interactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_SouthInteractions",
                table: "Rooms",
                column: "SouthInteractionId",
                principalTable: "Interactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_WestInteractions",
                table: "Rooms",
                column: "WestInteractionId",
                principalTable: "Interactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
