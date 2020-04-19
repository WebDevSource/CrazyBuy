﻿using CrazyBuy.Common;
using CrazyBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBuy.DAO
{
    public class TenantPrdCatDAO : CrazyBuyRerpository
    {
        public List<TenantPrdCat> getPrdRootCats(Guid tenantId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"select * from [TenantPrdCat] where tenantId = '{0}' and parentId IS NULL and status = N'正常'";
                string query = String.Format(sql, tenantId.ToString());
                return dbContext.Database.SqlQuery<TenantPrdCat>(query).ToList();
            }
        }

        public List<TenantPrdCatCount> getAllPrdCats(Guid tenantId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" select c.* ,  ";
                sql += @" (select count(id) from TenantPrdCatAd tpca where tpca.ancestorId  = c.id) as count , ";
                sql += @" ( select count(cr.id) from TenantPrdCatRel cr  ";
                sql += @"   left join TenantPrd prd on prd.id = cr.prdId ";
                sql += @"   where cr.catId  = c.id and cr.status = N'正常' ";
                sql += @"   and prd.status = N'上架' and (prd.dtSellEnd >= getdate() or (prd.dtSellEnd <= getdate() and prd.takeOffMethod = N'隱藏訂購鈕'))) as pcount ,";
                sql += @" (select count(*) as total from dbo.TenantPrd p ";
                sql += @" inner join TenantPrdCatRel r on r.prdId = p.id ";
                sql += @" inner join TenantPrdCatAd a on r.catId = a.descendantId ";
                sql += @" where a.ancestorId = c.id ";
                sql += @" and r.status = N'正常' and p.status = N'上架' and (p.dtSellEnd >= getdate() or (p.dtSellEnd <= getdate() and takeOffMethod = N'隱藏訂購鈕'))) as ccount ";
                sql += @" from [TenantPrdCat] c where tenantId = '{0}' and status = N'{1}'  order by parentId asc ";
                string query = String.Format(sql, tenantId.ToString(), "正常");
                MDebugLog.debug("@" + query);
                return dbContext.Database.SqlQuery<TenantPrdCatCount>(query).ToList();
            }
        }

        public List<TenantPrdCat> getPrdCatsByParentId(Guid tenantId, long parentId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"select * from [TenantPrdCat] where tenantId = '{0}' and parentId = {1} and status = N'正常'";
                string query = String.Format(sql, tenantId, parentId);
                return dbContext.Database.SqlQuery<TenantPrdCat>(query).ToList();
            }
        }

        public int getCountCatsByParentId(Guid tenantId, long parentId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"select count(*) as total from [TenantPrdCat] where tenantId = '{0}' and parentId = {1}";
                string query = String.Format(sql, tenantId, parentId);
                return dbContext.Database.SqlQuery<SqlQueryTotal>(query).SingleOrDefault().total;
            }
        }

        public List<TenantPrdCatRead> getCatReads(int catId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"select catId,type,tenantMemId,memLevelId from [TenantPrdCatRead] where catId = {0} and status = N'正常'";
                string query = String.Format(sql, catId);
                return dbContext.Database.SqlQuery<TenantPrdCatRead>(query).ToList();
            }
        }
    }
}
