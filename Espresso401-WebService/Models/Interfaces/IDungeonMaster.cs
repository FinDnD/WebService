using Espresso401_WebService.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Espresso401_WebService.Models.Interfaces
{
    public interface IDungeonMaster
    {
        /// <summary>
        /// Get all Dungeon Masters from the database (Currently not in use)
        /// </summary>
        /// <returns>Task of completion with list of Dungeon Masters from database</returns>
        Task<List<DungeonMasterDTO>> GetAllDungeonMasters();

        /// <summary>
        /// Get a specific Dungeon Master based on the UserID associated with them
        /// </summary>
        /// <param name="userId">User ID associated with Dungeon Master to get</param>
        /// <returns>Dungeon Master associated with the given userId</returns>
        Task<DungeonMasterDTO> GetDungeonMasterByUserId(string userId);

        /// <summary>
        /// Get a specific Dungeon Master based on their ID
        /// </summary>
        /// <param name="dungeonMasterId">Dungeon Master Id to be found</param>
        /// <returns>Dungeon Master associated with the given id</returns>
        Task<DungeonMasterDTO> GetDungeonMasterById(int id);

        /// <summary>
        /// Create a new Dungeon Master with the provided information from the API request &
        /// Create Request Objects between the new DM and all active players in the database.
        /// </summary>
        /// <param name="dungeonMaster">Dungeon Master information from the API request</param>
        /// <returns>Newly created Dungeon Master</returns>
        Task<DungeonMasterDTO> CreateDungeonMaster(DungeonMasterDTO dungeonMasterDTO);

        /// <summary>
        /// Update a specific Dungeon Master in the database
        /// </summary>
        /// <param name="dungeonMaster">Updated Dungeon Master information</param>
        /// <returns>Updated Dungeon Master</returns>
        Task<DungeonMasterDTO> UpdateDungeonMaster(DungeonMasterDTO dungeonMasterDTO);

        /// <summary>
        /// Delete a DM and their associated party from the database
        /// </summary>
        /// <param name="userId">User ID for Dungeon Master to be deleted</param>
        /// <returns>Task of completion for deletion of Dungeon Master</returns>
        Task<bool> DeleteDungeonMaster(int id);

        /// <summary>
        /// Builds a Dungeon Master DTO from a Dungeon master object
        /// </summary>
        /// <param name="dungeonMaster">Dungeon Master to be converted to DTO</param>
        /// <returns>Dungeon Master DTO</returns>
        Task<DungeonMasterDTO> BuildDTO(DungeonMaster dungeonMaster);

        /// <summary>
        /// Deconstruct a DungeonMasterDTO into a DungeonMaster object
        /// </summary>
        /// <param name="dungeonMasterDTO">Dungeon Master DTO for deconstruction</param>
        /// <returns>DungeonMaster object from DTO</returns>
        DungeonMaster DeconstructDTO(DungeonMasterDTO dungeonMasterDTO);
    }
}