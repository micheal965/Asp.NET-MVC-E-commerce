using Franshy.DataAccess.Data;
using Franshy.DataAccess.Repository.Interfaces;
using Franshy.Entities.Models;
using Franshy.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Franshy.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task Initialize()
        {
            //Migration
            try
            {
                await _context.Database.EnsureCreatedAsync();
                var result = await _context.Database.GetPendingMigrationsAsync();
                if (result.Count() > 0)
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            //Roles
            if (!await _roleManager.RoleExistsAsync(Roles.AdminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.AdminRole));
                await _roleManager.CreateAsync(new IdentityRole(Roles.CustomerRole));
                //User
                ApplicationUser user = new ApplicationUser
                {
                    Name = "Admin",
                    UserName = "Admin@Franshy.com",
                    Email = "Admin@Franshy.com",
                    PhoneNumber = "PhoneNumber",
                    Address = "Address",
                };
                await _userManager.CreateAsync(user, "P@ssw0rd");
                await _userManager.AddToRoleAsync(user, Roles.AdminRole);
            }
            return;
        }
    }
}