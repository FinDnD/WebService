using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Espresso401_WebService.Models;
using Espresso401_WebService.Models.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace Espresso401_WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserImagesController : ControllerBase
    {
        private IConfiguration _config;
        private IImage _image;
        private UserManager<ApplicationUser> _userManager;

        public UserImagesController(IConfiguration config, UserManager<ApplicationUser> userManager, IImage image)
        {
            _config = config;
            _image = image;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<string> UploadUserImage(IFormFile image)
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id;

            var path = Path.GetTempFileName();

            using(var stream = System.IO.File.Create(path))
            {
                await image.CopyToAsync(stream);
            }

            string imageUrl = await _image.UploadImage($"{user.UserName}CurrentImage", path, userId);
            return imageUrl;
        }
    }
}
