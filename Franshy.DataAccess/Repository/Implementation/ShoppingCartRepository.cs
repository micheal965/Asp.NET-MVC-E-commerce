using Franshy.DataAccess.Data;
using Franshy.DataAccess.Repository.Interfaces;
using Franshy.Entities.Models;
using Franshy.Entities.ViewModels;

namespace Franshy.DataAccess.Repository.Implementation
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext context;
        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<int> DecreaseCount(ShoppingCart shoppingCart, int Count)
        {
            shoppingCart.count -= Count;
            return shoppingCart.count;

        }

        public async Task<int> IncreaseCount(ShoppingCart shoppingCart, int Count)
        {
            shoppingCart.count += Count;
            return shoppingCart.count;
        }
    }
}
