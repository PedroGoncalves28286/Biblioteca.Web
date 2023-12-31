﻿using System;
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
    public class MembersController : Controller
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUserHelper _userHelper;

        public MembersController(IMemberRepository memberRepository ,IUserHelper userHelper)
        {
            _memberRepository = memberRepository;
            _userHelper = userHelper;
        }

        // GET: Members
        [RoleAuthorization("Admin")]
        public  IActionResult Index()
        {
            return View(_memberRepository.GetAll().OrderBy(l => l.LastName));
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberRepository.GetByIdAsync(id.Value);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        [RoleAuthorization("Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RoleAuthorization("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member)
        {
            if (ModelState.IsValid)
            {
                member.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _memberRepository.CreateAsync(member);
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        // GET: Members/Edit/5
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberRepository.GetByIdAsync(id.Value);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RoleAuthorization("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Member member)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    member.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _memberRepository.UpdateAsync(member);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _memberRepository.ExistAsync (member.Id))
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
            return View(member);
        }

        // GET: Members/Delete/5
        [RoleAuthorization("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _memberRepository.GetByIdAsync(id.Value);
                
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [RoleAuthorization("Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);  
            await _memberRepository.DeleteAsync(member);    
            return RedirectToAction(nameof(Index));
        }

       
    }
}
