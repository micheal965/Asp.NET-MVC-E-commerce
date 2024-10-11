
using Franshy.DataAccess.Data;
using Franshy.DataAccess.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Franshy.DataAccess.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private IDbContextTransaction _transaction;
        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IApplicationUserRepository User { get; private set; }



        public UnitOfWork(ApplicationDbContext context,
                           ICategoryRepository category,
                           IProductRepository product,
                           IShoppingCartRepository shoppingCart,
                           IOrderHeaderRepository orderheader,
                           IOrderDetailRepository orderdetail,
                           IApplicationUserRepository User)
        {
            this.context = context;
            Category = category;
            Product = product;
            ShoppingCart = shoppingCart;
            this.OrderHeader = orderheader;
            OrderDetail = orderdetail;
            this.User = User;
        }
        public async Task<int> complete()
        {
            return await context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await context.DisposeAsync();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await context.Database.BeginTransactionAsync();
        }
    }
}
