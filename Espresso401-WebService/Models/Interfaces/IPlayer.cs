using Espresso401_WebService.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Espresso401_WebService.Models.Interfaces
{
    public interface IPlayer
    {
        /// <summary>
        /// Get all players in the database (Currently not in use)
        /// </summary>
        /// <returns>List of all players in database</returns>
        Task<List<PlayerDTO>> GetAllPlayers();

        /// <summary>
        /// Gets a specific Player from the database by their ID
        /// </summary>
        /// <param name="playerId">Player ID to be searched for</param>
        /// <returns>Player from Database with associated ID</returns>
        Task<PlayerDTO> GetPlayerById(int id);

        /// <summary>
        /// Get a specific Player from the database by the associated userId
        /// </summary>
        /// <param name="userId">User ID associated with Player to be searched for</param>
        /// <returns>Player associated with the given ID as a DTO</returns>
        Task<PlayerDTO> GetPlayerByUserId(string userId);  
        
        /// <summary>
        /// Get a specific Player from the database by the associated userId
        /// </summary>
        /// <param name="userId">User ID associated with Player to be searched for</param>
        /// <returns>Player associated with the given ID</returns>
        Task<Player> GetPlayerByUserIdNonDTO(string userId);

        /// <summary>
        /// Create a new Player Profile for a user in the database
        /// </summary>
        /// <param name="player">Player profile to be added to database</param>
        /// <returns>Newly created player</returns>
        Task<PlayerDTO> CreatePlayer(PlayerDTO player);

        /// <summary>
        /// Updates a given player in the database
        /// </summary>
        /// <param name="player">Updated player information</param>
        /// <returns>Updated player</returns>
        Task<PlayerDTO> UpdatePlayer(PlayerDTO player);

        /// <summary>
        /// Delete a specific player from the database
        /// </summary>
        /// <param name="id">Id of player to be deleted</param>
        /// <returns>Task of completion for character deletion</returns>
        Task<bool> DeletePlayer(int charId);

        /// <summary>
        /// Build a Player DTO from a Player object
        /// </summary>
        /// <param name="player">Player to be converted to DTO</param>
        /// <returns>Player DTO</returns>
        Task<PlayerDTO> BuildDTO(Player player);

        /// <summary>
        /// Deconstruct a PlayerDTO into a Player object
        /// </summary>
        /// <param name="playerDTO">PlayerDTO to be deconstructed</param>
        /// <returns>Player object from PlayerDTO</returns>
        Player DeconstructDTO(PlayerDTO playerDTO);
    }
}