using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Web.Data;
using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Helpers;
using System.IO;
using Biblioteca.Web.Models;

namespace Biblioteca.Web.Controllers
{
    public class RentalsController : Controller
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IUserHelper _userHelper;

        public RentalsController(IRentalRepository rentalRepository, IUserHelper userHelper)
        {
            _rentalRepository = rentalRepository;
            _userHelper = userHelper;
        }

        // GET: Rentals
        public IActionResult Index()
        {
            return View(_rentalRepository.GetAll().OrderBy(u => u.Borrower));
        }

        // GET: Rentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _rentalRepository.GetByIdAsync(id.Value);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // GET: Rentals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\Images\\covers",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/covers/{file}";
                }

                var rental = this.ToRental(model, path);

                rental.User = await _userHelper.GetUserByEmailAsync("pedro@gmail.com");
                await _rentalRepository.CreateAsync(rental);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private Rental ToRental(RentalViewModel model, string path)
        {
            return new Rental
            {
                Id = model.Id,
                Borrower = model.Borrower,
                Author = model.Author,
                Title = model.Title,
                BookId = model.BookId,
                ImageUrl = path,
                Availability = model.Availability,
                ISBN = model.ISBN,
                Publisher = model.Publisher,
                StartDate = model.StartDate,
                ScheduleReturnDate = model.ScheduleReturnDate,
                ActualReturnDate = model.ActualReturnDate,
                RentalDuration = model.RentalDuration,
                User = model.User,
            };
        }

        // GET: Rentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _rentalRepository.GetByIdAsync(id.Value);
            if (rental == null)
            {
                return NotFound();
            }

            var model = this.ToRentalViewModel(rental);

            return View(model);
        }

        private RentalViewModel ToRentalViewModel(Rental rental)
        {
            return new RentalViewModel
            {
                Id = rental.Id,
                Borrower = rental.Borrower,
                Author = rental.Author,
                Title = rental.Title,
                BookId = rental.BookId,
                ImageUrl = rental.ImageUrl,
                Availability = rental.Availability,
                ISBN = rental.ISBN,
                Publisher = rental.Publisher,
                StartDate = rental.StartDate,
                ScheduleReturnDate = rental.ScheduleReturnDate,
                ActualReturnDate = rental.ActualReturnDate,
                RentalDuration = rental.RentalDuration,
                User = rental.User,
            };

        }


        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RentalViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = string.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\Images\\covers",
                            model.ImageFile.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/covers/{model.ImageFile.FileName}";
                    }

                    var rental = this.ToRental(model, path);

                    rental.User = await _userHelper.GetUserByEmailAsync("pedro@gmail.com");
                    await _rentalRepository.UpdateAsync(rental);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _rentalRepository.ExistAsync(model.Id))
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rental = await _rentalRepository.GetByIdAsync(id.Value);
            if (rental == null)
            {
                return NotFound();
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);
            await _rentalRepository.DeleteAsync(rental);
            return RedirectToAction(nameof(Index));
        }
    }
}

