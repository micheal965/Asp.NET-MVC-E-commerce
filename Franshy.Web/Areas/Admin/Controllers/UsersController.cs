using Franshy.DataAccess.Repository.Interfaces;
using Franshy.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Franshy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.AdminRole)]
    public class UsersController : Controller
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public UsersController(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }
        public async Task<IActionResult> Index()

        {
            var claimidentity = (ClaimsIdentity)User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            var UserId = claim.Value;
            return View("Index", await UnitOfWork.User.GetAllUsers(UserId));
        }
        public async Task<IActionResult> Delete(string? id)
        {
            // Attempt to delete the user
            bool result = await UnitOfWork.User.Delete(id);
            await UnitOfWork.complete();

            // Set success or error message in TempData
            if (result) // Assuming the delete method returns true on success
            {
                TempData["SuccessMessage"] = "User deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error while deleting the user.";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LockUnlock(string? Id)
        {
            await UnitOfWork.User.LockUnlock(Id);
            await UnitOfWork.complete();
            return RedirectToAction("Index");
        }
    }
}
