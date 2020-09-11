using Espresso401_WebService.Models;
using Espresso401_WebService.Models.DTOs;
using Espresso401_WebService.Models.Interfaces;
using Espresso401_WebService.Models.Services;
using System.Threading.Tasks;
using Xunit;

namespace Espresso401_WebServiceTests
{
    public class RequestTests : DatabaseTest
    {
        public IRequest BuildDb()
        {
            return new RequestRepository(_appDb, _party);
        }

        public IPlayer BuildPlayerDb()
        {
            return new PlayerRepository(_appDb, _party, _request);
        }

        public IDungeonMaster BuildDMDb()
        {
            return new DungeonMasterRepository(_appDb, _party, _request);
        }

        [Fact]
        public async Task CanCreateRequest()
        {
            var repo = BuildDb();
            var newReq = await repo.CreateRequest(1, 1);
            var result = await repo.GetAllUserRequests("4c035675-9c5d-4763-aabe-6295555466b7");
            bool contained = false;
            foreach (var item in result)
            {
                if (item.DungeonMasterId == newReq.DungeonMasterId && item.PlayerId == newReq.PlayerId) contained = true;
            }


            Assert.NotNull(result);
            Assert.True(contained);
        }

        [Fact]
        public async Task CanGetAllUserRequests()
        {
            #region DataSeeding

            var playerRepo = BuildPlayerDb();
            var dmRepo = BuildDMDb();
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

            PlayerDTO newPlayer = new PlayerDTO()
            {
                UserId = "test3",
                CharacterName = "Test1Name",
                Class = "Barbarian",
                Race = "Dragonborn",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            PlayerDTO newPlayer2 = new PlayerDTO()
            {
                UserId = "test4",
                CharacterName = "Test2Name",
                Class = "Monk",
                Race = "HalfElf",
                ExperienceLevel = "High",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            newPlayer = await playerRepo.CreatePlayer(newPlayer);
            newPlayer2 = await playerRepo.CreatePlayer(newPlayer2);
            newDm = await dmRepo.CreateDungeonMaster(newDm);
            newDm2 = await dmRepo.CreateDungeonMaster(newDm2);

            #endregion DataSeeding

            var repo = BuildDb();
            RequestDTO req1 = await repo.CreateRequest(newPlayer.Id, newDm.Id);
            RequestDTO req2 = await repo.CreateRequest(newPlayer.Id, newDm2.Id);
            RequestDTO req3 = await repo.CreateRequest(newPlayer2.Id, newDm.Id);
            RequestDTO req4 = await repo.CreateRequest(newPlayer2.Id, newDm2.Id);

            var result = await repo.GetAllUserRequests(newPlayer.UserId);

            bool contained1 = false;
            bool contained2 = false;
            bool contained3 = false;
            bool contained4 = false;
            foreach (var item in result)
            {
                if (item.DungeonMasterId == newDm.Id && item.PlayerId == newPlayer.Id) contained1 = true;
                else if (item.DungeonMasterId == newDm2.Id && item.PlayerId == newPlayer.Id) contained2 = true;
                else if (item.DungeonMasterId == newDm.Id && item.PlayerId == newPlayer2.Id) contained3 = true;
                else if (item.DungeonMasterId == newDm2.Id && item.PlayerId == newPlayer2.Id) contained4 = true;
            }

            Assert.True(contained1);
            Assert.True(contained2);
            Assert.False(contained3);
            Assert.False(contained4);
        }

        [Fact]
        public async Task CanGetAllActiveRequests()
        {
            #region DataSeeding

            var playerRepo = BuildPlayerDb();
            var dmRepo = BuildDMDb();
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

            PlayerDTO newPlayer = new PlayerDTO()
            {
                UserId = "test3",
                CharacterName = "Test1Name",
                Class = "Barbarian",
                Race = "Dragonborn",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            PlayerDTO newPlayer2 = new PlayerDTO()
            {
                UserId = "test4",
                CharacterName = "Test2Name",
                Class = "Monk",
                Race = "HalfElf",
                ExperienceLevel = "High",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };

            newPlayer = await playerRepo.CreatePlayer(newPlayer);
            newPlayer2 = await playerRepo.CreatePlayer(newPlayer2);
            newDm = await dmRepo.CreateDungeonMaster(newDm);
            newDm2 = await dmRepo.CreateDungeonMaster(newDm2);

            #endregion DataSeeding

            var repo = BuildDb();
            RequestDTO req1 = await repo.CreateRequest(newPlayer.Id, newDm.Id);
            RequestDTO req2 = await repo.CreateRequest(newPlayer.Id, newDm2.Id);
            RequestDTO req3 = await repo.CreateRequest(newPlayer2.Id, newDm.Id);
            RequestDTO req4 = await repo.CreateRequest(newPlayer2.Id, newDm2.Id);

            await repo.DeactivateRequest(req1.Id);

            var result = await repo.GetAllActiveUserRequests(newPlayer.UserId);

            bool contained1 = false;
            bool contained2 = false;
            bool contained3 = false;
            bool contained4 = false;
            foreach (var item in result)
            {
                if (item.DungeonMasterId == newDm.Id && item.PlayerId == newPlayer.Id) contained1 = true;
                else if (item.DungeonMasterId == newDm2.Id && item.PlayerId == newPlayer.Id) contained2 = true;
                else if (item.DungeonMasterId == newDm.Id && item.PlayerId == newPlayer2.Id) contained3 = true;
                else if (item.DungeonMasterId == newDm2.Id && item.PlayerId == newPlayer2.Id) contained4 = true;
            }
            Assert.True(contained1);
            Assert.True(contained2);
            Assert.False(contained3);
            Assert.False(contained4);
        }

        [Fact]
        public async Task CanDeleteAllUserRequests()
        {
            #region DataSeeding

            var playerRepo = BuildPlayerDb();
            var dmRepo = BuildDMDb();
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

            PlayerDTO newPlayer = new PlayerDTO()
            {
                UserId = "test3",
                CharacterName = "Test1Name",
                Class = "Barbarian",
                Race = "Dragonborn",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            PlayerDTO newPlayer2 = new PlayerDTO()
            {
                UserId = "test4",
                CharacterName = "Test2Name",
                Class = "Monk",
                Race = "HalfElf",
                ExperienceLevel = "High",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            newPlayer = await playerRepo.CreatePlayer(newPlayer);
            newPlayer2 = await playerRepo.CreatePlayer(newPlayer2);
            newDm =await dmRepo.CreateDungeonMaster(newDm);
            newDm2 = await dmRepo.CreateDungeonMaster(newDm2);

            #endregion DataSeeding

            var repo = BuildDb();

            await repo.DeleteAllUserRequests(newPlayer.Id, "Player");

            var result = await repo.GetAllActiveUserRequests(newPlayer.UserId);

            Assert.Empty(result);
        }

        [Fact]
        public async Task CanDeleteSpecificRequest()
        {
            #region DataSeeding

            var playerRepo = BuildPlayerDb();
            var dmRepo = BuildDMDb();
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

            PlayerDTO newPlayer = new PlayerDTO()
            {
                UserId = "test3",
                CharacterName = "Test1Name",
                Class = "Barbarian",
                Race = "Dragonborn",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };

            PlayerDTO newPlayer2 = new PlayerDTO()
            {
                UserId = "test4",
                CharacterName = "Test2Name",
                Class = "Monk",
                Race = "HalfElf",
                ExperienceLevel = "High",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            newPlayer = await playerRepo.CreatePlayer(newPlayer);
            newPlayer2 = await playerRepo.CreatePlayer(newPlayer2);
            newDm = await dmRepo.CreateDungeonMaster(newDm);
            newDm2 = await dmRepo.CreateDungeonMaster(newDm2);

            #endregion DataSeeding

            var repo = BuildDb();
            RequestDTO req1 = await repo.CreateRequest(newPlayer.Id, newDm.Id);
            RequestDTO req2 = await repo.CreateRequest(newPlayer.Id, newDm2.Id);
            RequestDTO req3 = await repo.CreateRequest(newPlayer2.Id, newDm.Id);
            RequestDTO req4 = await repo.CreateRequest(newPlayer2.Id, newDm2.Id);

            // Requests get created when Players/DMs so there's actually two in DB when I force a 2nd above, this is not a production situation, in Production they are only ever created when Profiles are Created and Deleted when either Profile in the RequestDTO is.
            await repo.DeleteRequest(newPlayer.Id, newDm.Id);
            await repo.DeleteRequest(newPlayer.Id, newDm.Id);

            var result = await repo.GetAllActiveUserRequests(newPlayer.UserId);

            bool contained1 = false;
            bool contained2 = false;
            bool contained3 = false;
            bool contained4 = false;
            foreach (var item in result)
            {
                if (item.DungeonMasterId == newDm.Id && item.PlayerId == newPlayer.Id) contained1 = true;
                else if (item.DungeonMasterId == newDm2.Id && item.PlayerId == newPlayer.Id) contained2 = true;
                else if (item.DungeonMasterId == newDm.Id && item.PlayerId == newPlayer2.Id) contained3 = true;
                else if (item.DungeonMasterId == newDm2.Id && item.PlayerId == newPlayer2.Id) contained4 = true;
            }
            Assert.False(contained1);
            Assert.True(contained2);
            Assert.False(contained3);
            Assert.False(contained4);
        }

        [Fact]
        public async Task CanDeactivateRequest()
        {
            var repo = BuildDb();
            // Starts Active by default
            RequestDTO newReq = await repo.CreateRequest(1, 1);

            Assert.True(newReq.Active);

            newReq = await repo.DeactivateRequest(newReq.Id);

            Assert.False(newReq.Active);
        }

        //[Fact]
        //public async Task CanUpdateRequest()
        //{
        //    var repo = BuildDb();
        //    RequestDTO newReq = await repo.CreateRequest(1, 1);
        //    RequestDTO updatedReq = new RequestDTO()
        //    {
        //        Id = newReq.Id,
        //        DungeonMasterId = 1,
        //        PlayerId = 1,
        //        DungeonMasterAccepted = true,
        //        PlayerAccepted = false,
        //        Active = true
        //    };
        //    var updateResult = await repo.UpdateRequest(updatedReq);

        //    var result = await repo.GetAllUserRequests("SeededDM");

        //    Assert.NotNull(result);
        //    Assert.Contains(updateResult, result);
        //    Assert.DoesNotContain(newReq, result);
        //}
    }
}