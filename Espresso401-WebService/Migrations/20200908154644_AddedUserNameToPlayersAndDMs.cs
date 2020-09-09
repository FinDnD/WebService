using Microsoft.EntityFrameworkCore.Migrations;

namespace Espresso401_WebService.Migrations
{
    public partial class AddedUserNameToPlayersAndDMs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "DungeonMasters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "DungeonMasters");
        }
    }
}
