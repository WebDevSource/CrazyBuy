using CrazyBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrazyBuy.DAO
{
    public class TenantFAQDAO : CrazyBuyRerpository
    {
        public List<TenantFAQ> getFAQ(Guid tenantId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                IQueryable<TenantFAQ> result = dbContext.TenantFAQ.Where(
                m => m.tenantId == tenantId);
                return result.ToList();
            }
        }
    }
}
