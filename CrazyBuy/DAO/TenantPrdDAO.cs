using CrazyBuy.Common;
using CrazyBuy.Models;
using CrazyBuy.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBuy.DAO
{
    public class TenantPrdDAO : CrazyBuyRerpository
    {
        // 首頁商品
        public List<TenantPrd> getHomePrds(Guid tenantId, int userId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"SELECT tp.* FROM [TenantPrd] tp";
                sql += @" left join [TenantHomePrd] hp on hp.prdId = tp.id";
                sql += @" left join [TenantPrdRead] pr on pr.prdId = tp.id ";
                sql += @" left join [TenantMember] mr on mr.memberId = " + userId;
                sql += @" where tp.[isOpenOrder] = 1 and tp.status = N'上架' and hp.dtStart <= getdate() and hp.dtEnd >= getdate() ";
                sql += @" and hp.[tenantId] ='{0}' ";
                sql += @" and pr.status = N'正常' and pr.type = N'所有會員' or pr.tenantMemId = {1} or pr.memLevelId = mr.levelId ";

                string query = String.Format(sql, tenantId.ToString(), userId);
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

        public int getCountByCatId(Guid tenantId, long catId, int memberId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = "";
                if (isParent(catId))
                {
                    //sql = @" select count(*) as total from dbo.TenantPrd p ";
                    //sql += @" left join dbo.TenantPrdCatRel r on r.prdId = p.id ";
                    //sql += @" left join dbo.TenantPrdCatAd a on a.descendantId = r.catId ";
                    //sql += @" where a.ancestorId = {0} and  ";
                    //sql += @" p.tenantId = '{1}' and r.status = N'正常' and p.status = N'上架' and (p.dtSellEnd >= getdate() or (p.dtSellEnd <= getdate() and takeOffMethod = N'隱藏訂購鈕')) ";

                    sql = @" select count(*) from dbo.TenantPrd p ";
                    sql += @" left join dbo.TenantPrdCatRel r on r.prdId = p.id ";
                    sql += @" left join dbo.TenantPrdCatAd a on a.descendantId = r.catId ";
                    sql += @" left join dbo.TenantPrdRead pr on pr.prdId = p.id ";
                    sql += @" left join dbo.TenantMember mr on mr.memberId = " + memberId;
                    sql += @" where p.tenantId = '{0}' and a.ancestorId = {1} and r.status = N'正常' and p.status = N'上架' and (p.dtSellEnd >= getdate() or (p.dtSellEnd <= getdate() and takeOffMethod = N'隱藏訂購鈕')) ";
                    sql += @" and pr.status = N'正常' and pr.type = N'所有會員' or pr.tenantMemId = {2} or pr.memLevelId = mr.levelId ";
                }
                else
                {
                    //    sql = @" select count(*) as total  from dbo.TenantPrd p ";
                    //    sql += @" left join dbo.TenantPrdCatRel r on r.prdId = p.id ";
                    //    sql += @" where r.catId = {0} and  ";
                    //    sql += @" p.tenantId = '{1}' and r.status = N'正常' and p.status = N'上架' and (p.dtSellEnd >= getdate() or (p.dtSellEnd <= getdate() and takeOffMethod = N'隱藏訂購鈕'))  ";

                    sql = @" select count(*) from dbo.TenantPrd p ";
                    sql += @" left join dbo.TenantPrdCatRel r on r.prdId = p.id ";
                    sql += @" left join dbo.TenantPrdRead pr on pr.prdId = p.id ";
                    sql += @" left join dbo.TenantMember mr on mr.memberId = " + memberId;
                    sql += @" where p.tenantId = '{0}' and r.catId = {1} and r.status = N'正常' and p.status = N'上架' and (p.dtSellEnd >= getdate() or (p.dtSellEnd <= getdate() and takeOffMethod = N'隱藏訂購鈕')) ";
                    sql += @" and pr.status = N'正常' and pr.type = N'所有會員' or pr.tenantMemId = {2} or pr.memLevelId = mr.levelId ";
                }
                //sql = String.Format(sql, top, tenantId, catId, notInsql);
                string query = String.Format(sql, tenantId, catId, memberId);
                MDebugLog.debug("[getCountByCatId] > " + query);
                return dbContext.Database.SqlQuery<SqlQueryTotal>(query).SingleOrDefault().total;
            }
        }

        public bool isParent(long catId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" select count(*) as total from TenantPrdCatAd a where a.ancestorId = {0} ";
                string query = String.Format(sql, catId);
                return dbContext.Database.SqlQuery<SqlQueryTotal>(query).SingleOrDefault().total > 0;
            }
        }

        public List<TenantPrd> getTenandPrdByCatId(PrdPageQuery pageQuery, int userId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                int top = pageQuery.count;
                int pageCount = top * pageQuery.page;
                string tenantId = pageQuery.tnenatId.ToString();
                long catId = pageQuery.catId;

                var sql = "";
                var notInsql = "";
                if (isParent(catId))
                {
                    notInsql = @" select TOP {0} p.id from dbo.TenantPrd p ";
                    notInsql += @" left join dbo.TenantPrdCatRel r on r.prdId = p.id ";
                    notInsql += @" left join dbo.TenantPrdCatAd a on a.descendantId = r.catId ";
                    notInsql += @" left join dbo.TenantPrdRead pr on pr.prdId = p.id ";
                    notInsql += @" left join dbo.TenantMember mr on mr.memberId = " + userId;
                    notInsql += @" where p.tenantId = '{1}' and a.ancestorId = {2} and r.status = N'正常' and p.status = N'上架' and (p.dtSellEnd >= getdate() or (p.dtSellEnd <= getdate() and takeOffMethod = N'隱藏訂購鈕'))  ";
                    notInsql += @" and pr.status = N'正常' and pr.type = N'所有會員' or pr.tenantMemId = {3} or pr.memLevelId = mr.levelId";
                    notInsql += SortType.getOrderBy(pageQuery.sortType);
                    notInsql = String.Format(notInsql, pageCount, tenantId, catId, userId);

                    sql = @" select TOP {0} p.* from dbo.TenantPrd p ";
                    sql += @" left join dbo.TenantPrdCatRel r on r.prdId = p.id ";
                    sql += @" left join dbo.TenantPrdCatAd a on a.descendantId = r.catId ";
                    sql += @" left join dbo.TenantPrdRead pr on pr.prdId = p.id ";
                    sql += @" left join dbo.TenantMember mr on mr.memberId = " + userId;
                    sql += @" where p.tenantId = '{1}' and a.ancestorId = {2} and r.status = N'正常' and p.status = N'上架' and (p.dtSellEnd >= getdate() or (p.dtSellEnd <= getdate() and takeOffMethod = N'隱藏訂購鈕')) ";
                    sql += @" and pr.status = N'正常' and pr.type = N'所有會員' or pr.tenantMemId = {3} or pr.memLevelId = mr.levelId ";
                    sql += @" and p.id not in ( {4} ) ";
                    sql += SortType.getOrderBy(pageQuery.sortType);
                }
                else
                {
                    notInsql = @" select TOP {0} p.id from dbo.TenantPrd p ";
                    notInsql += @" left join dbo.TenantPrdCatRel r on r.prdId = p.id ";
                    notInsql += @" left join dbo.TenantPrdRead pr on pr.prdId = p.id ";
                    notInsql += @" left join dbo.TenantMember mr on mr.memberId = " + userId;
                    notInsql += @" where p.tenantId = '{1}' and r.catId = {2} and r.status = N'正常' and p.status = N'上架' and (p.dtSellEnd >= getdate() or (p.dtSellEnd <= getdate() and takeOffMethod = N'隱藏訂購鈕')) ";
                    notInsql += @" and pr.status = N'正常' and pr.type = N'所有會員' or pr.tenantMemId = {3} or pr.memLevelId = mr.levelId ";
                    notInsql += SortType.getOrderBy(pageQuery.sortType);
                    notInsql = String.Format(notInsql, pageCount, tenantId, catId, userId);

                    sql = @" select TOP {0} p.* from dbo.TenantPrd p ";
                    sql += @" left join dbo.TenantPrdCatRel r on r.prdId = p.id ";
                    sql += @" left join dbo.TenantPrdRead pr on pr.prdId = p.id ";
                    sql += @" left join dbo.TenantMember mr on mr.memberId = " + userId;
                    sql += @" where p.tenantId = '{1}' and r.catId = {2} and r.status = N'正常' and p.status = N'上架' and (p.dtSellEnd >= getdate() or (p.dtSellEnd <= getdate() and takeOffMethod = N'隱藏訂購鈕')) ";
                    sql += @" and pr.status = N'正常' and pr.type = N'所有會員' or pr.tenantMemId = {3} or pr.memLevelId = mr.levelId ";
                    sql += @" and p.id not in ( {4} ) ";
                    sql += SortType.getOrderBy(pageQuery.sortType);
                }
                sql = String.Format(sql, top, tenantId, catId, userId, notInsql);

                MDebugLog.debug("[getTenandPrdByCatId] >" + sql);
                return dbContext.Database.SqlQuery<TenantPrd>(sql).ToList();
            }
        }

        public List<TenantPrd> getSearchTenandPrdByCatId(PrdSearchQuery searchQuery, int userId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                int top = searchQuery.count;
                int pageCount = top * searchQuery.page;
                string tenantId = searchQuery.tnenatId.ToString();
                long catId = searchQuery.catId;
                var notInsql = @" select TOP {0} p.id from [TenantPrd] p ";
                notInsql += @" left join [TenantPrdCatRel] r on r.prdId = p.id ";
                notInsql += @" left join [TenantPrdRead] pr on pr.prdId = p.id ";
                notInsql += @" left join [TenantMember] mr on mr.memberId = " + userId;
                notInsql += @" where p.tenantId = '{1}' and r.catId = {2} and p.status = N'上架' and p.name like N'%{3}%' and (p.dtSellEnd >= getdate() or (p.dtSellEnd <= getdate() and takeOffMethod = N'隱藏訂購鈕'))  ";
                notInsql += @" and pr.status = N'正常' and pr.type = N'所有會員' or pr.tenantMemId = {3} or pr.memLevelId = mr.levelId ";
                notInsql += SortType.getOrderBy(searchQuery.sortType);
                notInsql = String.Format(notInsql, pageCount, tenantId, catId, searchQuery.name, userId);

                var sql = @" select TOP {0} p.* from [TenantPrd] p ";
                sql += @" left join [TenantPrdCatRel] r on r.prdId = p.id ";
                sql += @" left join [TenantPrdRead] pr on pr.prdId = p.id ";
                sql += @" left join [TenantMember] mr on mr.memberId = " + userId;
                sql += @" where p.tenantId = '{1}' and r.catId = {2} and  p.status = N'上架' and p.name like N'%{3}%' and (p.dtSellEnd >= getdate() or (p.dtSellEnd <= getdate() and takeOffMethod = N'隱藏訂購鈕')) ";
                sql += @" and pr.status = N'正常' and pr.type = N'所有會員' or pr.tenantMemId = {3} or pr.memLevelId = mr.levelId ";
                sql += @" and p.id not in ( {4} ) ";
                sql += SortType.getOrderBy(searchQuery.sortType);
                sql = String.Format(sql, top, tenantId, catId, searchQuery.name, userId, notInsql);

                MDebugLog.debug("[getSearchTenandPrdByCatId] >" + sql);
                return dbContext.Database.SqlQuery<TenantPrd>(sql).ToList();
            }
        }

        public CustSpcPrice getSpcTenantPrdPrice(Guid tenantId, int prdId, int gradeId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" select * from [TenantPrdCustPrice] s ";
                sql += @" left join [TenantCustPriceGrade] g on g.id = s.custPriceGradeId ";
                sql += @" where s.prdId = {0} and s.custPriceGradeId = {1} ";
                sql += @" and g.status = N'正常' and g.tenantId = '{2}' ";
                sql = string.Format(sql, prdId, gradeId, tenantId);
                CustSpcPrice spc = dbContext.Database.SqlQuery<CustSpcPrice>(sql).SingleOrDefault();
                return spc;
            }
        }
    }
}
