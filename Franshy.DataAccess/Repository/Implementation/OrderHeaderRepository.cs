using Franshy.DataAccess.Data;
using Franshy.DataAccess.Repository.Interfaces;
using Franshy.Entities.Models;

namespace Franshy.DataAccess.Repository.Implementation
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task Update(OrderHeader OrderHeader) => context.Update(OrderHeader);

        public async Task UpdateOrderStatus(int id, string? OrderStatus, string? PaymentStatus)
        {
            OrderHeader OrderFromDb = context.OrderHeaders.FirstOrDefault(o => o.Id == id);
            if (OrderFromDb != null)
            {
                if (OrderStatus != null)
                    OrderFromDb.OrderStatus = OrderStatus;

                if (PaymentStatus != null)
                    OrderFromDb.PaymentStatus = PaymentStatus;

            }


        }
    }
}
