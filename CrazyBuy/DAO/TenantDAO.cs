using System.Collections.Generic;
using System.Linq;
using CrazyBuy.Models;
namespace CrazyBuy.DAO
{
    public class TenantDAO : CrazyBuyRerpository
    {
        public List<Tenant> GetTable(int limit)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                IQueryable<Tenant> result = dbContext.Tenant;
                result = result.Take(limit);
                return result.ToList();
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
    }
}
