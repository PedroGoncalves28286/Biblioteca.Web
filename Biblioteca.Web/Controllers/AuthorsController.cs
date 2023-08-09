using Biblioteca.Web.Data;
using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Helpers;
using Biblioteca.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public AuthorsController(IAuthorRepository authorRepository,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _authorRepository = authorRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Authors
        public IActionResult Index()
        {
            return View(_authorRepository.GetAll().OrderBy(l => l.LastName));
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _authorRepository.GetByIdAsync(id.Value);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path =string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                   

                    path = await _imageHelper.UploadImageAsyn(model.ImageFile,"authors");   
                }

                var author = _converterHelper.ToAuthor(model, path, true);

                await _authorRepository.CreateAsync(author);
                return RedirectToAction(nameof(Index));

            }
            return View(model);
        }

       

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _authorRepository.GetByIdAsync(id.Value);
            if (author == null)
            {
                return NotFound();
            }
            var model = _converterHelper.ToAuthorViewModel(author);
            model = _converterHelper.ToAuthorViewModel(author);
            return View(model);
        }

        

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorViewModel model)
        {
            

            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.AuthorImage;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                       

                        path = await _imageHelper.UploadImageAsyn(model.ImageFile,"authors");
                    }
                    var author= _converterHelper.ToAuthor(model,path, false);
                    await _authorRepository.UpdateAsync(author);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _authorRepository.ExistAsync(model.Id))
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

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _authorRepository.GetByIdAsync(id.Value);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            await _authorRepository.DeleteAsync(author);
            return RedirectToAction(nameof(Index));
        }

    }
}
