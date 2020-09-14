using Espresso401_WebService.Models;
using Espresso401_WebService.Models.DTOs;
using Espresso401_WebService.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Espresso401_WebService.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SwipesController : ControllerBase
    {
        private readonly IRequest _request;

        public SwipesController(IRequest request)
        {
            _request = request;
        }

        // PUT api/Swipes/5
        /// <summary>
        /// Handles the swiping mechanic on the front end. Request will have an updated boolean value representing the swipe that occured.
        /// </summary>
        /// <param name="id">Id of the Request</param>
        /// <param name="request">Updated request</param>
        /// <returns>Task of completion, 400 Bad Request if the IDs don't line up and 204 NoContent if it goes through.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, RequestDTO request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            await _request.UpdateRequest(request);
            return NoContent();
        }
    }
}