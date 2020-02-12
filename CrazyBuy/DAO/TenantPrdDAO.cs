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

        public List<TenantPrd> getTenandPrdByCatId(Guid tenantId, long catId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" select p.* from [TenantPrd] p ";
                sql += @" left join [TenantPrdCatRel] r on r.prdId = p.id ";
                sql += @" where p.tenantId = '{0}' and r.catId = {1} ";

                string query = String.Format(sql, tenantId.ToString(), catId);
                return dbContext.Database.SqlQuery<TenantPrd>(query).ToList();
            }
        }
    }
}
