﻿using System;
using System.Collections.Generic;
using System.Linq;
using CrazyBuy.Models;
namespace CrazyBuy.DAO
{
    public class TenantDAO : CrazyBuyRerpository
    {
        public Tenant getTenant(Guid id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Tenant model = dbContext.Tenant.Where(
                              m => m.tenantId == id).SingleOrDefault();
                return model;
            }
        }

        public Tenant getTenantByTenantCode(string tenantCode)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Tenant model = dbContext.Tenant.Where(
                              m => m.tenantCode == tenantCode).SingleOrDefault();
                return model;
            }
        }

        public Tenant getTenantByOwner(int ownerId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Tenant model = dbContext.Tenant.Where(
                              m => m.createdMemberId == ownerId).FirstOrDefault();
                return model;
            }
        }

        public List<Tenant> GetTable(int limit)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                //IQueryable<Tenant> result = dbContext.Tenant;
                //result = result.Take(limit);
                //return result.ToList();

                var sql = @"SELECT TOP (10) [tenantId],[tenantCode] FROM [dbo].[Tenant]";

                return dbContext.Database.SqlQuery<Tenant>(sql).ToList();
            }
        }

        public void addTenant(Tenant tenant)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.Tenant.Add(tenant);
                dbContext.SaveChanges();
            }
        }

        public void removeTenant(Tenant tenant)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Tenant model = dbContext.Tenant.Where(
                m => m.tenantId == tenant.tenantId).SingleOrDefault();

                if (model != null)
                {
                    dbContext.Tenant.Attach(model);
                    dbContext.Tenant.Remove(model);
                    dbContext.SaveChanges();
                }
            }
        }

        public void updateTenant(Tenant tenant)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Tenant model = dbContext.Tenant.Where(
                m => m.tenantId == tenant.tenantId).SingleOrDefault();
                if (model != null)
                {
                    dbContext.Entry(model).CurrentValues.SetValues(tenant);
                    dbContext.SaveChanges();
                }
            }
        }

        public List<TenantSetting> getTenantSetting(Guid tenantId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"SELECT * FROM dbo.TenantSetting where title in  (N'免運設定',N'常溫運費',N'低溫運費') and tenantId = '{0}' ";
                sql = string.Format(sql, tenantId);
                return dbContext.Database.SqlQuery<TenantSetting>(sql).ToList();
            }
        }

        public List<TenantSetting> getAllTenantSetting(Guid tenantId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"SELECT * FROM dbo.TenantSetting where  tenantId = '{0}' ";
                sql = string.Format(sql, tenantId);
                return dbContext.Database.SqlQuery<TenantSetting>(sql).ToList();
            }
        }

        public TenantSetting getTenantSetting(Guid tenantId, string title)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"SELECT * FROM dbo.TenantSetting where title = N'{0}' and tenantId = '{1}' ";
                sql = string.Format(sql, title, tenantId);
                return dbContext.Database.SqlQuery<TenantSetting>(sql).SingleOrDefault();
            }
        }

        public List<TenantSetting> GetDefaultTenantSettings()
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"SELECT * FROM dbo.TenantSetting where  tenantId is null ";
                //sql = string.Format(sql, tenantId);
                return dbContext.Database.SqlQuery<TenantSetting>(sql).ToList();
            }
        }

        public List<string> checkTenantCodeIsExist(List<string> tenantCodes)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                string strTenantCodes = string.Join("',N'", tenantCodes);
                var sql = @"SELECT DISTINCT tenantCode FROM dbo.Tenant where tenantCode IN (N'{0}')";
                sql = string.Format(sql, strTenantCodes);
                return dbContext.Database.SqlQuery<string>(sql).ToList();
            }
        }

        public TenantGrade GetTenantGrade(Guid tenantId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"SELECT Top 1 * FROM dbo.TenantGrade where tenantId = '{0}'  and status = N'正常'  order by dtStart desc";
                sql = string.Format(sql, tenantId);
                return dbContext.Database.SqlQuery<TenantGrade>(sql).SingleOrDefault();
            }
        }

    }
}
