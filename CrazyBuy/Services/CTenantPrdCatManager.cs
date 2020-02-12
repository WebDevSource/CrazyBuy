using CrazyBuy.Common;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using System;
using System.Collections.Generic;

namespace CrazyBuy.Services
{
    public class CTenantPrdCatManager
    {

        public static List<TenantPrdCat> getRootCats(Guid tenantId)
        {
            string key = tenantId.ToString();
            if (CacheResult.isKeyExist(key))
            {
                return (List<TenantPrdCat>)CacheResult.getData(key);
            }
            else
            {
                List<TenantPrdCat> data = DataManager.tenantPrdCatDao.getPrdRootCats(tenantId);
                CacheResult.setData(key, data);
                return data;
            }
        }

        public static List<TenantPrdCat> getTreeCats(Guid tenantId, long parentId)
        {
            string key = tenantId.ToString() + parentId;
            if (CacheResult.isKeyExist(key))
            {
                return (List<TenantPrdCat>)CacheResult.getData(key);
            }
            else
            {
                List<TenantPrdCat> data = DataManager.tenantPrdCatDao.getPrdCatsByParentId(tenantId, parentId);
                CacheResult.setData(key, data);
                return data;
            }
        }
    }
}
