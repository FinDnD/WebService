using Espresso401_WebService.Data;
using Espresso401_WebService.Models.DTOs;
using Espresso401_WebService.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Espresso401_WebService.Models.Services
{
    public class RequestRepository : IRequest
    {
        private readonly AppDbContext _context;
        private readonly IParty _party;

        /// <summary>
        /// Constructor for the Request Repository
        /// </summary>
        /// <param name="context">AppDbContext</param>
        /// <param name="party">Party interface</param>
        public RequestRepository(AppDbContext context, IParty party)
        {
            _context = context;
            _party = party;
        }

        /// <summary>
        /// Create a new request in the database between a Player and Dungeon Master
        /// </summary>
        /// <param name="playerId">Player ID</param>
        /// <param name="dmId">Dungeon Master ID</param>
        /// <returns>Task of completion for request creation</returns>
        public async Task<RequestDTO> CreateRequest(int playerId, int dmId)
        {
            Request req = new Request
            {
                PlayerId = playerId,
                PlayerAccepted = false,
                DungeonMasterId = dmId,
                DungeonMasterAccepted = false,
                Active = true
            };
            _context.Entry(req).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return await BuildDTO(req);
        }

        /// <summary>
        /// Delete a specific request from the database between a Player and DM
        /// </summary>
        /// <param name="playerId">Player ID</param>
        /// <param name="dmId">Dungeon Master</param>
        /// <returns>Boolean representing if Request was deleted</returns>
        public async Task DeleteRequest(int playerId, int dmId)
        {
            Request req = await _context.Requests.Where(x => x.DungeonMasterId == dmId && x.PlayerId == playerId).FirstOrDefaultAsync();
            if (req != null)
            {
                _context.Entry(req).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Delete all requests for a specific User in the database
        /// </summary>
        /// <param name="id">ID number for finding requests, can be either a Player ID OR a Dungeon Master Id</param>
        /// <returns>Boolean representing if Requests were deleted</returns>
        public async Task DeleteAllUserRequests(int id, string profileType)
        {
            List<Request> reqs = null;
            if (profileType == "DM")
            {
                reqs = await _context.Requests.Where(x => x.DungeonMasterId == id).ToListAsync();
            }
            else
            {
                reqs = await _context.Requests.Where(x => x.PlayerId == id).ToListAsync();
            }
            if (reqs != null)
            {
                foreach (var req in reqs)
                {
                    _context.Entry(req).State = EntityState.Deleted;
                }
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Get all of the requests associated with a given User ID that are active and have not already been "Swiped right" by user and are Active
        /// </summary>
        /// <param name="userId">User ID to be searched</param>
        /// <returns>List of Active Requests for user</returns>
        public async Task<List<RequestDTO>> GetAllActiveUserRequests(string userId)
        {
            List<Request> reqs = null;
            var userPlayer = await _context.Players.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (userPlayer != null)
            {
                reqs = await _context.Requests.Where(x => x.PlayerId == userPlayer.Id && x.Active && !x.PlayerAccepted)
                                              .OrderBy(x => x.DungeonMasterAccepted)
                                              .ToListAsync();
            }
            else
            {
                var userDm = await _context.DungeonMasters.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                reqs = await _context.Requests.Where(x => x.DungeonMasterId == userDm.Id && x.Active && !x.DungeonMasterAccepted && x.Player.PartyId == 1)
                                              .OrderBy(x => x.PlayerAccepted)
                                              .ToListAsync();
            }
            List<RequestDTO> dtos = new List<RequestDTO>();
            foreach (Request req in reqs)
            {
                dtos.Add(await BuildDTO(req));
            }
            return dtos;
        }

        /// <summary>
        /// Get all of the requests associated with a given User ID
        /// </summary>
        /// <param name="userId">User ID to be searched</param>
        /// <returns>List of all Requests for a given user (Active and Inactive)</returns>
        public async Task<List<RequestDTO>> GetAllUserRequests(string userId)
        {
            List<Request> rawRequests = null;
            var userPlayer = await _context.Players.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (userPlayer != null)
            {
                rawRequests = await _context.Requests.Where(x => x.PlayerId == userPlayer.Id).ToListAsync();
            }
            else
            {
                var userDm = await _context.DungeonMasters.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                if (userDm == null)
                {
                    return new List<RequestDTO>();
                }
                rawRequests = await _context.Requests.Where(x => x.DungeonMasterId == userDm.Id).ToListAsync();
            }
            List<RequestDTO> requestDTOs = new List<RequestDTO>();
            foreach (Request request in rawRequests)
            {
                requestDTOs.Add(await BuildDTO(request));
            }
            return requestDTOs;
        }

        /// <summary>
        /// Update a specific request in the database and check if both users in request have approved or "swiped right"
        /// </summary>
        /// <param name="updatedRequest">Updated request information from swipe</param>
        /// <returns>New updated Request</returns>
        public async Task<RequestDTO> UpdateRequest(RequestDTO updatedRequestDTO)
        {
            Request updatedRequest = DeconstructDTO(updatedRequestDTO);
            if (updatedRequest.PlayerAccepted && updatedRequest.DungeonMasterAccepted)
            {
                await _party.AddPlayerToParty(updatedRequest.DungeonMasterId, updatedRequest.PlayerId);
                updatedRequest.Active = false;
                await DeactivateAllPlayerRequests(updatedRequest.PlayerId);
            }
            _context.Entry(updatedRequest).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return updatedRequestDTO;
        }

        /// <summary>
        /// Private method for Deactivating all of a player's requests, used when they are added toa  party
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        private async Task DeactivateAllPlayerRequests(int playerId)
        {
            List<Request> reqs = await _context.Requests.Where(x => x.PlayerId == playerId).ToListAsync();
            foreach (Request req in reqs)
            {
                req.Active = false;
                _context.Entry(req).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Set a specific Request to Inactive
        /// </summary>
        /// <param name="requestId">Id of the Requested to be deactivated</param>
        /// <returns>Task of completion</returns>
        public async Task<RequestDTO> DeactivateRequest(int requestId)
        {
            Request req = await _context.Requests.FindAsync(requestId);
            if (req != null)
            {
                req.Active = false;
                _context.Entry(req).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            return await BuildDTO(req);
        }

        /// <summary>
        /// Build a Request DTO from a Request object
        /// </summary>
        /// <param name="Request">Request to be converted to DTO</param>
        /// <returns>Request DTO</returns>
        public async Task<RequestDTO> BuildDTO(Request request)
        {
            RequestDTO dto = new RequestDTO
            {
                Id = request.Id,
                DungeonMasterId = request.DungeonMasterId,
                DungeonMasterAccepted = request.DungeonMasterAccepted,
                PlayerId = request.PlayerId,
                PlayerAccepted = request.PlayerAccepted,
                Active = request.Active,
            };
            DungeonMaster dm = await _context.DungeonMasters.FindAsync(dto.DungeonMasterId);
            Player player = await _context.Players.FindAsync(dto.PlayerId);
            dto.DungeonMaster = new DungeonMasterDTO
            {
                Id = dm.Id,
                UserId = dm.UserId,
                CampaignName = dm.CampaignName,
                CampaignDesc = dm.CampaignDesc,
                ExperienceLevel = dm.ExperienceLevel.ToString(),
                PersonalBio = dm.PersonalBio,
                ImageUrl = dm.ImageUrl
            };
            dto.Player = new PlayerDTO
            {
                Id = player.Id,
                UserId = player.UserId,
                ImageUrl = player.ImageUrl,
                CharacterName = player.CharacterName,
                Class = player.Class.ToString(),
                Race = player.Race.ToString(),
                ExperienceLevel = player.ExperienceLevel.ToString(),
                GoodAlignment = player.GoodAlignment,
                LawAlignment = player.LawAlignment,
                PartyId = player.PartyId
            };
            return dto;
        }

        /// <summary>
        /// Deconstruct a RequestDTO into a Request object
        /// </summary>
        /// <param name="RequestDTO">RequestDTO to be deconstructed</param>
        /// <returns>Request object from RequestDTO</returns>
        public Request DeconstructDTO(RequestDTO requestDTO)
        {
            Request request = new Request
            {
                Id = requestDTO.Id,
                DungeonMasterId = requestDTO.DungeonMasterId,
                DungeonMasterAccepted = requestDTO.DungeonMasterAccepted,
                PlayerId = requestDTO.PlayerId,
                PlayerAccepted = requestDTO.PlayerAccepted,
                Active = requestDTO.Active
            };
            return request;
        }
    }
}