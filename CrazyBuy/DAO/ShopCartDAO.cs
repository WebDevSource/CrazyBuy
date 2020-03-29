using CrazyBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBuy.DAO
{
    public class ShopCartDAO : CrazyBuyRerpository
    {
        public ShopCartPrd getItem(Guid id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" SELECT s.*,p.name,p.prdImages,p.summary FROM [ShopCart] s ";
                sql += @" left join TenantPrd p on p.id = s.productId ";
                sql += @" WHERE s.id = '{0}' ";
                var query = String.Format(sql, id);
                return dbContext.Database.SqlQuery<ShopCartPrd>(query).FirstOrDefault();
            }
        }

        public void addItem(ShopCart item)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.ShopCart.Add(item);
                dbContext.SaveChanges();
            }
        }

        public void removeItem(Guid id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" DELETE FROM [ShopCart] WHERE id = '{0}' ";
                var query = String.Format(sql, id);
                dbContext.Database.ExecuteSqlCommand(query);
            }
        }

        public void updateItem(ShopCart item)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                ShopCart model = dbContext.ShopCart.Where(
                m => m.id == item.id).SingleOrDefault();
                if (model != null)
                {
                    dbContext.Entry(model).CurrentValues.SetValues(item);
                    dbContext.SaveChanges();
                }
            }
        }

        public List<ShopCartPrd> getItemsByMember(int memberId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" SELECT s.*,p.name,p.prdImages,p.summary,p.SpecialRule, p.shipType , p.stockNum FROM [ShopCart] s ";
                sql += @" left join TenantPrd p on p.id = s.productId ";
                sql += @" WHERE s.memberId = '{0}' ";
                var query = String.Format(sql, memberId);
                return dbContext.Database.SqlQuery<ShopCartPrd>(query).ToList();
            }
        }

        public void removeItemsByMember(int memberId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" DELETE FROM [ShopCart] WHERE memberId = '{0}' ";
                var query = String.Format(sql, memberId);
                dbContext.Database.ExecuteSqlCommand(query);
            }
        }
    }
}
