using Franshy.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Franshy.DataAccess.Repository.Interfaces
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        Task Update(OrderDetail orderDetail);
    }
}
