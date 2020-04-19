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

        public void deleteTimeOutItem(int memberId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" delete s from dbo.ShopCart s left join dbo.TenantPrd p ";
                sql += @" on p.id = s.productId where s.memberId = {0}  ";
                sql += @" and  p.dtSellEnd < getdate()); ";                
                var query = string.Format(sql, memberId);
                dbContext.Database.ExecuteSqlCommand(query);
            }
        }

        public List<ShopCartPrd> getTimeOutItem(int memberId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" select s.*,p.name,p.prdImages,p.summary,p.SpecialRule, p.shipType , p.stockNum from dbo.TenantPrd p ";
                sql += @" left join dbo.ShopCart s on s.productId = p.id ";
                sql += @" where s.memberId = {0} ";
                sql += @" and (p.dtSellStart > getdate() or p.dtSellEnd < getdate()) ";
                var query = string.Format(sql, memberId);
                return dbContext.Database.SqlQuery<ShopCartPrd>(query).ToList();
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

        public ShopCart getShopCartPrd(Guid tenantId, int memberId, int productId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" SELECT TOP 1 * FROM [ShopCart] c ";
                sql += @" where c.tenantId = '{0}' and c.memberId = {1} and c.productId = {2} ";
                var query = String.Format(sql, tenantId, memberId, productId);
                return dbContext.Database.SqlQuery<ShopCart>(query).FirstOrDefault();
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
