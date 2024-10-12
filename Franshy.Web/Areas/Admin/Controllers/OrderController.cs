using Franshy.Entities.Models;
using Franshy.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Franshy.Utilities;
using Franshy.Entities.ViewModels;
using Stripe;


namespace Franshy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        [BindProperty]
        public OrderVm ordervm { get; set; }
        public OrderController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task<IActionResult> Index()
        {

            return View("Index");
        }
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            try
            {
                IEnumerable<OrderHeader> orderheaders = await _unitofwork.OrderHeader.GetAllAsync(Includes: new string[] { "ApplicationUser" });
                var orderheadersData = orderheaders.Select(p => new
                {
                    name = p.Name,
                    address = p.Address,
                    phone = p.Phone,
                    city = p.City,
                    email = p.ApplicationUser.Email,
                    orderstatus = p.OrderStatus,
                    totalprice = p.TotalPrice.ToString("0") + " EGP",
                    actions = $"<a href='/Admin/Order/Details/{p.Id}' class='btn btn-info'>Details</a> "
                });
                return Json(new { data = orderheadersData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            ordervm = new OrderVm()
            {
                orderheader = await _unitofwork.OrderHeader.GetByIdAsync(o => o.Id == id, Includes: new string[] { "ApplicationUser" }),
                orderdetails = await _unitofwork.OrderDetail.GetAllAsync(o => o.OrderHeaderId == id, Includes: new string[] { "Product" })

            };
            return View("OrderDetails", ordervm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateOrderDetails(OrderVm ordervm)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var orderfromdb = await _unitofwork.OrderHeader.GetByIdAsync(o => o.Id == ordervm.orderheader.Id);
                    orderfromdb.Name = ordervm.orderheader.Name;
                    orderfromdb.Phone = ordervm.orderheader.Phone;
                    orderfromdb.Address = ordervm.orderheader.Address;
                    orderfromdb.City = ordervm.orderheader.City;
                    orderfromdb.TrackingNumber = ordervm.orderheader.TrackingNumber;
                    await _unitofwork.OrderHeader.Update(orderfromdb);
                    await _unitofwork.complete();
                    TempData["Update"] = "Order has been Updated successfully";
                    return RedirectToAction("Details", "Order", new { ordervm.orderheader.Id });
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Internal Server Error";
                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Index");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StartProcess(int id)
        {

            try
            {
                await _unitofwork.OrderHeader.UpdateOrderStatus(id, Roles.Processing, null);
                await _unitofwork.complete();
                TempData["Create"] = "Order has been Processed successfully";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["error"] = "Internal Server Error";
                return RedirectToAction("Details", "Order", new { id });

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StartShipping(OrderVm Ordervm)
        {

            try
            {
                OrderHeader orderheader = await _unitofwork.OrderHeader.GetByIdAsync(o => o.Id == Ordervm.orderheader.Id);
                orderheader.OrderStatus = Roles.Shipped;
                orderheader.TrackingNumber = ordervm.orderheader.TrackingNumber;
                orderheader.ShippingDate = DateTime.Now;
                await _unitofwork.complete();
                TempData["Create"] = "Order has been Shipped successfully";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                TempData["error"] = "Internal Server Error";
                return RedirectToAction("Details", "Order", new { Ordervm.orderheader.Id });

            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CancelOrder(int id)
        {

            try
            {
                OrderHeader orderheader = await _unitofwork.OrderHeader.GetByIdAsync(o => o.Id == id);
                if (orderheader.OrderStatus == Roles.Approved || orderheader.OrderStatus == Roles.Processing)
                {
                    var options = new RefundCreateOptions()
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderheader.PaymentIntentId,

                    };
                    var service = new RefundService();
                    var refund = await service.CreateAsync(options);
                    await _unitofwork.OrderHeader.UpdateOrderStatus(orderheader.Id, Roles.Cancelled, Roles.Refund);

                }
                else
                {
                    await _unitofwork.OrderHeader.UpdateOrderStatus(orderheader.Id, Roles.Cancelled, Roles.Cancelled);
                }
                await _unitofwork.complete();
                TempData["Create"] = "Order has been Cancelled successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Internal Server Error";
                return RedirectToAction("Details", "Order", new { id });

            }
        }



    }
}
