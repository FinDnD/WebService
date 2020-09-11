using Espresso401_WebService.Data;
using Espresso401_WebService.Models.DTOs;
using Espresso401_WebService.Models.Interfaces;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Espresso401_WebService.Models.Services
{
    public class ImageRepository : IImage
    {
        private IConfiguration _config;
        private IDungeonMaster _dungeonMaster;
        private IPlayer _player;
        private readonly AppDbContext _context;

        public CloudStorageAccount CloudStorageAccount { get; set; }

        public CloudBlobClient CloudBlobClient { get; set; }

        public ImageRepository(IConfiguration config, IDungeonMaster dungeonMaster, IPlayer player, AppDbContext context)
        {
            _config = config;
            _dungeonMaster = dungeonMaster;
            _player = player;
            _context = context;
            StorageCredentials storageCreds = new StorageCredentials(_config["AzureBlobAccountName"], _config["AzureBlobKey"]);
            CloudStorageAccount = new CloudStorageAccount(storageCreds, true);
            CloudBlobClient = CloudStorageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// Retrieve the CloudBlobContainer
        /// </summary>
        /// <param name="containerName">Blob container name</param>
        /// <returns>Task of completion of CloudBlobContainer object</returns>
        public async Task<CloudBlobContainer> GetContainerWith(string containerName)
        {
            CloudBlobContainer cloudBlobCtn = CloudBlobClient.GetContainerReference(containerName.ToLower());
            await cloudBlobCtn.CreateIfNotExistsAsync();
            await cloudBlobCtn.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            return cloudBlobCtn;
        }

        /// <summary>
        /// Upload a new image to Cloud Storage by bringing in and adding it to the Blob Container
        /// </summary>
        /// <param name="imageFileName">Image file name to use as the key in Cloud Storage</param>
        /// <param name="filePath">Temporary File Path for where the image is stored</param>       
        /// <param name="userId">Id of User to add image to their profile</param>
        /// <returns>Task of completion of URI string for the uploaded image</returns>
        public async Task<string> UploadImage(string imageFileName, string filePath, string userId)
        {
            CloudBlobContainer container = await GetContainerWith("espresso401images");
            CloudBlockBlob blobRef = container.GetBlockBlobReference(imageFileName);
            await blobRef.UploadFromFileAsync(filePath);
            return blobRef.Uri.AbsoluteUri;
        }

        /// <summary>
        /// Add the imageURI to the Profile being updated
        /// </summary>
        /// <param name="userId">Id of User to add image to their profile</param>
        /// <param name="imageURI">Image URI to add to the Profile, stored in cloud storage</param>
        /// <returns>Task of completion of bool representing if Update was successful</returns>
        public async Task<bool> UpdateAppDbFor(string userId, string imageURI)
        {
            Player userPlayer = await _player.GetPlayerByUserIdNonDTO(userId);
            if (userPlayer != null)
            {
                userPlayer.ImageUrl = imageURI;
                _context.Entry(userPlayer).State = EntityState.Modified;
                return true;
            }

            DungeonMaster userDungeonMaster = await _dungeonMaster.GetDungeonMasterByUserIdNonDTO(userId);
            if (userDungeonMaster != null)
            {
                userDungeonMaster.ImageUrl = imageURI;
                _context.Entry(userDungeonMaster).State = EntityState.Modified;
                return true;
            }
            return false;
        }
    }
}
