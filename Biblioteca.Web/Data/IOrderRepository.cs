using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Models;
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


        Task AddItemToOrderAsync(AddItemViewModel model, string userName);


        Task ModifyOrderDetailTempQuantityAsync(int id, int quantity);


        Task DeleteDetailTempAsync(int id);


        Task<bool> ConfirmOrderAsync(string userName);
    }
}
