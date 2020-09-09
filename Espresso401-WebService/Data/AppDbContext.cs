using Espresso401_WebService.Models;
using Microsoft.EntityFrameworkCore;
using static Espresso401_WebService.Models.Enums;

namespace Espresso401_WebService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PlayerInParty>().HasKey(x => new { x.PlayerId, x.PartyId });
            modelBuilder.Entity<Party>().HasData(
                new Party
                {
                    Id = 1,
                    DungeonMasterId = 1,
                    MaxSize = int.MaxValue,
                    Full = false
                }
            );
            modelBuilder.Entity<DungeonMaster>().HasData(
                new DungeonMaster
                {
                    Id = 1,
                    UserId = "4c035675-9c5d-4763-aabe-6295555466b7",
                    CampaignName = "Campaign Sample",
                    CampaignDesc = "Deep in the dungeon's of Elderon, evil secrets stir. The world is once again thrust into peril, and our only hope is a small group of unlikely adventurers.",
                    ExperienceLevel = ExperienceLevel.Medium,
                    PersonalBio = "I'm just a test Dungeon Master, I don't actually exist :)",
                    ImageUrl = "https://geekandsundry.com/wp-content/uploads/2015/12/dsc.jpg"
                }
            );
            modelBuilder.Entity<Player>().HasData(
                new Player
                {
                    Id = 1,
                    UserId = "SeededPlayer",
                    ImageUrl = "https://i.pinimg.com/236x/06/5d/fa/065dfa0df7eda641ab45bdeafc09dd22.jpg",
                    CharacterName = "Grontosh The Pummeler",
                    Class = Class.Barbarian,
                    Race = Race.HalfOrc,
                    ExperienceLevel = ExperienceLevel.FirstTime,
                    GoodAlignment = 50,
                    LawAlignment = 50,
                    PartyId = 1
                }
            );
        }

        public DbSet<DungeonMaster> DungeonMasters { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<PlayerInParty> PlayerInParty { get; set; }
    }
}