using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace Biblioteca.Web.Helpers
{
    public interface IPDFBlobHelper
    {
        Task<Guid> UploadPDFBlobAsync(IFormFile file, string containerName);
    }
}
