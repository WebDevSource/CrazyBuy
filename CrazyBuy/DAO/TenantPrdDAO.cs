﻿using CrazyBuy.Common;
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
                sql += @" where tp.[isOpenOrder] = 1 ";
                sql += @" and hp.[tenantId] ='{0}' ";

                string query = String.Format(sql, tenantId.ToString());
                return dbContext.Database.SqlQuery<TenantPrd>(query).ToList();
            }
        }

        // 單獨商品
        public TenantPrd getTenandPrd(Guid prdId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                TenantPrd model = dbContext.TenantPrd.Where(
                m => m.id == prdId).SingleOrDefault();
                return model;
            }
        }

        public int getCountByCatId(Guid tenantId, long catId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" select count(p.id) as total from [TenantPrd] p ";
                sql += @" left join [TenantPrdCatRel] r on r.prdId = p.id ";
                sql += @" where p.tenantId = '{0}' and r.catId = {1} ";
                string query = String.Format(sql, tenantId, catId);
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
                notInsql += @" where p.tenantId = '{1}' and r.catId = {2} ";
                notInsql += SortType.getOrderBy(pageQuery.sortType);
                notInsql = String.Format(notInsql, pageCount, tenantId, catId);

                var sql = @" select TOP {0} p.* from [TenantPrd] p ";
                sql += @" left join [TenantPrdCatRel] r on r.prdId = p.id ";
                sql += @" where p.tenantId = '{1}' and r.catId = {2} ";
                sql += @" and p.id not in ( {3} ) ";
                sql += SortType.getOrderBy(pageQuery.sortType);
                sql = String.Format(sql, top, tenantId, catId, notInsql);
               
                MDebugLog.debug("[getTenandPrdByCatId] >" + sql);
                return dbContext.Database.SqlQuery<TenantPrd>(sql).ToList();
            }
        }
    }
}
