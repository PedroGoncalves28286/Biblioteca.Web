﻿using System.Threading.Tasks;

namespace Biblioteca.Web.Helpers
{
    public interface IMailHelper
    {
        Response SendEmail(string to, string subject, string body);
    }
}
