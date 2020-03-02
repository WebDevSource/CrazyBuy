using CrazyBuy.Models;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBuy.DAO
{
    public class OrderContactItemDAO : CrazyBuyRerpository
    {
        public void add(OrderContactItem data)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.OrderContactItem.Add(data);
                dbContext.SaveChanges();
            }
        }

        public List<OrderContactItem> getListByOrderId(int orderId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"SELECT * FROM [OrderContactItem] where orderId = {0} order by dtContact desc";
                var query = string.Format(sql, orderId);
                return dbContext.Database.SqlQuery<OrderContactItem>(query).ToList();
            }
        }
    }
}
