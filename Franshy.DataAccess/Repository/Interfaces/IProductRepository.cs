using Franshy.Entities.Models;
namespace Franshy.DataAccess.Repository.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task Update(Product product);

    }
}
