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
        public async Task<IActionResult> Create([Bind("Id,UserId,Author,Title,BookId,ImageUrl,Availability,ISBN,Publisher,StartDate,ScheduleReturnDate,ActualReturnDate,RentalDuration")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                rental.User = await _userHelper.GetUserByEmailAsync("pedro@gmail.com");
                await _rentalRepository.CreateAsync(rental);
                return RedirectToAction(nameof(Index));
            }
            return View(rental);
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
            return View(rental);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Author,Title,BookId,ImageUrl,Availability,ISBN,Publisher,StartDate,ScheduleReturnDate,ActualReturnDate,RentalDuration")] Rental rental)
        {
            if (id != rental.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    rental.User = await _userHelper.GetUserByEmailAsync("pedro@gmail.com");
                    await _rentalRepository.UpdateAsync(rental);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _rentalRepository.ExistAsync(rental.Id))
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
            return View(rental);
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
