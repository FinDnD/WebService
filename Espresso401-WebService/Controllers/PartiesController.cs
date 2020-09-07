using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Espresso401_WebService.Data;
using Espresso401_WebService.Models;
using Espresso401_WebService.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Espresso401_WebService.Models.DTOs;

namespace Espresso401_WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartiesController : ControllerBase
    {
        private readonly IParty _party;

        public PartiesController(IParty party)
        {
            _party = party;
        }

        // GET: api/Parties
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<List<PartyDTO>> GetParties()
        {
            return await _party.GetAllParties();
        }

        // GET: api/Parties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PartyDTO>> GetParty(int id)
        {
            var party = await _party.GetPartyById(id);

            if (party == null)
            {
                return NotFound();
            }

            return party;
        }    
        
        // GET: api/Parties/DungeonMasterId/5
        [HttpGet("DungeonMasterId/{id}")]
        public async Task<ActionResult<PartyDTO>> GetByDmIDParty(int id)
        {
            var party = await _party.GetPartyByDMId(id);

            if (party == null)
            {
                return NotFound();
            }

            return party;
        }

        // PUT: api/Parties/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParty(int id, Party party)
        {
            if (id != party.Id)
            {
                return BadRequest();
            }

            await _party.UpdateParty(party);

            return NoContent();
        }

        // POST: api/Parties
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Party>> PostParty(Party party)
        {
            var result = await _party.CreateParty(party);

            return CreatedAtAction("GetParty", new { id = result.Id }, party);
        }
    }
}
