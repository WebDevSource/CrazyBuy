using System;
using System.Collections.Generic;
using CrazyBuy.DAO;
using CrazyBuy.Models;

namespace CrazyBuy.Services
{
    public class CTenantPrdCatManager
    {

        public static List<TenantPrdCat> getRootCats(Guid tenantId)
        {
            List<TenantPrdCat> data = DataManager.tenantPrdCatDao.getPrdRootCats(tenantId);
            return data;
        }

        public static List<TenantPrdCat> getTreeCats(Guid tenantId, long parentId)
        {
            List<TenantPrdCat> data = DataManager.tenantPrdCatDao.getPrdCatsByParentId(tenantId, parentId);
            return data;
        }

        public static List<TenantPrdCatCount> getAllCats(Guid tenantId)
        {
            List<TenantPrdCatCount> data = DataManager.tenantPrdCatDao.getAllPrdCats(tenantId);
            List<TenantPrdCatCount> result = new List<TenantPrdCatCount>();
            foreach (TenantPrdCatCount item in data)
            {
                item.count = item.count + item.pcount;
                result.Add(item);
            }
            return result;
        }
    }
}
