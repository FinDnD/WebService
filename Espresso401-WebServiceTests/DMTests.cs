using Espresso401_WebService.Models;
using Espresso401_WebService.Models.DTOs;
using Espresso401_WebService.Models.Interfaces;
using Espresso401_WebService.Models.Services;
using System.Threading.Tasks;
using Xunit;
using static Espresso401_WebService.Models.Enums;

namespace Espresso401_WebServiceTests
{
    public class DMTests : DatabaseTest
    {
        private IDungeonMaster BuildDb()
        {
            return new DungeonMasterRepository(_appDb, _party, _request);
        }

        [Fact]
        public async Task CanSaveNewDM()
        {
            DungeonMasterDTO newDm = new DungeonMasterDTO()
            {
                UserId = "test1",
                CampaignName = "Test Campaign",
                CampaignDesc = "Test Campaign Description",
                ExperienceLevel = "Medium",
                PersonalBio = "Test Personal Bio"
            };
            var repo = BuildDb();
            var result = await repo.CreateDungeonMaster(newDm);

            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(newDm.Id, result.Id);
            Assert.Equal(newDm.CampaignName, result.CampaignName);
        }

        [Fact]
        public async Task CanGetDMById()
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
            var repo = BuildDb();
            await repo.CreateDungeonMaster(newDm);
            await repo.CreateDungeonMaster(newDm2);
            await repo.CreateDungeonMaster(newDm3);

            var result = await repo.GetDungeonMasterById(newDm2.Id);

            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(newDm2.Id, result.Id);
            Assert.Equal(newDm2.CampaignName, result.CampaignName);
            Assert.Equal(newDm2.CampaignDesc, result.CampaignDesc);
        }

        [Fact]
        public async Task CanGetDMByUserId()
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
            var repo = BuildDb();
            await repo.CreateDungeonMaster(newDm);
            await repo.CreateDungeonMaster(newDm2);
            await repo.CreateDungeonMaster(newDm3);

            var result = await repo.GetDungeonMasterByUserId("test2");

            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(newDm2.Id, result.Id);
            Assert.Equal(newDm2.CampaignName, result.CampaignName);
            Assert.Equal(newDm2.CampaignDesc, result.CampaignDesc);
        }

        [Fact]
        public async Task CanGetAllDMs()
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
            var repo = BuildDb();
            await repo.CreateDungeonMaster(newDm);
            await repo.CreateDungeonMaster(newDm2);
            await repo.CreateDungeonMaster(newDm3);

            var result = await repo.GetAllDungeonMasters();

            bool dmInList = false;
            foreach (var item in result)
            {
                if (item.UserId == "test2") dmInList = true;
            }

            Assert.NotEmpty(result);
            Assert.True(dmInList);
        }

        [Fact]
        public async Task CanUpdateDM()
        {
            var repo = BuildDb();

            DungeonMasterDTO dmUpdate = new DungeonMasterDTO()
            {
                Id = 1,
                UserId = "test1Update",
                CampaignName = "Test Campaign Update",
                CampaignDesc = "Test Campaign Description Update",
                ExperienceLevel = "Low",
                PersonalBio = "Test Personal Bio Update"
            };

            var update = await repo.UpdateDungeonMaster(dmUpdate);

            Assert.NotNull(update);
            Assert.Equal(1, update.Id);
            Assert.Equal("test1Update", update.UserId);
            Assert.Equal("Test Campaign Description Update", update.CampaignDesc);
        }

        [Fact]
        public async Task CanDeleteDM()
        {
            DungeonMasterDTO newDm = new DungeonMasterDTO()
            {
                UserId = "test1",
                CampaignName = "Test Campaign",
                CampaignDesc = "Test Campaign Description",
                ExperienceLevel = "Medium",
                PersonalBio = "Test Personal Bio"
            };
            var repo = BuildDb();
            var create = await repo.CreateDungeonMaster(newDm);

            Assert.NotNull(create);

            await repo.DeleteDungeonMaster(newDm.Id);

            var result = await repo.GetDungeonMasterByUserId("test1");

            Assert.Null(result);
        }

        [Fact]
        public async Task CanBuildDTO()
        {
            var repo = BuildDb();
            DungeonMaster newDungeonMaster = new DungeonMaster()
            {
                UserId = "test1",
                CampaignName = "Test Campaign",
                CampaignDesc = "Test Campaign Description",
                ExperienceLevel = ExperienceLevel.Medium,
                PersonalBio = "Test Personal Bio"
            };
            DungeonMasterDTO dungeonMasterDTO = await repo.BuildDTO(newDungeonMaster);

            Assert.NotNull(dungeonMasterDTO);
            Assert.Equal(newDungeonMaster.UserId, dungeonMasterDTO.UserId);
            Assert.Equal(newDungeonMaster.PersonalBio, dungeonMasterDTO.PersonalBio);
        }

        [Fact]
        public void CanDeconstructDTO()
        {
            var repo = BuildDb();
            DungeonMasterDTO dungeonMasterDTO = new DungeonMasterDTO()
            {
                UserId = "test1",
                CampaignName = "Test Campaign",
                CampaignDesc = "Test Campaign Description",
                ExperienceLevel = "Medium",
                PersonalBio = "Test Personal Bio"
            };
            DungeonMaster dungeonMaster = repo.DeconstructDTO(dungeonMasterDTO);

            Assert.NotNull(dungeonMasterDTO);
            Assert.Equal(dungeonMasterDTO.UserId, dungeonMaster.UserId);
            Assert.Equal(dungeonMasterDTO.PersonalBio, dungeonMaster.PersonalBio);
        }
    }
}