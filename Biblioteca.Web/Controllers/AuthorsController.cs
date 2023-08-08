using Biblioteca.Web.Data;
using Biblioteca.Web.Data.Entities;
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

        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
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
                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\Images\\authors",
                        model.ImageFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/authors/{model.ImageFile.FileName}";
                }

                var author = this.ToAuthor(model, path);

                await _authorRepository.CreateAsync(author);
                return RedirectToAction(nameof(Index));

            }
            return View(model);
        }

        private Author ToAuthor(AuthorViewModel model, string path)
        {
            return new Author
            {
                Id = model.Id,
                AuthorImage = path,
                FirstName = model.FirstName,
                LastName = model.LastName,
              
            };
            
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
            var model = this.ToAuthorViewModel(author);
            return View(model);
        }

        private AuthorViewModel ToAuthorViewModel(Author author)
        {
            return new AuthorViewModel
            {
                Id = author.Id,
                AuthorImage = author.AuthorImage,   
                FirstName = author.FirstName,
                LastName = author.LastName,
            };

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
                    var path = string.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\Images\\authors",
                            model.ImageFile.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/authors/{model.ImageFile.FileName}";
                    }
                    var author=this.ToAuthor(model,path);
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
