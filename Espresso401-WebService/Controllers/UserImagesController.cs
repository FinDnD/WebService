﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Espresso401_WebService.Models;
using Espresso401_WebService.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace Espresso401_WebService.Controllers
{
    [Route("api/[controller]")]
    public class UserImagesController : Controller
    {
        private IImage _image;

        public UserImagesController(IImage image)
        {
            _image = image;
        }

        /// <summary>
        /// Route for uploading user images upon character creation. Takes in an image in the form of a MultiPartFormDataContent on the front end.
        /// </summary>
        /// <param name="file">Image file to be uploaded</param>
        /// <returns>Task of completion with string of image URL</returns>
        [HttpPost]
        public async Task<string> UploadUserImage(IFormFile file)
        {
            var userClaims = User.Claims;
            string userId = userClaims.FirstOrDefault(x => x.Type == "UserId").Value;

            var path = Path.GetTempFileName();

            using(var stream = System.IO.File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            string imageUrl = await _image.UploadImage($"{userId}CurrentImage", path, userId);
            return imageUrl;
        }
    }
}