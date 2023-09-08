using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Helpers;
using Biblioteca.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Data
{
    public class LendRepository : GenericRepository<Lend> , ILendRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public LendRepository(DataContext context, IUserHelper userHelper) : base(context) 
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task AddItemToLendAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var book = await _context.Books.FindAsync(model.BookId);
            if (book == null)
            {
                return;
            }

            var lendDetailTemp = await _context.LendsDetailTemp
                .Where(odt => odt.User == user && odt.Book == book)
                .FirstOrDefaultAsync();

            if (lendDetailTemp == null)
            {
                lendDetailTemp = new LendDetailTemp
                {
                    Book = book,
                    LendDate = model.LendDate,
                    User = user
                };
                _context.LendsDetailTemp.Add(lendDetailTemp);
            }
            

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmLendAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return false;
            }

            var lendTmps = await _context.LendsDetailTemp
                .Include(o => o.Book)
                .Where(o => o.User == user)
                .ToListAsync();

            if (lendTmps == null || lendTmps.Count == 0)
            {
                return false;
            }

            var details = lendTmps.Select(o => new LendDetail
            {
                Book = o.Book,
                LendDate = o.LendDate
            }).ToList();

            var lend = new Lend
            { 
                LendDate = DateTime.UtcNow,
                User = user,
                Items = details
            };

            await CreateAsync(lend);
            _context.LendsDetailTemp.RemoveRange(lendTmps);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var orderDetailTemp = await _context.LendsDetailTemp.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            _context.LendsDetailTemp.Remove(orderDetailTemp);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<LendDetailTemp>> GetDetailTempAsync(string userName)
        {
            var user =await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }
            return _context.LendsDetailTemp
                .Include(b => b.Book)
                .Where(o => o.User == user)
                .OrderBy(o => o.Book.Title);
        }

        public async Task<IQueryable<Lend>> GetLendAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Lends
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(b => b.Book)
                    .OrderByDescending(o => o.LendDate);
            }

            return _context.Lends
                .Include(o => o.Items)
                .ThenInclude(b => b.Book)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.LendDate);
        }

    }
}
