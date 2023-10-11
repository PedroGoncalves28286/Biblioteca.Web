using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Data
{
    public interface ICityRepository : IGenericRepository<City>
    {
        IQueryable GetCitiesWithLibraries();

        Task<City> GetCityWithLibrariesAsync(int id);

        Task<Library> GetLibraryAsync(int id);

        Task AddLibraryAsync(LibraryViewModel model);

        Task<int> UpdateLibraryAsync(Library library);

        Task<int> DeleteLibraryAsync(Library library);

        IEnumerable<SelectListItem> GetComboCities();

        IEnumerable<SelectListItem> GetComboLibraries(int cityId);

        Task<City> GetCityAsync(Library library);
    }
}
