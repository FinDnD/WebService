using Microsoft.EntityFrameworkCore.Migrations;

namespace Espresso401_WebService.Migrations
{
    public partial class AddImagesToDms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "DungeonMasters",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DungeonMasters",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImageUrl",
                value: "https://geekandsundry.com/wp-content/uploads/2015/12/dsc.jpg");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "DungeonMasters");
        }
    }
}
