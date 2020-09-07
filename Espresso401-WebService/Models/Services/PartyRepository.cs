using Espresso401_WebService.Data;
using Espresso401_WebService.Models.DTOs;
using Espresso401_WebService.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Espresso401_WebService.Models.Services
{
    public class PartyRepository : IParty
    {
        private readonly AppDbContext _context;

        public PartyRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new party in the database
        /// </summary>
        /// <param name="party">Party information to be added to database</param>
        /// <returns>Newly created party</returns>
        public async Task<Party> CreateParty(Party party)
        {
            _context.Entry(party).State = EntityState.Added;
            var result = await _context.SaveChangesAsync();
            return party;
        }

        /// <summary>
        /// Delete a specific party from the database
        /// </summary>
        /// <param name="dmId">Dungeon Master associated with party to be deleted</param>
        /// <returns>Task of completion for DM deletion</returns>
        public async Task DeleteParty(int dmId)
        {
            var party = await _context.Parties.Where(x => x.DungeonMasterId == dmId).FirstOrDefaultAsync();
            if (party != null)
            {
                // TODO: Is this handled by Cascade Delete already?
                List<PlayerInParty> players = await _context.PlayerInParty.Where(x => x.PartyId == party.Id).ToListAsync();
                _context.PlayerInParty.RemoveRange(players);
                _context.Entry(party).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Get all parties in the database (Not currently in use)
        /// </summary>
        /// <returns>List of all Parties in database as DTOs</returns>
        public async Task<List<PartyDTO>> GetAllParties()
        {
            List<Party> parties = await _context.Parties.ToListAsync();
            List<PartyDTO> dtos = new List<PartyDTO>();
            foreach (Party party in parties)
            {
                dtos.Add(await BuildPartyDTO(party));
            }
            return dtos;
        }

        /// <summary>
        /// Get a specific party by the Dungeon Master Id associated with it
        /// </summary>
        /// <param name="dmId">Dungeon master id to find party from</param>
        /// <returns>PartyDTO for requested party</returns>
        public async Task<PartyDTO> GetPartyByDMId(int dmId)
        {
            Party party = await _context.Parties.Where(x => x.DungeonMasterId == dmId).Include(x => x.PlayersInParty).FirstOrDefaultAsync();
            if (party != null)
            {
                var dto = await BuildPartyDTO(party);
                return dto;
            }
            return null;
        }

        /// <summary>
        /// Get a specific party by the partyId
        /// </summary>
        /// <param name="partyId">Id of party to find</param>
        /// <returns>PartyDTO for requested party</returns>
        public async Task<PartyDTO> GetPartyById(int partyId)
        {
            Party party = await _context.Parties.FindAsync(partyId);
            if (party != null)
            {
                var dto = await BuildPartyDTO(party);
                return dto;
            }
            return null;
        }

        /// <summary>
        /// Build a PartyDTO object out of a Party Object
        /// </summary>
        /// <param name="party">Party object to build DTO from</param>
        /// <returns>PartyDTO object</returns>
        public async Task<PartyDTO> BuildPartyDTO(Party party)
        {
            DungeonMaster dm = await _context.DungeonMasters.FindAsync(party.DungeonMasterId);
            List<Player> members = await _context.Players.Where(x => x.PartyId == party.Id).ToListAsync();
            PartyDTO dto = new PartyDTO
            {
                Id = party.Id,
                DungeonMasterId = party.DungeonMasterId,
                MaxSize = party.MaxSize,
                Full = party.Full,
                PlayersInParty = new List<PartyPlayerDTO>(),
                DungeonMasterDTO = new PartyDmDTO
                {
                    Id = dm.Id,
                    CampaignName = dm.CampaignName,
                    CampaignDesc = dm.CampaignDesc,
                    ExperienceLevel = dm.ExperienceLevel.ToString(),
                    PersonalBio = dm.PersonalBio
                }
            };
            foreach (Player player in members)
            {
                dto.PlayersInParty.Add(new PartyPlayerDTO
                {
                    Id = player.Id,
                    ImageUrl = player.ImageUrl,
                    CharacterName = player.CharacterName,
                    Class = player.Class.ToString(),
                    Race = player.Race.ToString(),
                    ExperienceLevel = player.ExperienceLevel.ToString()
                });
            }
            return dto;
        }

        /// <summary>
        /// Update a specific party
        /// </summary>
        /// <param name="party">Updated party information</param>
        /// <returns>Task of completion for party update</returns>
        public async Task UpdateParty(Party party)
        {
            _context.Entry(party).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Add a player to a party with a Dungeon Master ID for the party and player ID to be added
        /// </summary>
        /// <param name="dmId">Dungeon Master associated with the party's ID</param>
        /// <param name="playerId">Player to be added's ID</param>
        /// <returns>Task of completion for adding player to party</returns>
        public async Task AddPlayerToParty(int dmId, int playerId)
        {
            Party party = await _context.Parties.Where(x => x.DungeonMasterId == dmId).FirstOrDefaultAsync();
            PlayerInParty newPlayer = new PlayerInParty
            {
                PartyId = party.Id,
                PlayerId = playerId
            };
            Player player = await _context.Players.Where(x => x.Id == playerId).FirstOrDefaultAsync();
            player.PartyId = party.Id;
            _context.Entry(player).State = EntityState.Modified;
            _context.Entry(newPlayer).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove a player from a DM's party
        /// </summary>
        /// <param name="dmId">Dungeon Master associated with the party's ID</param>
        /// <param name="playerId">Player to be removed's ID</param>
        /// <returns>Task of completion for removing character from party</returns>
        public async Task RemovePlayerFromParty(int dmId, int playerId)
        {
            Party party = await _context.Parties.Where(x => x.DungeonMasterId == dmId).FirstOrDefaultAsync();
            PlayerInParty playerInParty = await _context.PlayerInParty.FindAsync(playerId, party.Id);
            if (playerInParty != null)
            {
                _context.Entry(playerInParty).State = EntityState.Deleted;
                var playerReqs = await _context.Requests.Where(x => x.PlayerId == playerId).ToListAsync();
                // TODO: Revisit this logic
                foreach (Request req in playerReqs)
                {
                    req.PlayerAccepted = false;
                    req.DungeonMasterAccepted = false;
                    req.Active = true;
                }
                Player player = await _context.Players.Where(x => x.Id == playerId).FirstOrDefaultAsync();
                player.PartyId = 1;
                await _context.SaveChangesAsync();
            }
        }
    }
}