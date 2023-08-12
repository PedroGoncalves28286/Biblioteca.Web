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
    public class MembershipsController : Controller
    {

        private readonly IMembershipRepository _membershipRepository;
        private readonly IUserHelper _userHelper;

        public MembershipsController(IMembershipRepository membershipRepository ,IUserHelper userHelper)
        {
            _membershipRepository = membershipRepository;
            _userHelper = userHelper;
        }

        // GET: Memberships
        public  IActionResult Index()
        {
            return View (_membershipRepository.GetAll().OrderBy(n => n.Name));
        }

        // GET: Memberships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = _membershipRepository.GetByIdAsync(id.Value);
                
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // GET: Memberships/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Memberships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Membership membership)
        {
            if (ModelState.IsValid)
            {
                membership.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _membershipRepository.CreateAsync(membership);    
                return RedirectToAction(nameof(Index));
            }
            return View(membership);
        }

        // GET: Memberships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _membershipRepository.GetByIdAsync(id.Value);
            if (membership == null)
            {
                return NotFound();
            }
            return View(membership);
        }

        // POST: Memberships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Membership membership)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    membership.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _membershipRepository.UpdateAsync(membership);    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _membershipRepository.ExistAsync (membership.Id))
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
            return View(membership);
        }

        // GET: Memberships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membership = await _membershipRepository.GetByIdAsync (id.Value);

            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // POST: Memberships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var membership = await _membershipRepository.GetByIdAsync(id);
            await _membershipRepository.DeleteAsync(membership);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
