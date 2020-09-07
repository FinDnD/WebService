using System.Collections.Generic;
using System.Threading.Tasks;

namespace Espresso401_WebService.Models.Interfaces
{
    public interface IRequest
    {
        /// <summary>
        /// Create a new request in the database between a Player and Dungeon Master
        /// </summary>
        /// <param name="playerId">Player ID</param>
        /// <param name="dmId">Dungeon Master ID</param>
        /// <returns>Newly created Request</returns>
        Task<Request> CreateRequest(int playerId, int dmId);

        /// <summary>
        /// Get all of the requests associated with a given User ID
        /// </summary>
        /// <param name="userId">User ID to be searched</param>
        /// <returns>List of all Requests for a given user (Active and Inactive)</returns>
        Task<List<Request>> GetAllUserRequests(string userId);

        /// <summary>
        /// Get all of the requests associated with a given User ID that are active and have not already been "Swiped right" by user and are Active
        /// </summary>
        /// <param name="userId">User ID to be searched</param>
        /// <returns>List of Active Requests for user</returns>
        Task<List<Request>> GetAllActiveUserRequests(string userId);

        /// <summary>
        /// Update a specific request in the database and check if both users in request have approved or "swiped right"
        /// </summary>
        /// <param name="updatedRequest">Updated request information from swipe</param>
        /// <returns>New updated Request</returns>
        Task<Request> UpdateRequest(Request updatedRequest);

        /// <summary>
        /// Delete a specific request from the database between a Player and DM
        /// </summary>
        /// <param name="playerId">Player ID</param>
        /// <param name="dmId">Dungeon Master</param>
        /// <returns>Boolean representing if Request was deleted</returns>
        Task DeleteRequest(int playerId, int dmId);

        /// <summary>
        /// Delete all requests for a specific User in the database
        /// </summary>
        /// <param name="id">ID number for finding requests, can be either a Player ID OR a Dungeon Master Id</param>
        /// <returns>Boolean representing if Requests were deleted</returns>
        Task DeleteAllUserRequests(int id, string profileType);

        /// <summary>
        /// Set a specific Request to Inactive
        /// </summary>
        /// <param name="requestId">Id of the Requested to be deactivated</param>
        /// <returns>Task of completion</returns>
        Task<Request> DeactivateRequest(int requestId);
    }
}