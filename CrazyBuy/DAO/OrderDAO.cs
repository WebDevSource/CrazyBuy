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

        public OrderMaster getOrder(int id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                OrderMaster model = dbContext.OrderMaster.Where(
                              m => m.id == id).SingleOrDefault();
                return model;
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
        public List<OrderDetail> getDetailLists(int id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"SELECT * FROM [OrderDetail] where orderId = {0} ";
                var query = String.Format(sql, id);
                return dbContext.Database.SqlQuery<OrderDetail>(query).ToList();
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
