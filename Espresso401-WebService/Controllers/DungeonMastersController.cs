using Espresso401_WebService.Models;
using Espresso401_WebService.Models.DTOs;
using Espresso401_WebService.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Espresso401_WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "DungeonMasters")]
    [ApiController]
    public class DungeonMastersController : ControllerBase
    {
        private readonly IDungeonMaster _dungeonMaster;
        private UserManager<ApplicationUser> _userManager;

        public DungeonMastersController(IDungeonMaster dungeonMaster, UserManager<ApplicationUser> userManager)
        {
            _dungeonMaster = dungeonMaster;
            _userManager = userManager;
        }

        // GET: api/DungeonMasters
        // TODO: Admin Authorization
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<List<DungeonMasterDTO>> GetDungeonMasters()
        {
            return await _dungeonMaster.GetAllDungeonMasters();
        }

        // GET: api/DungeonMasters/5
        [HttpGet("{id}")]
        public async Task<DungeonMasterDTO> GetDungeonMasterById(int id)
        {
            return await _dungeonMaster.GetDungeonMasterById(id);
        }

        // GET: api/DungeonMasters/5
        [HttpGet("UserId/{id}")]
        public async Task<DungeonMasterDTO> GetDungeonMasterByUserId(string id)
        {
            return await _dungeonMaster.GetDungeonMasterByUserId(id);
        }

        // PUT: api/DungeonMasters/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<DungeonMasterDTO> PutDungeonMaster(int id, DungeonMasterDTO dungeonMaster)
        {
            if (id != dungeonMaster.Id)
            {
                return null;
            }

            return await _dungeonMaster.UpdateDungeonMaster(dungeonMaster);
        }

        // POST: api/DungeonMasters
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DungeonMaster>> PostDungeonMaster(DungeonMasterDTO dungeonMaster)
        {
            var userClaims = User.Claims;

            if(userClaims != null)
            {
                dungeonMaster.UserId = userClaims.FirstOrDefault(x => x.Type == "UserId").Value;
                DungeonMasterDTO result = await _dungeonMaster.CreateDungeonMaster(dungeonMaster);

                if (result.Id != 0)
                {
                    return CreatedAtAction("GetDungeonMaster", new { id = dungeonMaster.Id }, dungeonMaster);
                }
            }
            return null;
        }

        // DELETE: api/DungeonMasters/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteDungeonMaster(int id)
        {
            bool result = await _dungeonMaster.DeleteDungeonMaster(id);
            return result;
        }
    }
}