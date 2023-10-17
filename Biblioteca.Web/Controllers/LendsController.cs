using Biblioteca.Web.Data;
using Biblioteca.Web.Helpers;
using Biblioteca.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Biblioteca.Web.Controllers
{
    [Authorize]
    public class LendsController : Controller
    {
        private readonly ILendRepository _lendRepository;
        private readonly IBookRepository _bookRepository;
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMailHelper _mailHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LendsController(ILendRepository lendRepository,
            IBookRepository bookRepository,
            DataContext context, UserManager<User> userManager,
            IMailHelper mailHelper,
            IWebHostEnvironment webHostEnvironment)
        {
            _lendRepository = lendRepository;
            _bookRepository = bookRepository;
            _context = context;
            _userManager = userManager;
            _mailHelper = mailHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _lendRepository.GetLendAsync(this.User.Identity.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _lendRepository.GetDetailTempAsync(this.User.Identity.Name);
            return View(model);
        }

        public IActionResult AddBook()
        {
            var model = new AddItemViewModel
            {
                LendDate = System.DateTime.Now,
                DevolutionDate = DateTime.Now,
            };
            model.Books = _bookRepository.GetComboBooks();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(AddItemViewModel model)
        {
            // Check if the selected book is available
            var selectedBook = await _bookRepository.GetBookByIdAsync(model.BookId);

            if (ModelState.IsValid && selectedBook != null && selectedBook.IsAvailable)
            {
                // Set the current date as the LendDate when adding the book
                model.LendDate = DateTime.Now;

                // Calculate DevolutionDate based on your logic
                model.DevolutionDate = model.LendDate.AddDays(14); // 14 days after LendDate

                // Check if there are available copies
                if (selectedBook.AvailableCopies > 0)
                {
                    // Proceed with the lend
                    await _lendRepository.AddItemToLendAsync(model, this.User.Identity.Name);

                    // Decrease available copies by one
                    selectedBook.AvailableCopies--;

                    // Update IsAvailable property
                    selectedBook.IsAvailable = selectedBook.AvailableCopies > 0;
                }
                else
                {
                    // Handle the case where there are no available copies
                    ModelState.AddModelError(string.Empty, "No available copies of the selected book.");
                }

                // Update the book in the repository
                await _bookRepository.UpdateBookAsync(selectedBook);

                return RedirectToAction("Create");
            }

            // If the model state is not valid or the book is not available, return to the view.
            model.Books = _bookRepository.GetComboBooks();

            // The client-side code will handle displaying the error message.
            return View(model);
        }



        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _lendRepository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> ConfirmLend()
        {
            var response = await _lendRepository.ConfirmLendAsync(this.User.Identity.Name);

            if (response)
            {
                // Retrieve the user's full name from the User object
                string fullName = User.Identity is ClaimsIdentity identity
                    ? $"{identity.FindFirst("FirstName")?.Value} {identity.FindFirst("LastName")?.Value}"
                    : string.Empty;

                // Retrieve the lend details
                var lends = await _lendRepository.GetLendAsync(this.User.Identity.Name);

                if (lends != null && lends.Any())
                {
                    // Choose the specific lend you want to confirm, or handle multiple lends if needed
                    var lendToConfirm = lends.FirstOrDefault(); // Change this logic as needed

                    if (lendToConfirm != null)
                    {
                        // Retrieve the first lend detail associated with the lend
                        var lendDetail = lendToConfirm.Items.FirstOrDefault();

                        if (lendDetail != null)
                        {
                            // Retrieve the book associated with the lend detail
                            var book = lendDetail.Book;

                            // Generate the book details URL with the correct protocol (http or https)
                            var bookDetailsUrl = Url.Action("Details", "Books", new { id = book.Id }, protocol: HttpContext.Request.Scheme);

                            // Include the book details link in the email body
                            string subject = "Lend Confirmation";
                            string body = $"Your lend has been confirmed! <br/> The following are the details related to the lend: <br/>" +
                                $"Borrower: {fullName}<br/>" +
                                $"Book title: <a href=\"{bookDetailsUrl}\" style=\"color: blue; text-decoration: underline;\">{book.Title}</a><br/>" + // Include book title as a link
                                $"Author: {book.Author}<br/>" +
                                $"Genre: {book.GenreName}<br/>" +
                                $"Lend Date: {lendToConfirm.LendDate}<br/>" +
                                $"Devolution Date: {lendToConfirm.DevolutionDate}";

                            Response emailResponse = _mailHelper.SendEmail(this.User.Identity.Name, subject, body);

                            if (emailResponse.IsSuccess)
                            {
                                TempData["EmailConfirmationMessage"] = "Your lend has been confirmed! Please check your Email!";
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "Failed to send confirmation email.");
                            }
                        }

                        else
                        {
                            // Handle the case where the specific lend detail is not found
                            ModelState.AddModelError(string.Empty, "Lend detail not found.");
                        }
                    }
                    else
                    {
                        // Handle the case where the specific lend is not found
                        ModelState.AddModelError(string.Empty, "Lend not found.");
                    }
                }
                else
                {
                    // Handle the case where lends are not found for the user
                    ModelState.AddModelError(string.Empty, "No lends found for the user.");
                }

                return RedirectToAction("Index");
            }
            else
            {
                // Handle the case where lend confirmation failed, e.g., lend not found or other error.
                ModelState.AddModelError(string.Empty, "Failed to confirm lend.");
                return RedirectToAction("Create");
            }
        }

        // GET: Lends/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var lendToDelete = await _lendRepository.GetByIdWithDetailsAsync(id);

            if (lendToDelete == null)
            {
                return NotFound(); // Handle the case where the lend is not found
            }

            return View(lendToDelete);
        }

        // POST: Lends/DeleteConfirmed/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lendToDelete = await _lendRepository.GetByIdWithDetailsAsync(id);

            if (lendToDelete == null)
            {
                return NotFound();
            }

            // Perform the deletion logic here
            await _lendRepository.DeleteAsync(lendToDelete);

            return RedirectToAction("Index"); // Redirect to the list of lends after deletion
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var model = await _lendRepository.GetByIdWithDetailsAsync(id.Value);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ExtendLend(int id)
        {
            var lendToExtend = await _lendRepository.GetByIdAsync(id);

            if (lendToExtend == null)
            {
                return NotFound();
            }

            // Return a view with the lend information and a form to request an extension
            return View(lendToExtend);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExtendLend(int id, int extensionDays)
        {
            var lendToExtend = _lendRepository.GetById(id);

            if (lendToExtend == null)
            {
                return NotFound();
            }

            // If the first extension has not been done, allow it
            if (!lendToExtend.FirstExtensionDone)
            {
                // Calculate the new extended devolution date based on the current devolution date
                var extendedDevolutionDate = lendToExtend.DevolutionDate.AddDays(extensionDays);

                // Update the lend's devolution date
                lendToExtend.DevolutionDate = extendedDevolutionDate;

                // Set the FirstExtensionDone flag to true
                lendToExtend.FirstExtensionDone = true;

                // Save the updated lend information in your repository
                _lendRepository.Update(lendToExtend); // Make sure your repository saves changes to the database

                // Return the updated devolution date as a string
                return Content(extendedDevolutionDate.ToString("dd/MM/yyyy"));
            }
            else
            {
                // Return an error message indicating that further extensions are not allowed
                return Content("Error: Further extensions are not allowed.");
            }
        }

        // GET: Lends/History
        [Authorize]
        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound(); // User not found, return a 404 response
            }

            // Retrieve the user's lending history, for example, based on their user ID
            var lendingHistory = await _lendRepository.GetLendingHistoryAsync(user.Id);

            return View(lendingHistory);

        }

        private string GenerateConfirmationDocumentContent(Lend lend)
        {
            // Define CSS styles
            string cssStyles = @"
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        margin: 20px;
                    }
                    h1 {
                        background-color: #007BFF;
                        color: #FFF;
                        padding: 10px;
                        text-align: center;
                    }
                    ul {
                        list-style-type: none;
                        padding: 0;
                    }
                    li {
                        margin-bottom: 10px;
                    }
                    /* Add more styles as needed */
                </style>
            ";

            // Generate the HTML content with styles
            string htmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>Lend Confirmation</title>
                    {cssStyles}
                </head>
                <body>
                    <h1>Lend Confirmation</h1>
                    <p>Lend Details:</p>
                    <ul>
                        <li><strong>Borrower:</strong> {lend.User.UserName}</li>
                        <li><strong>Book Title:</strong> {lend.BookTitle}</li>
                        <li><strong>Lend Date:</strong> {lend.LendDate}</li>
                        <li><strong>Devolution Date:</strong> {lend.DevolutionDate}</li>
                        <!-- Add more details as needed -->
                    </ul>
                </body>
                </html>
            ";

            return htmlContent;
        }

        public IActionResult DownloadConfirmationDocument(int lendId)
        {
            // Retrieve the lend details based on the lendId
            var lend = _lendRepository.GetByIdWithDetailsAsync(lendId).Result;

            if (lend == null)
            {
                return NotFound(); // Handle the case where the lend is not found
            }

            // Generate the confirmation document content (e.g., HTML content)
            // You can use a library like RazorEngine to render a Razor view to HTML if needed.

            string documentContent = GenerateConfirmationDocumentContent(lend);

            // Convert the document content to bytes
            var documentBytes = Encoding.UTF8.GetBytes(documentContent);

            // Set the file name for the downloaded document
            string fileName = $"Lend_Confirmation_{lendId}.html";

            // Return the document as a downloadable file
            return File(documentBytes, "text/html", fileName);
        }
    }
}
