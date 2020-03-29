using CrazyBuy.Models;

namespace CrazyBuy.DAO
{
    public class OrderAmountHistoryDao : CrazyBuyRerpository
    {
        public int add(OrderAmountHistory history)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.OrderAmountHistory.Add(history);
                dbContext.SaveChanges();
                return history.id;
            }
        }
    }
}
