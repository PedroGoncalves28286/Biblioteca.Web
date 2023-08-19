using Biblioteca.Web.Data;
using Biblioteca.Web.Helpers;
using Biblioteca.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Biblioteca.Web.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBookRepository _bookRepository;

        public OrdersController(IOrderRepository orderRepository ,
            IBookRepository bookRepository)
        {
            _orderRepository = orderRepository;
            _bookRepository = bookRepository;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _orderRepository.GetOrderAsync(this.User.Identity.Name);
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            var model = await _orderRepository.GetDetailTempAsync(this.User.Identity.Name);
            return View(model);
        }
        public IActionResult AddBook()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                Books = _bookRepository.GetComboBooks()
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddBook(AddItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);
                return RedirectToAction("Create");
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _orderRepository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Increase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value,1);
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Decrease(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1);
            return RedirectToAction("Create");
        }
    }
}
