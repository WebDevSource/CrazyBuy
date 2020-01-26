using CrazyBuy.Models;
using CrazyBuy.Common;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CrazyBuy
{
    public class PlusOneRerpository
    {
        public PlusOneDbContext ContextInit()
        {
            //通常連線字串會放在config中
            return new PlusOneDbContext(Utils.GetConfiguration("DataBase"));
        }

        public List<Tenant> GetTable(int limit)
        {
            using (PlusOneDbContext dbContext = ContextInit())
            {
                IQueryable<Tenant> result = dbContext.getTenant;
                result = result.Take(limit);
                return result.ToList();
            }
        }

        public void addTenant(Tenant tenant)
        {
            using (PlusOneDbContext dbContext = ContextInit())
            {
                dbContext.getTenant.Add(tenant);
                dbContext.SaveChanges();
            }
        }

        public void removeTenant(Tenant tenant)
        {
            using (PlusOneDbContext dbContext = ContextInit())
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
            using (PlusOneDbContext dbContext = ContextInit())
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
