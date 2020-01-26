using System.Collections.Generic;
using CrazyBuy.DAO;
using CrazyBuy.Models;

namespace CrazyBuy
{
    public class TenantManager
    {
        private static TenantDAO rerpos = new TenantDAO();

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
