using Franshy.Entities.Models;

namespace Franshy.DataAccess.Repository.Interfaces
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<List<ApplicationUser>> GetAllUsers(string? Id);
        Task<bool> Delete(string? Id);
        Task LockUnlock(string? Id);
    }
}
