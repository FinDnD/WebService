using Espresso401_WebService.Models;
using Espresso401_WebService.Models.DTOs;
using Espresso401_WebService.Models.Interfaces;
using Espresso401_WebService.Models.Services;
using System.Threading.Tasks;
using Xunit;

namespace Espresso401_WebServiceTests
{
    public class PartyTests : DatabaseTest
    {
        private IParty BuildDb()
        {
            return new PartyRepository(_appDb);
        }

        private IDungeonMaster BuildDmDb()
        {
            return new DungeonMasterRepository(_appDb, _party, _request);
        }

        [Fact]
        public async Task CanSaveNewParty()
        {
            DungeonMasterDTO newDm = new DungeonMasterDTO()
            {
                UserId = "test1",
                CampaignName = "Test Campaign",
                CampaignDesc = "Test Campaign Description",
                ExperienceLevel = "Medium",
                PersonalBio = "Test Personal Bio"
            };
            var dmRepo = BuildDmDb();
            await dmRepo.CreateDungeonMaster(newDm);

            Party newParty = new Party()
            {
                DungeonMasterId = newDm.Id,
                MaxSize = 4,
                Full = false
            };
            var repo = BuildDb();
            var result = await repo.CreateParty(newParty);

            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(newParty.Id, result.Id);
            Assert.Equal(newParty.MaxSize, result.MaxSize);
        }

        [Fact]
        public async Task CanDeleteParty()
        {
            DungeonMasterDTO newDm = new DungeonMasterDTO()
            {
                UserId = "test1",
                CampaignName = "Test Campaign",
                CampaignDesc = "Test Campaign Description",
                ExperienceLevel = "Medium",
                PersonalBio = "Test Personal Bio"
            };
            var dmRepo = BuildDmDb();
            await dmRepo.CreateDungeonMaster(newDm);

            Party newParty = new Party()
            {
                DungeonMasterId = newDm.Id,
                MaxSize = 4,
                Full = false
            };
            var repo = BuildDb();
            var create = await repo.CreateParty(newParty);

            Assert.NotNull(create);

            await repo.DeleteParty(create.Id);

            var result = await repo.GetPartyById(create.Id);

            Assert.Null(result);
        }

        [Fact]
        public async Task CanGetAllParties()
        {
            DungeonMasterDTO newDm = new DungeonMasterDTO()
            {
                UserId = "test1",
                CampaignName = "Test Campaign",
                CampaignDesc = "Test Campaign Description",
                ExperienceLevel = "Medium",
                PersonalBio = "Test Personal Bio"
            };
            DungeonMasterDTO newDm2 = new DungeonMasterDTO()
            {
                UserId = "test2",
                CampaignName = "Test Campaign2",
                CampaignDesc = "Test Campaign Description2",
                ExperienceLevel = "High",
                PersonalBio = "Test Personal Bio2"
            };
            DungeonMasterDTO newDm3 = new DungeonMasterDTO()
            {
                UserId = "test3",
                CampaignName = "Test Campaign3",
                CampaignDesc = "Test Campaign Description3",
                ExperienceLevel = "FirstTime",
                PersonalBio = "Test Personal Bio3"
            };
            var dmRepo = BuildDmDb();
            await dmRepo.CreateDungeonMaster(newDm);
            await dmRepo.CreateDungeonMaster(newDm2);
            await dmRepo.CreateDungeonMaster(newDm3);

            Party newParty = new Party()
            {
                DungeonMasterId = newDm.Id,
                MaxSize = 4,
                Full = false
            };
            Party newParty2 = new Party()
            {
                DungeonMasterId = newDm2.Id,
                MaxSize = 133,
                Full = false
            };
            Party newParty3 = new Party()
            {
                DungeonMasterId = newDm3.Id,
                MaxSize = 8,
                Full = false
            };
            var repo = BuildDb();
            await repo.CreateParty(newParty);
            await repo.CreateParty(newParty2);
            await repo.CreateParty(newParty3);

            var result = await repo.GetAllParties();

            bool partyInList = false;
            foreach (var item in result)
            {
                if (item.MaxSize == 133) partyInList = true;
            }

            Assert.NotEmpty(result);
            Assert.True(partyInList);
        }

        [Fact]
        public async Task CanGetPartyByDmId()
        {
            DungeonMasterDTO newDm = new DungeonMasterDTO()
            {
                UserId = "test1",
                CampaignName = "Test Campaign",
                CampaignDesc = "Test Campaign Description",
                ExperienceLevel = "Medium",
                PersonalBio = "Test Personal Bio"
            };
            var dmRepo = BuildDmDb();
            await dmRepo.CreateDungeonMaster(newDm);

            Party newParty = new Party()
            {
                DungeonMasterId = newDm.Id,
                MaxSize = 4,
                Full = false
            };

            var repo = BuildDb();
            await repo.CreateParty(newParty);

            var result = await repo.GetPartyByDMId(newDm.Id);

            Assert.NotNull(result);
            Assert.Equal(4, result.MaxSize);
        }

        [Fact]
        public async Task CanGetPartyById()
        {
            DungeonMasterDTO newDm = new DungeonMasterDTO()
            {
                UserId = "test1",
                CampaignName = "Test Campaign",
                CampaignDesc = "Test Campaign Description",
                ExperienceLevel = "Medium",
                PersonalBio = "Test Personal Bio"
            };
            var dmRepo = BuildDmDb();
            await dmRepo.CreateDungeonMaster(newDm);

            Party newParty = new Party()
            {
                DungeonMasterId = newDm.Id,
                MaxSize = 45,
                Full = false
            };

            var repo = BuildDb();
            await repo.CreateParty(newParty);

            var result = await repo.GetPartyByDMId(newParty.Id);

            Assert.NotNull(result);
            Assert.Equal(45, result.MaxSize);
        }


        [Fact]
        public async Task CanUpdateParty()
        {
            var repo = BuildDb();
            Party newPartyUpdate = new Party()
            {
                Id = 1,
                DungeonMasterId = 1,
                MaxSize = 93,
                Full = false
            };

            await repo.UpdateParty(newPartyUpdate);

            var result = await repo.GetPartyById(1);

            Assert.NotNull(result);
            Assert.Equal(93, result.MaxSize);
        }
    }
}