using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;

namespace Espresso401_WebService.Models.Interfaces
{
    public interface IImage
    {
        /// <summary>
        /// Retrieve the CloudBlobContainer
        /// </summary>
        /// <param name="containerName">Blob container name</param>
        /// <returns>Task of completion of CloudBlobContainer object</returns>
        Task<CloudBlobContainer> GetContainerWith(string containerName);

        /// <summary>
        /// Upload a new image to Cloud Storage by bringing in and adding it to the Blob Container
        /// </summary>
        /// <param name="imageFileName">Image file name to use as the key in Cloud Storage</param>
        /// <param name="filePath">Temporary File Path for where the image is stored</param>       
        /// <param name="userId">Id of User to add image to their profile</param>
        /// <returns>Task of completion of URI string for the uploaded image</returns>
        Task<string> UploadImage(string imageFileName, string filePath, string userId);

        /// <summary>
        /// Add the imageURI to the Profile being updated
        /// </summary>
        /// <param name="userId">Id of User to add image to their profile</param>
        /// <param name="imageURI">Image URI to add to the Profile, stored in cloud storage</param>
        /// <returns>Task of completion of bool representing if Update was successful</returns>
        Task<bool> UpdateAppDbFor(string userId, string imageURI);
    }
}
