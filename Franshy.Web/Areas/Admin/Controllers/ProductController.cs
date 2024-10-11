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
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment webHostEnvironment;
        Product product;
        ProductVm productvm;
        public ProductController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitofwork)
        {
            this.webHostEnvironment = webHostEnvironment;
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
                string[] includes = { "Category" };
                IEnumerable<Product> products = await _unitofwork.Product.GetAllAsync(Includes: includes);
                var productData = products.Select(p => new
                {
                    name = p.Name,
                    description = p.Description,
                    Price = p.Price.ToString("0") + " EGP",
                    discount = p.InsteadOf.ToString("0") + " EGP",
                    imgurl = $"<img src=\"{p.ImgUrl}\" width=\"80px\" height=\"80px\">",
                    category = p.Category.Name,
                    actions = $"<a href='/Admin/Product/Edit/{p.Id}' class='btn btn-primary'>Edit</a> " +
                              $"<a onClick=DeleteItem(\"/Admin/Product/Delete/{p.Id}\")  class='btn btn-danger'>Delete</a>" // Fixed quotes for JavaScript
                });
                return Json(new { data = productData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            productvm = new ProductVm()
            {
                categories = await _unitofwork.Category.GetAllAsync()

            };
            return View("CreateProduct", productvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductVm productVm, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Uploadfile<Product> uploadfile = new Uploadfile<Product>(webHostEnvironment);
                    productVm.product.ImgUrl = await uploadfile.upload(productVm.product, file, "Product");
                    await _unitofwork.Product.AddAsync(productVm.product);
                    await _unitofwork.complete();
                    TempData["Create"] = "Product has been Added successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Internal Server Error!";
                    return RedirectToAction("Create");
                }
            }
            productvm = new ProductVm()
            {
                product = product,
                categories = await _unitofwork.Category.GetAllAsync()
            };
            return View("CreateProduct", productvm);

        }
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            product = await _unitofwork.Product.GetByIdAsync(c => c.Id == id);
            productvm = new ProductVm()
            {
                product = product,
                categories = await _unitofwork.Category.GetAllAsync()

            };
            return View("EditProduct", productvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveEdit(ProductVm productVm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (productVm.product.ImgUrl != null && file != null)
                    {
                        Uploadfile<Product> uploadfile = new Uploadfile<Product>(webHostEnvironment);
                        var oldimg = Path.Combine(webHostEnvironment.WebRootPath, productVm.product.ImgUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }
                        productVm.product.ImgUrl = await uploadfile.upload(productVm.product, file, "Product");
                    }

                    await _unitofwork.Product.Update(productVm.product);
                    await _unitofwork.complete();
                    TempData["Update"] = "Product has been Edited successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Delete"] = "Internal Server Error!";
                    return RedirectToAction("Create");
                }
            }
            productVm.categories = await _unitofwork.Category.GetAllAsync();
            return View("EditProduct", productVm);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                product = await _unitofwork.Product.GetByIdAsync(c => c.Id == id);
                var oldimg = Path.Combine(webHostEnvironment.WebRootPath, product.ImgUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldimg))
                {
                    System.IO.File.Delete(oldimg);
                }
                await _unitofwork.Product.Remove(product);
                await _unitofwork.complete();
                return Json(new { success = true, message = "Product has been Deleted successfully" });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error occured while Deleting product" });

            }

        }

    }
}
