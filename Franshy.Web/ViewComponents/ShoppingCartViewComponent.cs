using Franshy.DataAccess.Repository.Interfaces;
using Franshy.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Franshy.Web.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string SessionKey = Roles.SessionKey; // Use a constant for the session key

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                if (HttpContext.Session.GetInt32(SessionKey) != null)
                {
                    return View(HttpContext.Session.GetInt32(SessionKey));
                }
                else
                {
                    try
                    {
                        var count = await _unitOfWork.ShoppingCart.GetAllAsync(sh => sh.userId == claim.Value);
                        HttpContext.Session.SetInt32(SessionKey, count.ToList().Count);

                        return View(HttpContext.Session.GetInt32(SessionKey));
                    }
                    catch (Exception ex)
                    {
                        // Log the exception (consider using a logging framework)
                        // Return view with an appropriate error message
                        return View(0); // or handle appropriately
                    }
                }
            }
            else
            {
                HttpContext.Session.Remove(SessionKey);
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
