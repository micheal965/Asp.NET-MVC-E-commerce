using Franshy.Entities.Models;
namespace Franshy.DataAccess.Repository.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task Update(Category category);

    }
}
