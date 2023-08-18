﻿using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Data
{
    public class OrderRepository : GenericRepository<Order> , IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context) 
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IQueryable<Order>> GetOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(b => b.Book)
                    .OrderByDescending(o => o.OrderDate);
            }

            return _context.Orders
                .Include(o => o.Items)
                .ThenInclude(b => b.Book)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }
    }
}
