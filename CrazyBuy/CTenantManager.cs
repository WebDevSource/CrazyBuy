using CrazyBuy;
using CrazyBuy.Models;
using System.Collections.Generic;

namespace CrazyBuy
{
    public class TenantManager
    {
        private static PlusOneRerpository rerpos = new PlusOneRerpository();

        public static List<Tenant> getTenantTop(int limit)
        {
           return rerpos.GetTable(limit);
        }

        public static void saveTenant(Tenant tenant)
        {
            rerpos.addTenant(tenant);
        }

        public static void removeTenant(Tenant tenant)
        {
            rerpos.removeTenant(tenant);
        }

        public static void updateTenant(Tenant tenant)
        {
            rerpos.updateTenant(tenant);
        }
    }
}
