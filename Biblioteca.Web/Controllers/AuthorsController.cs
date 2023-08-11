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

        private readonly IConverterHelper _converterHelper;
        private readonly IBlobHelper _blobHelper;

        public AuthorsController(IAuthorRepository authorRepository,
            IConverterHelper converterHelper, IBlobHelper blobHelper)
        {
            _authorRepository = authorRepository;
            _converterHelper = converterHelper;
            _blobHelper = blobHelper;
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
                Guid authorImageId = Guid.Empty;   

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                   

                    authorImageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "authors");
                }

                var author = _converterHelper.ToAuthor(model,authorImageId,true);

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
                    Guid authorImageId = model.AuthorImageId;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {

                        authorImageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "authors");
                        
                    }
                    var author= _converterHelper.ToAuthor(model,authorImageId, false);
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
