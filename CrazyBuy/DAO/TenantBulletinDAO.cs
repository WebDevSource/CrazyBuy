using CrazyBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBuy.DAO
{
    public class TenantBulletinDAO : CrazyBuyRerpository
    {
        public List<TenantBulletin> getBulletin(Guid tenantId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"SELECT * FROM [TenantBulletin] where tenantId = '{0}' and status = N'上架' order by isTop desc ,sort";
                sql = String.Format(sql, tenantId);
                return dbContext.Database.SqlQuery<TenantBulletin>(sql).ToList();
            }
        }
    }
}
