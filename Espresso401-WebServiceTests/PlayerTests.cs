using Espresso401_WebService.Models;
using Espresso401_WebService.Models.DTOs;
using Espresso401_WebService.Models.Interfaces;
using Espresso401_WebService.Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static Espresso401_WebService.Models.Enums;

namespace Espresso401_WebServiceTests
{
    public class PlayerTests : DatabaseTest
    {
        private IPlayer BuildDb()
        {
            return new PlayerRepository(_appDb, _party, _request);
        }

        [Fact]
        public async Task CanSaveNewPlayer()
        {
            PlayerDTO newPlayer = new PlayerDTO()
            {
                UserId = "test1",
                CharacterName = "Test1Name",
                Class = "Barbarian",
                Race = "Dragonborn",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };

            var repo = BuildDb();
            var result = await repo.CreatePlayer(newPlayer);

            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(newPlayer.CharacterName, result.CharacterName);
        }

        [Fact]
        public async Task CanGetPlayerById()
        {
            PlayerDTO newPlayer = new PlayerDTO()
            {
                UserId = "test1",
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
                UserId = "test1",
                CharacterName = "Test1Name",
                Class = "Barbarian",
                Race = "Dragonborn",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            PlayerDTO newPlayer3 = new PlayerDTO()
            {
                UserId = "test1",
                CharacterName = "Test1Name",
                Class = "Barbarian",
                Race = "Dragonborn",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            var repo = BuildDb();
            newPlayer = await repo.CreatePlayer(newPlayer);
            newPlayer2 = await repo.CreatePlayer(newPlayer2);
            newPlayer3 = await repo.CreatePlayer(newPlayer3);

            var result = await repo.GetPlayerById(newPlayer2.Id);

            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(newPlayer2.Id, result.Id);
            Assert.Equal(newPlayer2.CharacterName, result.CharacterName);
            Assert.Equal(newPlayer2.Class, result.Class);
        }

        [Fact]
        public async Task CanGetPlayerByUserId()
        {
            PlayerDTO newPlayer = new PlayerDTO()
            {
                UserId = "test1",
                CharacterName = "Test1Name",
                Class = "Paladin",
                Race = "Human",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            PlayerDTO newPlayer2 = new PlayerDTO()
            {
                UserId = "test2",
                CharacterName = "Test2Name",
                Class = "Barbarian",
                Race = "Dragonborn",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            PlayerDTO newPlayer3 = new PlayerDTO()
            {
                UserId = "test3",
                CharacterName = "Test3Name",
                Class = "Druid",
                Race = "Dwarf",
                ExperienceLevel = "High",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            var repo = BuildDb();
            await repo.CreatePlayer(newPlayer);
            newPlayer2 = await repo.CreatePlayer(newPlayer2);
            await repo.CreatePlayer(newPlayer3);

            var result = await repo.GetPlayerByUserId("test2");

            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(newPlayer2.Id, result.Id);
            Assert.Equal(newPlayer2.CharacterName, result.CharacterName);
            Assert.Equal(newPlayer2.Class, result.Class);
        }

        [Fact]
        public async Task CanGetAllPlayers()
        {
            PlayerDTO newPlayer = new PlayerDTO()
            {
                UserId = "test1",
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
                UserId = "test2",
                CharacterName = "Test1Name",
                Class = "Barbarian",
                Race = "Dragonborn",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            PlayerDTO newPlayer3 = new PlayerDTO()
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
            var repo = BuildDb();
            newPlayer = await repo.CreatePlayer(newPlayer);
            newPlayer2 = await repo.CreatePlayer(newPlayer2);
            newPlayer3 = await repo.CreatePlayer(newPlayer3);

            List<PlayerDTO> result = await repo.GetAllPlayers();

            bool playerInList = false;
            foreach (var item in result)
            {
                if (item.UserId == "test2") playerInList = true;
            }

            Assert.NotEmpty(result);
            Assert.True(playerInList);
        }

        [Fact]
        public async Task CanUpdatePlayer()
        {
            var repo = BuildDb();
            PlayerDTO playerUpdate = new PlayerDTO()
            {
                Id = 1,
                UserId = "test1Update",
                CharacterName = "Test1Name",
                Class = "Druid",
                Race = "Gnome",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };

            var update = await repo.UpdatePlayer(playerUpdate);

            Assert.NotNull(update);
            Assert.Equal(playerUpdate.Id, update.Id);
            Assert.Equal("test1Update", update.UserId);
            Assert.Equal(playerUpdate.Class, update.Class);
        }

        [Fact]
        public async Task CanDeletePlayer()
        {
            PlayerDTO newPlayer = new PlayerDTO()
            {
                UserId = "test1",
                CharacterName = "Test1Name",
                Class = "Barbarian",
                Race = "Dragonborn",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            var repo = BuildDb();
            var create = await repo.CreatePlayer(newPlayer);

            Assert.NotNull(create);

            await repo.DeletePlayer(create.Id);

            var result = await repo.GetPlayerByUserId("test1");

            Assert.Null(result);
        }

        [Fact]
        public async Task CanBuildDTO()
        {
            var repo = BuildDb();
            Player newPlayer = new Player()
            {
                UserId = "test1",
                CharacterName = "Test1Name",
                Class = Class.Barbarian,
                Race = Race.Dragonborn,
                ExperienceLevel = ExperienceLevel.FirstTime,
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            PlayerDTO playerDTO = await repo.BuildDTO(newPlayer);

            Assert.NotNull(playerDTO);
            Assert.Equal(newPlayer.UserId, playerDTO.UserId);
            Assert.Equal(newPlayer.PartyId, playerDTO.PartyId);
        }

        [Fact]
        public void CanDeconstructDTO()
        {
            var repo = BuildDb();
            PlayerDTO playerDTO = new PlayerDTO()
            {
                UserId = "test1",
                CharacterName = "Test1Name",
                Class = "Barbarian",
                Race = "Dragonborn",
                ExperienceLevel = "FirstTime",
                GoodAlignment = 50,
                LawAlignment = 50,
                PartyId = 1
            };
            Player player = repo.DeconstructDTO(playerDTO);

            Assert.NotNull(playerDTO);
            Assert.Equal(playerDTO.UserId, player.UserId);
            Assert.Equal(playerDTO.PartyId, player.PartyId);
        }
    }
}