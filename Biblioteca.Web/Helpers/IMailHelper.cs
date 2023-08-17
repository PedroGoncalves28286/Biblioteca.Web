using System.Threading.Tasks;

namespace Biblioteca.Web.Helpers
{
    public interface IMailHelper
    {
        Task<Response> SendEmail(string to, string subject, string body);
    }
}
