using CrazyBuy.Common;
using CrazyBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBuy.DAO
{
    public class TenantPrdDAO : CrazyBuyRerpository
    {
        // 首頁商品
        public List<TenantPrd> getHomePrds(Guid tenantId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"SELECT tp.* FROM [TenantPrd] tp";
                sql += @" left join [TenantHomePrd] hp on hp.prdId = tp.id";
                sql += @" where tp.status not in (N'刪除',N'下架') ";
                sql += @" and hp.[tenantId] ='{0}' ";


                string query = String.Format(sql, tenantId.ToString(), DateTime.Now);
                MDebugLog.debug("[getHomePrds] > " + query);
                return dbContext.Database.SqlQuery<TenantPrd>(query).ToList();
            }
        }

        // 單獨商品
        public TenantPrd getTenandPrd(int prdId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                TenantPrd model = dbContext.TenantPrd.Where(
                m => m.id == prdId).SingleOrDefault();
                return model;
            }
        }

        public void updateTenandPrd(TenantPrd tenantPrd)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                TenantPrd model = dbContext.TenantPrd.Where(
                m => m.id == tenantPrd.id).SingleOrDefault();
                if (model != null)
                {
                    dbContext.Entry(model).CurrentValues.SetValues(tenantPrd);
                    dbContext.SaveChanges();
                }
            }
        }

        public int getCountByCatId(Guid tenantId, long catId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" select count(p.id) as total from [TenantPrd] p ";
                sql += @" left join [TenantPrdCatRel] r on r.prdId = p.id ";
                sql += @" where p.tenantId = '{0}' and r.catId = {1} and p.status not in (N'刪除',N'下架') ";
                string query = String.Format(sql, tenantId, catId);
                MDebugLog.debug("[getCountByCatId] > " + query);
                return dbContext.Database.SqlQuery<SqlQueryTotal>(query).SingleOrDefault().total;
            }
        }

        public List<TenantPrd> getTenandPrdByCatId(PrdPageQuery pageQuery)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                int top = pageQuery.count;
                int pageCount = top * pageQuery.page;
                string tenantId = pageQuery.tnenatId.ToString();
                long catId = pageQuery.catId;

                var notInsql = @" select TOP {0} p.id from [TenantPrd] p ";
                notInsql += @" left join [TenantPrdCatRel] r on r.prdId = p.id ";
                notInsql += @" where p.tenantId = '{1}' and r.catId = {2} and p.status not in (N'刪除',N'下架') ";
                notInsql += SortType.getOrderBy(pageQuery.sortType);
                notInsql = String.Format(notInsql, pageCount, tenantId, catId);

                var sql = @" select TOP {0} p.* from [TenantPrd] p ";
                sql += @" left join [TenantPrdCatRel] r on r.prdId = p.id ";
                sql += @" where p.tenantId = '{1}' and r.catId = {2} and p.status  not in (N'刪除',N'下架') ";
                sql += @" and p.id not in ( {3} ) ";
                sql += SortType.getOrderBy(pageQuery.sortType);
                sql = String.Format(sql, top, tenantId, catId, notInsql);

                MDebugLog.debug("[getTenandPrdByCatId] >" + sql);
                return dbContext.Database.SqlQuery<TenantPrd>(sql).ToList();
            }
        }

        public List<TenantPrd> getSearchTenandPrdByCatId(PrdSearchQuery searchQuery)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                int top = searchQuery.count;
                int pageCount = top * searchQuery.page;
                string tenantId = searchQuery.tnenatId.ToString();
                long catId = searchQuery.catId;

                var notInsql = @" select TOP {0} p.id from [TenantPrd] p ";
                notInsql += @" left join [TenantPrdCatRel] r on r.prdId = p.id ";
                notInsql += @" where p.tenantId = '{1}' and r.catId = {2} and p.status not in (N'刪除',N'下架') and p.name like N'%{3}%' ";
                notInsql += SortType.getOrderBy(searchQuery.sortType);
                notInsql = String.Format(notInsql, pageCount, tenantId, catId, searchQuery.name);

                var sql = @" select TOP {0} p.* from [TenantPrd] p ";
                sql += @" left join [TenantPrdCatRel] r on r.prdId = p.id ";
                sql += @" where p.tenantId = '{1}' and r.catId = {2} and and p.status not in (N'刪除',N'下架') p.name like N'%{3}%' ";
                sql += @" and p.id not in ( {4} ) ";
                sql += SortType.getOrderBy(searchQuery.sortType);
                sql = String.Format(sql, top, tenantId, catId, searchQuery.name, notInsql);

                MDebugLog.debug("[getSearchTenandPrdByCatId] >" + sql);
                return dbContext.Database.SqlQuery<TenantPrd>(sql).ToList();
            }
        }

        public CustSpcPrice getSpcTenantPrdPrice(Guid tenantId, int prdId, int gradeId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" select name,price from [TenantPrdCustPrice] s ";
                sql += @" left join [TenantCustPriceGrade] g on g.id = s.custPriceGradeId ";
                sql += @" where s.prdId = {0} and s.custPriceGradeId = {1} ";
                sql += @" and g.status = N'正常' and g.tenantId = '{2}' ";
                sql = string.Format(sql, prdId, gradeId, tenantId);
                CustSpcPrice spc = dbContext.Database.SqlQuery<CustSpcPrice>(sql).SingleOrDefault();
                return spc ;
            }
        }
    }
}
