using Franshy.DataAccess.Repository.Interfaces;
using Franshy.Entities.ViewModels;
using Franshy.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Franshy.Utilities;

namespace Franshy.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            int pagesize = 8;
            IEnumerable<Product> Products = await UnitOfWork.Product.GetAllAsync();
            return View("index", Products);
        }

        public async Task<IActionResult> Details(int id)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                product = await UnitOfWork.Product.GetByIdAsync(p => p.Id == id),
                count = 1
            };
            return View("Details", shoppingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddtoCart(ShoppingCart shoppingCart)
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var userid = claim.Value;

            shoppingCart.userId = userid;

            ShoppingCart CartFromDb = await UnitOfWork.ShoppingCart.GetByIdAsync(c => c.userId == userid && c.productId == shoppingCart.productId);

            if (CartFromDb == null)
            {
                await UnitOfWork.ShoppingCart.AddAsync(shoppingCart);
                await UnitOfWork.complete();
                var count = await UnitOfWork.ShoppingCart.GetAllAsync(sh => sh.userId == userid);
                HttpContext.Session.SetInt32(Roles.SessionKey, count.ToList().Count);
            }
            else
            {
                await UnitOfWork.ShoppingCart.IncreaseCount(CartFromDb, shoppingCart.count);
            }

            return RedirectToAction("Index");
        }
    }
}
