using System;
using System.Collections.Generic;
using System.Linq;
using CrazyBuy.Models;

namespace CrazyBuy.DAO
{
    public class TenantFAQDAO : CrazyBuyRerpository
    {
        public List<TenantFAQ> getFAQ(Guid tenantId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"SELECT * FROM [TenantFAQ] where tenantId = '{0}' order by sort desc ";
                sql = String.Format(sql, tenantId);
                return dbContext.Database.SqlQuery<TenantFAQ>(sql).ToList();                
            }
        }
    }
}
