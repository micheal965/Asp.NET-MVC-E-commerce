using Franshy.DataAccess.Data;
using Franshy.DataAccess.Repository.Interfaces;
using Franshy.Entities.Models;

namespace Franshy.DataAccess.Repository.Implementation
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext context;
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task Update(OrderDetail OrderDetail) => context.Update(OrderDetail);
    }
}
