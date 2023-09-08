using Biblioteca.Web.Data;
using Biblioteca.Web.Helpers;
using Biblioteca.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Biblioteca.Web.Data.Entities;

namespace Biblioteca.Web.Controllers
{
    [Authorize]
    public class LendsController : Controller
    {
        private readonly ILendRepository _lendRepository;
        private readonly IBookRepository _bookRepository;

        public LendsController(ILendRepository lendRepository,
            IBookRepository bookRepository)
        {
            _lendRepository = lendRepository;
            _bookRepository = bookRepository;
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
            if (ModelState.IsValid)
            {
                // Set the current date as the LendDate when adding the book
                model.LendDate = DateTime.Now;

                await _lendRepository.AddItemToLendAsync(model, this.User.Identity.Name);
                return RedirectToAction("Create");
            }

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

    }
}
