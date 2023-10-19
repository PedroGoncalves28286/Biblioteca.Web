using Biblioteca.Web.Data.Entities;
using Biblioteca.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Crypto.Digests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Web.Data
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly DataContext _context;

        public CityRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddLibraryAsync(LibraryViewModel model)
        {
            var city = await this.GetCityWithLibrariesAsync(model.CityId);
            if (city == null)
            {
                return;
            }

            city.Libraries.Add(new Library { Name = model.Name });
            _context.Cities.Update(city);
            await _context.SaveChangesAsync();
        }  
        
        public async Task<int> DeleteLibraryAsync(Library library)
        {
            var city = await _context.Cities
                .Where(l => l.Libraries.Any(li => li.Id == library.Id))
                .FirstOrDefaultAsync();
            if (city == null)
            {
                return 0;
            }

            _context.Libraries.Remove(library);
            await _context.SaveChangesAsync();
            return city.Id;
        }

        public IQueryable GetCitiesWithLibraries()
        {
            return _context.Cities
                .Include(l => l.Libraries)
                .OrderBy(l => l.Name);
        }

        public async Task<City> GetCityWithLibrariesAsync(int id)
        {
            return await _context.Cities
                .Include(l => l.Libraries)
                .Where(l => l.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateLibraryAsync(Library library)
        {
            var city = await _context.Cities
                .Where(l => l.Libraries.Any(li => li.Id == library.Id))
                .FirstOrDefaultAsync();
            if (city == null)
            {
                return 0;
            }

            _context.Libraries.Update(library);
            await _context.SaveChangesAsync();
            return city.Id;
        }

        public async Task<Library> GetLibraryAsync(int id)
        {
            return await _context.Libraries.FindAsync(id);
        }

        public async Task<City> GetCityAsync(Library library)
        {
            return await _context.Cities
                .Where(l => l.Libraries.Any(li => li.Id == library.Id))
                .FirstOrDefaultAsync();
        }

        public IEnumerable<SelectListItem> GetComboCities()
        {
            var list = _context.Cities.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()

            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a city...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboLibraries(int cityId)
        {
            var city = _context.Cities.Find(cityId);
            var list = new List<SelectListItem>();
            if (city != null)
            {
                list = _context.Libraries.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()

                }).OrderBy(l => l.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = "(Select a library...)",
                    Value = "0"
                });
            }
            
            return list;
        }


    }
}
