﻿using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Data
{
    public interface ILendRepository : IGenericRepository<Lend>
    {
        Task<IQueryable<Lend>> GetLendAsync(string userName);

        Task<IQueryable<LendDetailTemp>> GetDetailTempAsync(string userName);

        Task AddItemToLendAsync(AddItemViewModel model, string userName);

        Task DeleteDetailTempAsync(int id);

        Task<bool> ConfirmLendAsync(string userName);

        Task<Lend> GetByIdWithDetailsAsync(int id);

        Task<IEnumerable<Lend>> GetLendingHistoryAsync(string userId);

        void Update(Lend lend);

        Lend GetById(int id);

        Task<bool> UserHasLentBookAsync(string userName, int bookId);

        Task<Lend> GetLendByIdAsync(int lendId);
    }

}
