using Biblioteca.Web.Data;
using Biblioteca.Web.Helpers;
using Biblioteca.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public BooksController(IBookRepository bookRepository, IUserHelper userHelper,
            IConverterHelper converterHelper,
             IBlobHelper blobHelper)
        {
            _bookRepository = bookRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
        }

        // GET: Rentals
        public IActionResult Index()
        {
            return View(_bookRepository.GetAll().OrderBy(u => u.Borrower));
        }

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(book);
        }

        // GET: Rentals/Create
        [RoleAuthorization("Admin", "Staff", "Reader")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RoleAuthorization("Admin", "Staff", "Reader")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid coverId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {

                    coverId = await _blobHelper.UploadBlobAsync(model.ImageFile, "covers");

                }

                var book = _converterHelper.ToRental(model, coverId, true);

                book.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                await _bookRepository.CreateAsync(book);

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        // GET: Rentals/Edit/5
        [RoleAuthorization("Staff", "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var model = _converterHelper.ToRentalViewModel(book);


            return View(model);
        }




        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RoleAuthorization("Staff", "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid coverId = model.CoverId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        coverId = await _blobHelper.UploadBlobAsync(model.ImageFile, "covers");

                    }

                    var book = _converterHelper.ToRental(model, coverId, false);

                    book.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _bookRepository.UpdateAsync(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bookRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);


        }

        // GET: Rentals/Delete/5
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(book);
        }

        // POST: Rentals/Delete/5
        [RoleAuthorization("Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            await _bookRepository.DeleteAsync(book);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ProductNotFound()
        {
            return View();
        }
    }
}
