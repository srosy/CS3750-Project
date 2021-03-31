using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;

namespace LMS.Data
{
    public class AzureStorage
    {
        private const string STORAGE_CONN_STRING = "DefaultEndpointsProtocol=https;AccountName=3750lms;AccountKey=Ov5wA7dDj2Zd5+XJMdi5ntZc5ilUBhHNzIBjIDq5GKZNin4VbOzuEjYzCW89TD1eF5lPBgLloatPzpiw+NjGIA==;EndpointSuffix=core.windows.net";

        /// <summary>
        /// Uploads a file to the Azure Blob Storage Account.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileBytes"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public async Task<string> UploadFile(string fileName, byte[] fileBytes, int acctId)
        {
            var container = await GetContainer(acctId);
            BlobClient blob = container.GetBlobClient(fileName);
            var uri = blob.Uri.AbsoluteUri;

            using (var ms = new MemoryStream(fileBytes, false))
            {
                await blob.UploadAsync(ms, true);
            }

            return uri;
        }

        /// <summary>
        /// Gets the Azure Blob Container by acctId.
        /// </summary>
        /// <param name="acctId"></param>
        /// <returns></returns>
        private async Task<BlobContainerClient> GetContainer(int acctId)
        {
            var blobServiceClient = new BlobServiceClient(STORAGE_CONN_STRING);
            var container = blobServiceClient.GetBlobContainerClient(acctId.ToString());
            var exists = container.Exists();

            if (!exists)
            {
                await container.CreateIfNotExistsAsync();
                await container.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            }

            return container;
        }
    }
}
