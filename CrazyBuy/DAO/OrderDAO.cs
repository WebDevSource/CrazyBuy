using CrazyBuy.Common;
using CrazyBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBuy.DAO
{
    public class OrderDAO : CrazyBuyRerpository
    {
        public int addOrderMaster(OrderMaster orderMaster)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.OrderMaster.Add(orderMaster);
                dbContext.SaveChanges();
                return orderMaster.id;
            }
        }

        public OrderMasterUser getOrder(int id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" SELECT m.*,r.name as userName FROM [OrderMaster] m ";
                sql += @" LEFT JOIN [Member] r on r.memberId = m.memberId ";
                sql += @" WHERE m.Id = {0} ";
                var query = String.Format(sql, id);                
                return dbContext.Database.SqlQuery<OrderMasterUser>(query).FirstOrDefault();
            }
        }

        public List<OrderMaster> getOrderByMember(int memberId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                List<OrderMaster> result = dbContext.OrderMaster.Where(
                              m => m.memberId == memberId).ToList();
                return result;
            }
        }
        public List<OrderPrdDetail> getDetailLists(int id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" SELECT d.*,p.name,p.summary,p.prdImages,p.prdSepc FROM [OrderDetail] d ";
                sql += @" LEFT JOIN [TenantPrd] p on p.id = d.prdId ";
                sql += @" where d.orderId = {0} ";
                var query = String.Format(sql, id);
                return (List<OrderPrdDetail>)dbContext.Database.SqlQuery<OrderPrdDetail>(query).ToList();
            }
        }


        public void addOrderDetail(OrderDetail orderDetail)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.OrderDetail.Add(orderDetail);
                dbContext.SaveChanges();
            }
        }

        public void removeOrder(int id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" DELETE FROM [OrderMaster] WHERE id = {0} ";
                var query = String.Format(sql, id);
                dbContext.Database.ExecuteSqlCommand(query);

                sql = @" DELETE FROM [OrderDetail] WHERE orderId = {0} ";
                query = String.Format(sql, id);
                dbContext.Database.ExecuteSqlCommand(query);
            }
        }
    }
}
