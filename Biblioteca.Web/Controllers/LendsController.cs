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

namespace Biblioteca.Web.Controllers
{
    [Authorize]
    public class LendsController : Controller
    {
        private readonly ILendRepository _lendRepository;
        private readonly IBookRepository _bookRepository;
        private readonly DataContext _context;

        public LendsController(ILendRepository lendRepository,
            IBookRepository bookRepository,
            DataContext context)
        {
            _lendRepository = lendRepository;
            _bookRepository = bookRepository;
            _context = context;
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
                Books = _bookRepository.GetComboBooks()
            };

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

                await _lendRepository.AddItemToLendAsync(model, this.User.Identity.Name);
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
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
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


    }
}
