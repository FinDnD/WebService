using Espresso401_WebService.Data;
using Espresso401_WebService.Models.DTOs;
using Espresso401_WebService.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Espresso401_WebService.Models.Enums;

namespace Espresso401_WebService.Models.Services
{
    public class PlayerRepository : IPlayer
    {
        private readonly AppDbContext _context;
        private readonly IParty _party;
        private readonly IRequest _request;

        /// <summary>
        /// Constructor for the Player Repository
        /// </summary>
        /// <param name="context">AppDbContext</param>
        /// <param name="party">Party interface</param>
        /// <param name="request">Request interface</param>
        public PlayerRepository(AppDbContext context, IParty party, IRequest request)
        {
            _context = context;
            _party = party;
            _request = request;
        }

        /// <summary>
        /// Create a new Player Profile for a user in the database
        /// </summary>
        /// <param name="player">Player profile to be added to database</param>
        /// <returns>Newly created player</returns>
        public async Task<PlayerDTO> CreatePlayer(PlayerDTO playerDTO)
        {
            Player player = DeconstructDTO(playerDTO);
            player.PartyId = 1;
            _context.Entry(player).State = EntityState.Added;
            await _context.SaveChangesAsync();
            playerDTO = await BuildDTO(player);
            playerDTO.ActiveRequests = new List<RequestDTO>();
            // TODO: Only get DMs with a party that isn't already full
            var dms = await _context.DungeonMasters.ToListAsync();
            if (dms != null)
            {
                foreach (var dm in dms)
                {
                    RequestDTO newReq = await _request.CreateRequest(player.Id, dm.Id);
                    playerDTO.ActiveRequests.Add(newReq);
                }
            }
            playerDTO.Id = player.Id;
            return playerDTO;
        }

        /// <summary>
        /// Delete a specific player from the database
        /// </summary>
        /// <param name="id">Id of player to be deleted</param>
        /// <returns>Task of completion for character deletion</returns>
        public async Task<bool> DeletePlayer(int id)
        {
            // TODO: Delete all associated requests
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                await _request.DeleteAllUserRequests(player.Id, "Player");
                _context.Entry(player).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get all players in the database (Currently not in use)
        /// </summary>
        /// <returns>List of all players in database</returns>
        public async Task<List<PlayerDTO>> GetAllPlayers()
        {
            List<Player> players = await _context.Players.ToListAsync();
            List<PlayerDTO> playerDTOs = new List<PlayerDTO>();
            foreach (Player player in players)
            {
                playerDTOs.Add(await BuildDTO(player));
            }
            return playerDTOs;
        }

        /// <summary>
        /// Get a specific Player from the database by the associated userId
        /// </summary>
        /// <param name="userId">User ID associated with Player to be searched for</param>
        /// <returns>Player associated with the given ID</returns>
        public async Task<PlayerDTO> GetPlayerByUserId(string userId)
        {
            Player player = await _context.Players.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            PlayerDTO dto = null;
            if (player != null)
            {
                dto = await BuildDTO(player);
            }
            return dto;
        }     
        
        /// <summary>
        /// Get a specific Player from the database by the associated userId
        /// </summary>
        /// <param name="userId">User ID associated with Player to be searched for</param>
        /// <returns>Player associated with the given ID</returns>
        public async Task<Player> GetPlayerByUserIdNonDTO(string userId)
        {
            Player player = await _context.Players.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            return player;
        }

        /// <summary>
        /// Gets a specific Player from the database by their ID
        /// </summary>
        /// <param name="playerId">Player ID to be searched for</param>
        /// <returns>Player from Database with associated ID</returns>
        public async Task<PlayerDTO> GetPlayerById(int playerId)
        {
            Player player = await _context.Players.FindAsync(playerId);
            PlayerDTO dto = null;
            if (player != null)
            {
                dto = await BuildDTO(player);
            }
            return dto;
        }

        /// <summary>
        /// Updates a given player in the database
        /// </summary>
        /// <param name="player">Updated player information</param>
        /// <returns>Updated player</returns>
        public async Task<PlayerDTO> UpdatePlayer(PlayerDTO playerDTO)
        {
            Player player = DeconstructDTO(playerDTO);
            _context.Entry(player).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return playerDTO;
        }

        /// <summary>
        /// Build a Player DTO from a Player object
        /// </summary>
        /// <param name="player">Player to be converted to DTO</param>
        /// <returns>Player DTO</returns>
        public async Task<PlayerDTO> BuildDTO(Player player)
        {
            PlayerDTO result = new PlayerDTO
            {
                Id = player.Id,
                UserId = player.UserId,
                UserName = player.UserName,
                UserEmail = player.UserEmail,
                ImageUrl = player.ImageUrl,
                CharacterName = player.CharacterName,
                Class = player.Class.ToString(),
                Race = player.Race.ToString(),
                ExperienceLevel = player.ExperienceLevel.ToString(),
                GoodAlignment = player.GoodAlignment,
                LawAlignment = player.LawAlignment,
                PartyId = player.PartyId,
                Party = await _party.GetPartyById((int)player.PartyId),
            };
            var reqs = await _request.GetAllUserRequests(player.UserId);
            if (reqs != null)
            {
                result.ActiveRequests = reqs;
            }
            return result;
        }

        /// <summary>
        /// Deconstruct a PlayerDTO into a Player object
        /// </summary>
        /// <param name="playerDTO">PlayerDTO to be deconstructed</param>
        /// <returns>Player object from PlayerDTO</returns>
        public Player DeconstructDTO(PlayerDTO playerDTO)
        {
            Enum.TryParse(playerDTO.Race, out Race race);
            Enum.TryParse(playerDTO.Class, out Class playerClass);
            Enum.TryParse(playerDTO.ExperienceLevel, out ExperienceLevel exp);
            Player result = new Player
            {
                Id = playerDTO.Id,
                UserId = playerDTO.UserId,
                UserName = playerDTO.UserName,
                UserEmail = playerDTO.UserEmail,
                ImageUrl = playerDTO.ImageUrl,
                CharacterName = playerDTO.CharacterName,
                Class = playerClass,
                Race = race,
                ExperienceLevel = exp,
                GoodAlignment = playerDTO.GoodAlignment,
                LawAlignment = playerDTO.LawAlignment,
                PartyId = playerDTO.PartyId,
            };
            return result;
        }
    }
}