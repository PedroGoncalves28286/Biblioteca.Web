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
using Microsoft.AspNetCore.Authorization;

namespace Biblioteca.Web.Controllers
{
    public class RentalsController : Controller
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public RentalsController(IRentalRepository rentalRepository, IUserHelper userHelper ,
            IConverterHelper converterHelper,
             IBlobHelper blobHelper)
        {
            _rentalRepository = rentalRepository;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
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
                return new NotFoundViewResult("ProductNotFound");
            }

            var rental = await _rentalRepository.GetByIdAsync(id.Value);
            if (rental == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(rental);
        }

        // GET: Rentals/Create
        [RoleAuthorization("Admin", "Staff")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RoleAuthorization("Admin", "Staff")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid coverId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {

                    coverId = await _blobHelper.UploadBlobAsync(model.ImageFile, "covers");
                    
                }

                var rental = _converterHelper.ToRental(model,coverId, true);

                rental.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                await _rentalRepository.CreateAsync(rental);

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

            var rental = await _rentalRepository.GetByIdAsync(id.Value);
            if (rental == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var model = _converterHelper.ToRentalViewModel(rental);
            

            return View(model);
        }




        // POST: Rentals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RoleAuthorization("Staff", "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RentalViewModel model)
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

                   var rental= _converterHelper.ToRental(model,coverId, false);

                    rental.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
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
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var rental = await _rentalRepository.GetByIdAsync(id.Value);
            if (rental == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(rental);
        }

        // POST: Rentals/Delete/5
        [RoleAuthorization("Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);
            await _rentalRepository.DeleteAsync(rental);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ProductNotFound()
        {
            return View();
        }
    }
}

