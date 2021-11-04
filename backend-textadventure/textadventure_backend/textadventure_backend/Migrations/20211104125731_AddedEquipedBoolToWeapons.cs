using Microsoft.EntityFrameworkCore.Migrations;

namespace textadventure_backend.Migrations
{
    public partial class AddedEquipedBoolToWeapons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "equiped",
                table: "Weapons",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "equiped",
                table: "Weapons");
        }
    }
}
