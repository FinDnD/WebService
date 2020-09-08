using Espresso401_WebService.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Espresso401_WebService.Models.Interfaces
{
    public interface IParty
    {
        /// <summary>
        /// Creates a new party in the database
        /// </summary>
        /// <param name="party">Party information to be added to database</param>
        /// <returns>Newly created party</returns>
        Task<PartyDTO> CreateParty(PartyDTO party);

        /// <summary>
        /// Get a specific party by the Dungeon Master Id associated with it
        /// </summary>
        /// <param name="dmId">Dungeon master id to find party from</param>
        /// <returns>PartyDTO for requested party</returns>
        Task<PartyDTO> GetPartyByDMId(int dmId);

        /// <summary>
        /// Get a specific party by the partyId
        /// </summary>
        /// <param name="partyId">Id of party to find</param>
        /// <returns>PartyDTO for requested party</returns>
        Task<PartyDTO> GetPartyById(int partyId);

        /// <summary>
        /// Get all parties in the database (Not currently in use)
        /// </summary>
        /// <returns>List of all Parties in database as DTOs</returns>
        Task<List<PartyDTO>> GetAllParties();

        /// <summary>
        /// Delete a specific party from the database
        /// </summary>
        /// <param name="dmId">Dungeon Master associated with party to be deleted</param>
        /// <returns>Task of completion for DM deletion</returns>
        Task DeleteParty(int dmId);

        /// <summary>
        /// Update a specific party
        /// </summary>
        /// <param name="party">Updated party information</param>
        /// <returns>Task of completion for party update</returns>
        Task UpdateParty(PartyDTO party);

        /// <summary>
        /// Build a PartyDTO object out of a Party Object
        /// </summary>
        /// <param name="party">Party object to build DTO from</param>
        /// <returns>PartyDTO object</returns>
        Task<PartyDTO> BuildPartyDTO(Party party); 
        
        /// <summary>
        /// Deconstruct a PartyDTO object into a Party Object
        /// </summary>
        /// <param name="partyDTO">PartyDTO object to deconstruct</param>
        /// <returns>Party object</returns>
        Party DeconstructDTO(PartyDTO partyDTO);

        /// <summary>
        /// Add a player to a party with a Dungeon Master ID for the party and player ID to be added
        /// </summary>
        /// <param name="dmId">Dungeon Master associated with the party's ID</param>
        /// <param name="playerId">Player to be added's ID</param>
        /// <returns>Task of completion for adding player to party</returns>
        Task AddPlayerToParty(int dmId, int playerId);

        /// <summary>
        /// Remove a player from a DM's party
        /// </summary>
        /// <param name="dmId">Dungeon Master associated with the party's ID</param>
        /// <param name="playerId">Player to be removed's ID</param>
        /// <returns>Task of completion for removing character from party</returns>
        Task RemovePlayerFromParty(int dmId, int playerId);
    }
}