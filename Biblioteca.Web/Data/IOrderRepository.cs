﻿using Biblioteca.Web.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Data
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetOrderAsync(string userName);

        Task<IQueryable<OrderDetailTemp>> GetDetailTempAsync(string userName);

        
    }
}
