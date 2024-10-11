using Franshy.DataAccess.Data;
using Franshy.DataAccess.Repository.Interfaces;
using Franshy.Entities.Models;
using Franshy.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Franshy.DataAccess.Repository.Implementation
{
    [Authorize(Roles = Roles.AdminRole)]
    public class ApplicationUserRepository : GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<bool> Delete(string? Id)
        {

            ApplicationUser Usertodelete = context.ApplicationUsers.FirstOrDefault(x => x.Id == Id);
            if (Usertodelete != null)
            {
                context.ApplicationUsers.Remove(Usertodelete);
                return true;
            }
            return false;
        }

        public async Task<List<ApplicationUser>> GetAllUsers(string? Id)
        {
            return context.ApplicationUsers.Where(u => u.Id != Id).ToList();
        }
        public async Task LockUnlock(string? Id)
        {
            ApplicationUser usertolockunlock = context.ApplicationUsers.FirstOrDefault(x => x.Id == Id);

            if (usertolockunlock.LockoutEnd == null || usertolockunlock.LockoutEnd < DateTime.Now)
            {
                usertolockunlock.LockoutEnd = DateTime.Now.AddYears(1);
            }
            else
            {
                usertolockunlock.LockoutEnd = DateTime.Now;
            }
        }


    }
}

