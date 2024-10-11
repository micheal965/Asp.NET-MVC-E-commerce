namespace Franshy.Entities.ViewModels
{
    public class ShoppingCartVm
    {
        public IEnumerable<ShoppingCart> CartLists { get; set; }
        public decimal? TotalPrice { get; set; }
        public short itemcount { get; set; }

    }
}
