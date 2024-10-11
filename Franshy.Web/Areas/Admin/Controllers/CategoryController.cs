using Franshy.Entities.Models;
using Franshy.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Franshy.Utilities;

namespace Franshy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        Category category;
        public CategoryController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public async Task<IActionResult> Index()
        {

            return View("Index");
        }
        public async Task<IActionResult> GetData()
        {
            IEnumerable<Category> categories = await _unitofwork.Category.GetAllAsync();
            var categoriesdata = categories.Select(c => new
            {
                name = c.Name,
                actions = $"<a href='/Admin/Category/Edit/{c.Id}' class='btn btn-primary'>Edit</a> " +
                          $"<a onClick=DeleteItem(\"/Admin/Category/Delete/{c.Id}\")  class='btn btn-danger'>Delete</a>"

            });
            return Json(new { data = categoriesdata });
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View("CreateCategory");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _unitofwork.Category.AddAsync(category);
                    await _unitofwork.complete();
                    TempData["Create"] = "Category has been Added successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Internal Server Error!";
                    return View("CreateCategory", category);
                }
            }
            return View("CreateCategory", category);
        }
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            category = await _unitofwork.Category.GetByIdAsync(c => c.Id == id);
            return View("EditCategory", category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _unitofwork.Category.Update(category);
                    await _unitofwork.complete();
                    TempData["Update"] = "Category has been Edited successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Delete"] = "Internal Server Error!";
                    return View("EditCategory", category);
                }
            }
            return View("EditCategory", category);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                category = await _unitofwork.Category.GetByIdAsync(c => c.Id == id);
                await _unitofwork.Category.Remove(category);
                await _unitofwork.complete();
                return Json(new { success = true, message = "Category has been Deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error Occured while Deleting Category" });
            }
        }
    }
}
