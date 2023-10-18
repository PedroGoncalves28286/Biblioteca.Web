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
    public class NewslettersController : Controller
    {
        private readonly INewsletterRepository _newsletterRepository;
        private readonly IUserHelper _userHelper;

        public NewslettersController(INewsletterRepository newsletterRepository,
            IUserHelper userHelper)
        {
            _newsletterRepository = newsletterRepository;
            _userHelper = userHelper;
        }

        // GET: Newsletters
        public IActionResult Index()
        {
            return View(_newsletterRepository.GetAll().OrderBy(n => n.Id));
        }

        // GET: Newsletters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsletter = await _newsletterRepository.GetByIdAsync(id.Value);
            if (newsletter == null)
            {
                return NotFound();
            }

            return View(newsletter);
        }

        // GET: Newsletters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Newsletters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Newsletter newsletter)
        {
            if (ModelState.IsValid)
            {
                newsletter.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _newsletterRepository.CreateAsync(newsletter);
                return RedirectToAction(nameof(Index));
            }
            return View(newsletter);
        }

        // GET: Newsletters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsletter = await _newsletterRepository.GetByIdAsync(id.Value);
            if (newsletter == null)
            {
                return NotFound();
            }
            return View(newsletter);
        }

        // POST: Newsletters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Newsletter newsletter)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    newsletter.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _newsletterRepository.UpdateAsync(newsletter);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _newsletterRepository.ExistAsync(newsletter.Id))
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
            return View(newsletter);
        }

        // GET: Newsletters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsletter = await _newsletterRepository.GetByIdAsync(id.Value);
            if (newsletter == null)
            {
                return NotFound();
            }

            return View(newsletter);
        }

        // POST: Newsletters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsletter = await _newsletterRepository.GetByIdAsync(id);
            await _newsletterRepository.DeleteAsync(newsletter);
            return RedirectToAction(nameof(Index));
        }

    }
}
