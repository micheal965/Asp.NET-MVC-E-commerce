using Franshy.Entities.Models;
using Franshy.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Franshy.Entities.ViewModels;
using Franshy.DataAccess.Repository.Implementation;
using Microsoft.AspNetCore.Authorization;
using Franshy.Utilities;



namespace Franshy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.AdminRole)]
    public class DashBoardController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment webHostEnvironment;
        Product product;
        ProductVm productvm;
        public DashBoardController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitofwork)
        {
            this.webHostEnvironment = webHostEnvironment;
            _unitofwork = unitofwork;
        }
        public async Task<IActionResult> Index()
        {
            var ordercount = await _unitofwork.OrderHeader.GetAllAsync();
            var usercount = await _unitofwork.User.GetAllAsync();
            var productcount = await _unitofwork.Product.GetAllAsync();
            var approvedcount = await _unitofwork.OrderHeader.GetAllAsync(o => o.OrderStatus == Roles.Approved);

            DashBoardVm dashBoardVm = new DashBoardVm()
            {

                Orders = ordercount.Count(),
                Users = usercount.Count(),
                Products = productcount.Count(),
                ApprovedOrders = approvedcount.Count(),
            };
            return View("Index", dashBoardVm);
        }

    }
}
