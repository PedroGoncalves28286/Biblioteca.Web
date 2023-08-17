using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Biblioteca.Web.Helpers
{
    public class NotFoundViewResult : ViewResult
    {
        public NotFoundViewResult(string viewName)
        {
            ViewName = viewName;
            StatusCode = (int)HttpStatusCode.NotFound;

        } 
    }
}
