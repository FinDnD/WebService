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
    public class DungeonMasterRepository : IDungeonMaster
    {
        private readonly AppDbContext _context;
        private readonly IParty _party;
        private readonly IRequest _request;

        /// <summary>
        /// Constructor for the Dungeon Master repository
        /// </summary>
        /// <param name="context">AppDbContext</param>
        /// <param name="party">Party interface</param>
        /// <param name="request">Request interface</param>
        public DungeonMasterRepository(AppDbContext context, IParty party, IRequest request)
        {
            _context = context;
            _party = party;
            _request = request;
        }

        /// <summary>
        /// Create a new Dungeon Master with the provided information from the API request &
        /// Create Request Objects between the new DM and all active players in the database.
        /// </summary>
        /// <param name="dungeonMaster">Dungeon Master information from the API request</param>
        /// <returns>Newly created Dungeon Master</returns>
        public async Task<DungeonMasterDTO> CreateDungeonMaster(DungeonMasterDTO dungeonMasterDTO)
        {
            DungeonMaster dungeonMaster = DeconstructDTO(dungeonMasterDTO);
            _context.Entry(dungeonMaster).State = EntityState.Added;
            var result = await _context.SaveChangesAsync();
            dungeonMasterDTO = await BuildDTO(dungeonMaster);
            dungeonMasterDTO.ActiveRequests = new List<RequestDTO>();

            var players = await _context.Players.Where(x => x.PartyId == 1).ToListAsync();
            if (players != null)
            {
                foreach (var player in players)
                {
                    RequestDTO newReq = await _request.CreateRequest(player.Id, dungeonMaster.Id);
                    dungeonMasterDTO.ActiveRequests.Add(newReq);
                }
            }
            PartyDTO newParty = new PartyDTO
            {
                DungeonMasterId = dungeonMasterDTO.Id,
                MaxSize = dungeonMasterDTO.PartySize,
                Full = false
            };

            dungeonMasterDTO.Party = await _party.CreateParty(newParty);

            dungeonMasterDTO.Id = dungeonMaster.Id;
            return dungeonMasterDTO;
        }

        /// <summary>
        /// Delete a DM and their associated party from the database
        /// </summary>
        /// <param name="userId">User ID for Dungeon Master to be deleted</param>
        /// <returns>Task of completion for deletion of Dungeon Master</returns>
        public async Task<bool> DeleteDungeonMaster(int id)
        {
            // TODO: Delete all associated requests
            var dm = await _context.DungeonMasters.FindAsync(id);
            if (dm != null)
            {
                await _party.DeleteParty(dm.Id);
                await _request.DeleteAllUserRequests(dm.Id, "DM");
                _context.Entry(dm).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get all Dungeon Masters from the database (Currently not in use)
        /// </summary>
        /// <returns>Task of completion with list of Dungeon Masters from database</returns>
        public async Task<List<DungeonMasterDTO>> GetAllDungeonMasters()
        {
            List<DungeonMaster> dungeonMasters = await _context.DungeonMasters.ToListAsync();
            List<DungeonMasterDTO> dtos = new List<DungeonMasterDTO>();
            foreach (DungeonMaster dungeonMaster in dungeonMasters)
            {
                dtos.Add(await BuildDTO(dungeonMaster));
            }
            return dtos;
        }

        /// <summary>
        /// Get a specific Dungeon Master based on the UserID associated with them
        /// </summary>
        /// <param name="userId">User ID associated with Dungeon Master to get</param>
        /// <returns>Dungeon Master associated with the given userId</returns>
        public async Task<DungeonMasterDTO> GetDungeonMasterByUserId(string userId)
        {
            DungeonMaster dungeonMaster = await _context.DungeonMasters.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            DungeonMasterDTO dmDTO = null;
            if (dungeonMaster != null)
            {
                dmDTO = await BuildDTO(dungeonMaster);
            }
            return dmDTO;
        }      
        
        /// <summary>
        /// Get a specific Dungeon Master based on the UserID associated with them
        /// </summary>
        /// <param name="userId">User ID associated with Dungeon Master to get</param>
        /// <returns>Dungeon Master associated with the given userId</returns>
        public async Task<DungeonMaster> GetDungeonMasterByUserIdNonDTO(string userId)
        {
            DungeonMaster dungeonMaster = await _context.DungeonMasters.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            return dungeonMaster;
        }

        /// <summary>
        /// Get a specific Dungeon Master based on their ID
        /// </summary>
        /// <param name="dungeonMasterId">Dungeon Master Id to be found</param>
        /// <returns>Dungeon Master associated with the given id</returns>
        public async Task<DungeonMasterDTO> GetDungeonMasterById(int dungeonMasterId)
        {
            DungeonMaster dungeonMaster = await _context.DungeonMasters.FindAsync(dungeonMasterId);
            DungeonMasterDTO dmDTO = null;
            if (dungeonMaster != null)
            {
                dmDTO = await BuildDTO(dungeonMaster);
            }
            return dmDTO;
        }

        /// <summary>
        /// Update a specific Dungeon Master in the database
        /// </summary>
        /// <param name="dungeonMaster">Updated Dungeon Master information</param>
        /// <returns>Updated Dungeon Master</returns>
        public async Task<DungeonMasterDTO> UpdateDungeonMaster(DungeonMasterDTO dungeonMasterDTO)
        {
            DungeonMaster dungeonMaster = DeconstructDTO(dungeonMasterDTO);
            _context.Entry(dungeonMaster).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return dungeonMasterDTO;
        }

        /// <summary>
        /// Builds a Dungeon Master DTO from a Dungeon master object
        /// </summary>
        /// <param name="dungeonMaster">Dungeon Master to be converted to DTO</param>
        /// <returns>Dungeon Master DTO</returns>
        public async Task<DungeonMasterDTO> BuildDTO(DungeonMaster dungeonMaster)
        {
            DungeonMasterDTO result = new DungeonMasterDTO
            {
                Id = dungeonMaster.Id,
                UserId = dungeonMaster.UserId,
                UserName = dungeonMaster.UserName,
                ImageUrl = dungeonMaster.ImageUrl,
                CampaignName = dungeonMaster.CampaignName,
                CampaignDesc = dungeonMaster.CampaignDesc,
                ExperienceLevel = dungeonMaster.ExperienceLevel.ToString(),
                PersonalBio = dungeonMaster.PersonalBio,
                Party = await _party.GetPartyByDMId(dungeonMaster.Id),
                ActiveRequests = await _request.GetAllActiveUserRequests(dungeonMaster.UserId)
            };
            return result;
        }

        /// <summary>
        /// Deconstruct a DungeonMasterDTO into a DungeonMaster object
        /// </summary>
        /// <param name="dungeonMasterDTO">Dungeon Master DTO for deconstruction</param>
        /// <returns>DungeonMaster object from DTO</returns>
        public DungeonMaster DeconstructDTO(DungeonMasterDTO dungeonMasterDTO)
        {
            Enum.TryParse(dungeonMasterDTO.ExperienceLevel, out ExperienceLevel exp);
            DungeonMaster result = new DungeonMaster
            {
                Id = dungeonMasterDTO.Id,
                UserId = dungeonMasterDTO.UserId,
                UserName = dungeonMasterDTO.UserName,
                CampaignName = dungeonMasterDTO.CampaignName,
                CampaignDesc = dungeonMasterDTO.CampaignDesc,
                ExperienceLevel = exp,
                PersonalBio = dungeonMasterDTO.PersonalBio
            };
            return result;
        }
    }
}