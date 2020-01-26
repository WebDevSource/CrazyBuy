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
                IQueryable<Tenant> result = dbContext.getTenant;
                result = result.Take(limit);
                return result.ToList();
            }
        }

        public void addTenant(Tenant tenant)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.getTenant.Add(tenant);
                dbContext.SaveChanges();
            }
        }

        public void removeTenant(Tenant tenant)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Tenant model = dbContext.getTenant.Where(
                m => m.tenantId == tenant.tenantId).SingleOrDefault();

                if (model != null)
                {
                    dbContext.getTenant.Attach(model);
                    dbContext.getTenant.Remove(model);
                    dbContext.SaveChanges();
                }
            }
        }

        public void updateTenant(Tenant tenant)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Tenant model = dbContext.getTenant.Where(
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
