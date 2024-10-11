using Franshy.DataAccess.Repository.Implementation;
using Franshy.DataAccess.Repository.Interfaces;
using Franshy.Entities.Models;
using Franshy.Entities.ViewModels;
using Franshy.Utilities;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace Franshy.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork UnitOfWork;
        public ShoppingCartVm shoppingcartVm { get; set; }
        public SummaryCartVm summarycartvm { get; set; }
        public OrderHeader orderHeader { get; set; }
        public ApplicationUser applicationuser { get; set; }
        public ShoppingCart shoppingcart { get; set; }

        public IEnumerable<ShoppingCart> shoppingcarts { get; set; }
        public CartController(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingcarts = await UnitOfWork.ShoppingCart.GetAllAsync(u => u.userId == claim.Value, Includes: new string[] { "product" });
            decimal totalprice = 0;
            short itemcount = 0;
            foreach (var shoppingcart in shoppingcarts)
            {
                totalprice += (shoppingcart.count * shoppingcart.product.Price);
                itemcount++;
            }
            shoppingcartVm = new ShoppingCartVm()
            {
                CartLists = shoppingcarts,
                TotalPrice = totalprice,
                itemcount = itemcount
            };
            return View("Index", shoppingcartVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Plus(int id)
        {
            shoppingcart = await UnitOfWork.ShoppingCart.GetByIdAsync(C => C.Id == id);
            await UnitOfWork.ShoppingCart.IncreaseCount(shoppingcart, 1);
            await UnitOfWork.complete();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Minus(int id)
        {
            shoppingcart = await UnitOfWork.ShoppingCart.GetByIdAsync(C => C.Id == id);
            if (shoppingcart.count == 1)
            {
                await UnitOfWork.ShoppingCart.Remove(shoppingcart);
                await UnitOfWork.complete();
                return RedirectToAction("Index");
            }
            else
            {
                await UnitOfWork.ShoppingCart.DecreaseCount(shoppingcart, 1);
                await UnitOfWork.complete();
            }
            return RedirectToAction("Index");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingcart = await UnitOfWork.ShoppingCart.GetByIdAsync(C => C.Id == id);
            await UnitOfWork.ShoppingCart.Remove(shoppingcart);
            await UnitOfWork.complete();

            var count = await UnitOfWork.ShoppingCart.GetAllAsync(sh => sh.userId == claim.Value);
            HttpContext.Session.SetInt32(Roles.SessionKey, count.ToList().Count);
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);

            orderHeader = new OrderHeader();
            shoppingcarts = await UnitOfWork.ShoppingCart.GetAllAsync(u => u.userId == claim.Value, Includes: new string[] { "user", "product" });
            applicationuser = await UnitOfWork.User.GetByIdAsync(u => u.Id == claim.Value);

            orderHeader.ApplicationUserId = applicationuser.Id;
            orderHeader.Name = applicationuser.Name;
            orderHeader.Address = applicationuser.Address;
            foreach (var item in shoppingcarts)
            {
                orderHeader.TotalPrice += (item.count * item.product.Price);
            }

            summarycartvm = new SummaryCartVm()
            {
                orderHeader = orderHeader,
                shoppingcarts = shoppingcarts,
            };
            return View("Summary", summarycartvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Summary(SummaryCartVm sumcartvm)
        {
            if (ModelState.IsValid)
            {
                var claimidentity = (ClaimsIdentity)User.Identity;
                var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
                using (var transaction = await UnitOfWork.BeginTransactionAsync()) // Start a transaction
                {
                    try
                    {
                        sumcartvm.orderHeader.OrderDate = DateTime.Now;
                        sumcartvm.orderHeader.OrderStatus = Roles.Pending;
                        sumcartvm.orderHeader.PaymentStatus = Roles.Pending;
                        await UnitOfWork.OrderHeader.AddAsync(sumcartvm.orderHeader);
                        await UnitOfWork.complete();
                        sumcartvm.shoppingcarts = await UnitOfWork.ShoppingCart.GetAllAsync(
                            u => u.userId == claim.Value,
                            Includes: new string[] { "user", "product" });

                        foreach (var item in sumcartvm.shoppingcarts)
                        {
                            OrderDetail orderDetail = new OrderDetail()
                            {
                                OrderHeaderId = sumcartvm.orderHeader.Id,
                                ProductId = item.productId,
                                Count = item.count,
                                Price = item.product.Price
                            };
                            await UnitOfWork.OrderDetail.AddAsync(orderDetail);
                        }
                        await UnitOfWork.complete();

                        var domain = "https://localhost:7254/";

                        var options = new SessionCreateOptions
                        {
                            LineItems = new List<SessionLineItemOptions>(),
                            Mode = "payment",
                            SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={sumcartvm.orderHeader.Id}",
                            CancelUrl = domain + $"Customer/Cart/Index",
                        };

                        foreach (var item in sumcartvm.shoppingcarts)
                        {
                            var sessionlineoption = new SessionLineItemOptions
                            {
                                PriceData = new SessionLineItemPriceDataOptions
                                {
                                    UnitAmount = (long)(item.product.Price * 100),
                                    Currency = "EGP",
                                    ProductData = new SessionLineItemPriceDataProductDataOptions
                                    {
                                        Name = item.product.Name,
                                    },
                                },
                                Quantity = item.count,
                            };
                            options.LineItems.Add(sessionlineoption);
                        }

                        var service = new SessionService();
                        Session session = service.Create(options);
                        sumcartvm.orderHeader.SessionId = session.Id;

                        await UnitOfWork.complete();
                        await transaction.CommitAsync();

                        Response.Headers.Add("Location", session.Url);
                        return new StatusCodeResult(303);
                    }
                    catch
                    {
                        await transaction.RollbackAsync(); // Rollback on error
                        TempData["error"] = "Internal Server Error!";
                    }
                }
            }
            return RedirectToAction("Summary");
        }

        public async Task<IActionResult> OrderConfirmation(int Id)
        {
            using (var transaction = await UnitOfWork.BeginTransactionAsync())
            {
                try
                {
                    orderHeader = await UnitOfWork.OrderHeader.GetByIdAsync(u => u.Id == Id);
                    var service = new SessionService();
                    Session session = service.Get(orderHeader.SessionId);

                    if (session.PaymentStatus.ToLower() == "paid")
                    {
                        await UnitOfWork.OrderHeader.UpdateOrderStatus(Id, Roles.Approved, Roles.Approved);
                        orderHeader.PaymentIntentId = session.PaymentIntentId;
                        orderHeader.PaymentDate = DateTime.Now;
                        await UnitOfWork.complete();
                    }

                    shoppingcarts = await UnitOfWork.ShoppingCart.GetAllAsync(u => u.userId == orderHeader.ApplicationUserId);
                    await UnitOfWork.ShoppingCart.RemoveRange(shoppingcarts);
                    await UnitOfWork.complete();

                    await transaction.CommitAsync(); // Commit transaction
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(); // Rollback on error
                    TempData["error"] = "Error While processing order!";
                    return RedirectToAction("Index");
                }
                return View("OrderConfirmation", orderHeader);
            }

        }

    }
}
