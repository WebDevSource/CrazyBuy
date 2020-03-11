using System.Collections.Generic;
using CrazyBuy.DAO;
using CrazyBuy.Models;

namespace CrazyBuy
{
    public class CTenantManager
    {        

        public static List<Tenant> getTenantTop(int limit)
        {
           return DataManager.tenantDao.GetTable(limit);
        }

        public static void saveTenant(Tenant tenant)
        {
            DataManager.tenantDao.addTenant(tenant);
        }

        public static void removeTenant(Tenant tenant)
        {
            DataManager.tenantDao.removeTenant(tenant);
        }

        public static void updateTenant(Tenant tenant)
        {
            DataManager.tenantDao.updateTenant(tenant);
        }

        public static bool isOwner(int userId)
        {
            return DataManager.tenantDao.getTenantByOwner(userId) != null ? true : false;
        }
    }
}
