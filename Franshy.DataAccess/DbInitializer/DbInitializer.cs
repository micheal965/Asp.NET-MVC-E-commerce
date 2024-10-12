using Franshy.DataAccess.Data;
using Franshy.DataAccess.Repository.Implementation;
using Franshy.DataAccess.Repository.Interfaces;
using Franshy.Entities.Models;
using Franshy.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franshy.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly IUnitOfWork unitOfwork;
        private readonly ApplicationDbContext _context;

        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> _rolemanager, IUnitOfWork unitofwork, ApplicationDbContext context)
        {
            _userManager = userManager;
            this._rolemanager = _rolemanager;
            unitOfwork = unitofwork;
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
            if (!_rolemanager.RoleExistsAsync(Roles.AdminRole).GetAwaiter().GetResult())
            {
                _rolemanager.CreateAsync(new IdentityRole(Roles.AdminRole)).GetAwaiter().GetResult();
                _rolemanager.CreateAsync(new IdentityRole(Roles.CustomerRole)).GetAwaiter().GetResult();
                //User
                ApplicationUser user = new ApplicationUser
                {
                    Name = "Admin",
                    UserName = "Admin@Franshy.com",
                    Email = "Admin@Franshy.com",
                    PhoneNumber = "PhoneNumber",
                    Address = "Address",
                };
                _userManager.CreateAsync(user, "P@ssw0rd").GetAwaiter().GetResult();
                await _userManager.AddToRoleAsync(user, Roles.AdminRole);
            }
            return;
        }
    }
}