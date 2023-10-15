using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Biblioteca.Web.Helpers
{
    public class PDFBlobHelper : IPDFBlobHelper
    {
        private readonly CloudBlobClient _blobClient;

        public PDFBlobHelper(IConfiguration configuration)
        {
            string keys = configuration["Blob:ConnectionString"];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(keys);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task<Guid> UploadPDFBlobAsync(IFormFile file, string containerName)
        {
            Stream stream = file.OpenReadStream();
            return await UploadStreamAsync(stream, containerName, file.FileName);
        }

        private async Task<Guid> UploadStreamAsync(Stream stream, string containerName, string fileName)
        {
            Guid name = Guid.NewGuid();
            CloudBlobContainer container = _blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{name}_{fileName}");
            await blockBlob.UploadFromStreamAsync(stream);
            return name;
        }
    }
}
