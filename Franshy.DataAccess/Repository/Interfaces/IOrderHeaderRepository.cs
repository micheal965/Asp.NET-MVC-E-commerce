using Franshy.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franshy.DataAccess.Repository.Interfaces
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        Task Update(OrderHeader OrderHeader);

        Task UpdateOrderStatus(int id, string? OrderStatus, string? PaymentStatus);
    }
}
