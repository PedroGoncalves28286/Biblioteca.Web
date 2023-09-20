using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Web.Data;
using Biblioteca.Web.Data.Entities;
using MailKit;
using Biblioteca.Web.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Biblioteca.Web.Controllers
{
    public class ContactsController : Controller
    {
        private readonly DataContext _context;
        private readonly IMailHelper _mailHelper;

        public ContactsController(DataContext context,
            IMailHelper mailHelper)
        {
            _context = context;
            _mailHelper = mailHelper;
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                Response response = _mailHelper.SendEmail("caseiroinc@gmail.com", contact.Subject, "Name: " + contact.Name + 
                    "<br /><br />" + "Email: " + contact.Email + "<br /><br />" + "Phone Number: " +
                    contact.PhoneNumber + "<br /><br />" + "Message: " + contact.Message);
                if (response.IsSuccess)
                {
                    ViewBag.Message = "Your contact request has been recieved. We will get in touch with you as soon as possible.";
                }
                else
                {
                    ViewBag.Message = "Unfortunatly something went wrong. Please update the webpage and try again later.";
                }
                return View();
                
            }
            return RedirectToAction("Contact", "Contacts");
        }
    }
}
