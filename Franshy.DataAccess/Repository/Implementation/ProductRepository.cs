using Franshy.DataAccess.Data;
using Franshy.DataAccess.Repository.Interfaces;
using Franshy.Entities.Models;

namespace Franshy.DataAccess.Repository.Implementation
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
        public async Task Update(Product product) => context.Update(product);

    }
}
