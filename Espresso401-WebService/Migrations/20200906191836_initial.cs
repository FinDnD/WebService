using Microsoft.EntityFrameworkCore.Migrations;

namespace Espresso401_WebService.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DungeonMasters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    CampaignName = table.Column<string>(nullable: true),
                    CampaignDesc = table.Column<string>(nullable: true),
                    ExperienceLevel = table.Column<int>(nullable: false),
                    PersonalBio = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DungeonMasters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DungeonMasterId = table.Column<int>(nullable: false),
                    MaxSize = table.Column<int>(nullable: false),
                    Full = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parties_DungeonMasters_DungeonMasterId",
                        column: x => x.DungeonMasterId,
                        principalTable: "DungeonMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    CharacterName = table.Column<string>(nullable: true),
                    Class = table.Column<int>(nullable: false),
                    Race = table.Column<int>(nullable: false),
                    ExperienceLevel = table.Column<int>(nullable: false),
                    GoodAlignment = table.Column<int>(nullable: false),
                    LawAlignment = table.Column<int>(nullable: false),
                    PartyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlayerInParty",
                columns: table => new
                {
                    PartyId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerInParty", x => new { x.PlayerId, x.PartyId });
                    table.ForeignKey(
                        name: "FK_PlayerInParty_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerInParty_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(nullable: false),
                    DungeonMasterId = table.Column<int>(nullable: false),
                    PlayerAccepted = table.Column<bool>(nullable: false),
                    DungeonMasterAccepted = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_DungeonMasters_DungeonMasterId",
                        column: x => x.DungeonMasterId,
                        principalTable: "DungeonMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DungeonMasters",
                columns: new[] { "Id", "CampaignDesc", "CampaignName", "ExperienceLevel", "PersonalBio", "UserId" },
                values: new object[] { 1, "Deep in the dungeon's of Elderon, evil secrets stir. The world is once again thrust into peril, and our only hope is a small group of unlikely adventurers.", "Campaign Sample", 2, "I'm just a test Dungeon Master, I don't actually exist :)", "SeededDM" });

            migrationBuilder.InsertData(
                table: "Parties",
                columns: new[] { "Id", "DungeonMasterId", "Full", "MaxSize" },
                values: new object[] { 1, 1, false, 2147483647 });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CharacterName", "Class", "ExperienceLevel", "GoodAlignment", "ImageUrl", "LawAlignment", "PartyId", "Race", "UserId" },
                values: new object[] { 1, "Grontosh The Pummeler", 0, 0, 50, "https://i.pinimg.com/236x/06/5d/fa/065dfa0df7eda641ab45bdeafc09dd22.jpg", 50, 1, 6, "SeededPlayer" });

            migrationBuilder.CreateIndex(
                name: "IX_Parties_DungeonMasterId",
                table: "Parties",
                column: "DungeonMasterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerInParty_PartyId",
                table: "PlayerInParty",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_PartyId",
                table: "Players",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_DungeonMasterId",
                table: "Requests",
                column: "DungeonMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_PlayerId",
                table: "Requests",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerInParty");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "DungeonMasters");
        }
    }
}