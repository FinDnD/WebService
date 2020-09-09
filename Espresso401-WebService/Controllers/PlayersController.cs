using Espresso401_WebService.Models;
using Espresso401_WebService.Models.DTOs;
using Espresso401_WebService.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Espresso401_WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "Players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayer _player;
        private UserManager<ApplicationUser> _userManager;


        public PlayersController(IPlayer player, UserManager<ApplicationUser> userManager)
        {
            _player = player;
            _userManager = userManager;
        }

        // GET: api/Players
        // TODO: Admin Authorization
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<List<PlayerDTO>> GetPlayers()
        {
            return await _player.GetAllPlayers();
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<PlayerDTO> GetPlayerById(int id)
        {
            return await _player.GetPlayerById(id);
        }

        // GET: api/Players/5
        [HttpGet("UserId/{id}")]
        public async Task<PlayerDTO> GetPlayerByUserId(string id)
        {
            return await _player.GetPlayerByUserId(id);
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<PlayerDTO> PutPlayer(int id, PlayerDTO player)
        {
            if (id != player.Id)
            {
                return null;
            }

            return await _player.UpdatePlayer(player);
        }

        // POST: api/Players
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<PlayerDTO> PostPlayer(PlayerDTO player)
        {
            var userClaims = User.Claims;
            if(userClaims != null)
            {
                player.UserId = userClaims.FirstOrDefault(x => x.Type == "UserId").Value;
                player.UserName = userClaims.FirstOrDefault(x => x.Type == "UserName").Value;

                PlayerDTO result = await _player.CreatePlayer(player);

                if (result.Id != 0)
                {
                    return result;
                }
            }

            return null;
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<bool> DeletePlayer(int id)
        {
            bool result = await _player.DeletePlayer(id);
            return result;
        }
    }
}