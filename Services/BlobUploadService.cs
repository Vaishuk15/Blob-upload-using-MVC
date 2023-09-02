using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlobUpload.Services
{
    public class BlobUploadService : IBlobUploadService
    {
        private readonly BlobContainerClient _blobContainerClient;

        public BlobUploadService(IConfiguration configuration)
        {
            var blobConnection = configuration.GetConnectionString("StorageConnection") ?? Environment.GetEnvironmentVariable("StorageConnection");

            _blobContainerClient = GetContainerClient(blobConnection, configuration["BlobUpload"] ?? Environment.GetEnvironmentVariable("BlobUpload"));
        }
        public async Task<string> UploadInBlob(IFormFile image)
        {
            var blobUrl = await UploadStreamIntoBlobAsync(
                image.FileName, image.OpenReadStream(), image.ContentType);
            return blobUrl;
        }

        private static BlobContainerClient GetContainerClient(string connectionString, string containerName)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            bool isExist = containerClient.Exists();
            if (!isExist)
            {
                containerClient.Create();
            }

            return containerClient;
        }

        public async Task<string> UploadStreamIntoBlobAsync(string fileName, Stream stream, string contentType)
        {
            try
            {
                // Retrieve reference to a blob named.
                BlobClient blockBlob = _blobContainerClient.GetBlobClient(fileName);

                var blobHttpHeader = new BlobHttpHeaders { ContentType = contentType };

                await blockBlob.UploadAsync(stream, blobHttpHeader);

                return blockBlob.Uri.ToString();
            }
            catch (Exception e)
            {
                //_logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
