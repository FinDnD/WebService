using Microsoft.EntityFrameworkCore.Migrations;

namespace Espresso401_WebService.Migrations
{
    public partial class AddedUserEmailsToProfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "DungeonMasters",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DungeonMasters",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: "4c035675-9c5d-4763-aabe-6295555466b7");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "DungeonMasters");

            migrationBuilder.UpdateData(
                table: "DungeonMasters",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserId",
                value: "SeededDM");
        }
    }
}
