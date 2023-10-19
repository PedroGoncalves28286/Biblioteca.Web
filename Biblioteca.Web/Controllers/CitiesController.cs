using Biblioteca.Web.Data;
using Biblioteca.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Biblioteca.Web.Models;
using Microsoft.WindowsAzure.Storage;

namespace Biblioteca.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CitiesController : Controller
    {
        private readonly ICityRepository _cityRepository;

        public CitiesController(
            ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IActionResult> DeleteLibrary(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _cityRepository.GetLibraryAsync(id.Value);
            if (library == null)
            {
                return NotFound();
            }

            var cityId = await _cityRepository.DeleteLibraryAsync(library);
            return this.RedirectToAction($"Details", new { id = cityId });
        }

        public async Task<IActionResult> EditLibrary(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _cityRepository.GetLibraryAsync(id.Value);
            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }

        [HttpPost]
        public async Task<IActionResult> EditLibrary(Library library)
        {
            if (this.ModelState.IsValid)
            {
                var cityId = await _cityRepository.UpdateLibraryAsync(library);
                if (cityId != 0)
                {
                    return this.RedirectToAction($"Details", new { id = cityId });
                }
            }

            return this.View(library);
        }

        public async Task<IActionResult> AddLibrary(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _cityRepository.GetByIdAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            var model = new LibraryViewModel { CityId = city.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddLibrary(LibraryViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await _cityRepository.AddLibraryAsync(model);
                return RedirectToAction("Details", new { id = model.CityId });
            }

            return View(model);
        }

        public IActionResult Index()
        {
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            return View(_cityRepository.GetCitiesWithLibraries());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _cityRepository.GetCityWithLibrariesAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(City city)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _cityRepository.CreateAsync(city);
                    TempData["SuccessMessage"] = "City created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "This city already exists!.";
                    //_flashMessage.Danger("This city already exists!");
                }

                return View(city);
            }

            return View(city);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _cityRepository.GetByIdAsync(id.Value);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(City city)
        {
            if (ModelState.IsValid)
            {
                await _cityRepository.UpdateAsync(city);
                return RedirectToAction(nameof(Index));
            }

            return View(city);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var city = await _cityRepository.GetCityWithLibrariesAsync(id.Value);
            if (city.Libraries.Count > 0) // To avoid cascade delete
            {
                //this.ViewBag.ErrorTitle = "Error";
                //this.ViewBag.ErrorMessage = "In order to delete this city, you must first delete the libraries within it.";
                return RedirectToAction("CascadeError", "Errors");
            }

            await _cityRepository.DeleteAsync(city);
            return RedirectToAction(nameof(Index));
        }
    }
}
