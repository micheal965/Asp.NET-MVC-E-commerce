using Franshy.Entities.Models;
using Franshy.Entities.ViewModels;
namespace Franshy.DataAccess.Repository.Interfaces
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {

        Task<int> IncreaseCount(ShoppingCart shoppingCart, int Count);
        Task<int> DecreaseCount(ShoppingCart shoppingCart, int Count);
    }
}
