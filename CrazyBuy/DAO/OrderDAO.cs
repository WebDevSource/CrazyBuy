using CrazyBuy.Common;
using CrazyBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBuy.DAO
{
    public class OrderDAO : CrazyBuyRerpository
    {
        public OrderMaster addOrderMaster(OrderMaster orderMaster)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.OrderMaster.Add(orderMaster);
                dbContext.SaveChanges();
                return orderMaster;
            }
        }

        public OrderMasterUser getOrder(int id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" SELECT m.*,r.name as userName FROM [OrderMaster] m ";
                sql += @" LEFT JOIN [Member] r on r.memberId = m.memberId ";
                sql += @" WHERE m.Id = {0} and m.status <> N'刪除'";
                var query = String.Format(sql, id);
                return dbContext.Database.SqlQuery<OrderMasterUser>(query).FirstOrDefault();
            }
        }

        public OrderMaster getOrderMaster(int id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" SELECT m.* FROM [OrderMaster] m ";                
                sql += @" WHERE m.Id = {0} ";
                var query = String.Format(sql, id);
                return dbContext.Database.SqlQuery<OrderMaster>(query).FirstOrDefault();
            }
        }

        public void updateOrderMaster(OrderMaster orderMaster)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                OrderMaster model = dbContext.OrderMaster.Where(
                m => m.id == orderMaster.id).SingleOrDefault();
                if (model != null)
                {
                    dbContext.Entry(model).CurrentValues.SetValues(orderMaster);
                    dbContext.SaveChanges();
                }
            }
        }

        public List<OrderMaster> getOrderByMember(int memberId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" SELECT * FROM [OrderMaster] WHERE memberId = {0} and status <> N'刪除' order by id desc ";
                var query = String.Format(sql, memberId);
                return (List<OrderMaster>)dbContext.Database.SqlQuery<OrderMaster>(query).ToList();
            }
        }
        public List<OrderPrdDetail> getDetailLists(int id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" SELECT d.*,p.name,p.summary,p.prdImages,p.prdSepc,p.prdCode,(case when d.priceGradeType = N'自訂價' then tcpg.name  when d.priceGradeType = N'轉批價' then d.priceGradeType else  N'會員價'  end) as priceType FROM [OrderDetail] d ";
                sql += @" LEFT JOIN [TenantPrd] p on p.id = d.prdId ";
                sql += @" LEFT JOIN [TenantPrdCustPrice] pcp on pcp.id = d.prdCustPriceId ";
                sql += @" LEFT JOIN [TenantCustPriceGrade] tcpg on tcpg.id = pcp.custPriceGradeId ";
                sql += @" where d.orderId = {0} and d.status = N'正常' ";
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
