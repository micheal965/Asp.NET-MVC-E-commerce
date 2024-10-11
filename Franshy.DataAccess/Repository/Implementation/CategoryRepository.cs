using Franshy.DataAccess.Data;
using Franshy.DataAccess.Repository.Interfaces;
using Franshy.Entities.Models;

namespace Franshy.DataAccess.Repository.Implementation
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task Update(Category category) => context.Update(category);

    }
}
