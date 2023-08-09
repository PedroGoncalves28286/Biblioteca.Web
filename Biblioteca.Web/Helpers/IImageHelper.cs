using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Biblioteca.Web.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsyn(IFormFile imageFile, string folder);
    }
}
