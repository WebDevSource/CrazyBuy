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
            string key = tenantId.ToString() + "getRootCats";
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
            string key = tenantId.ToString() + parentId + "getTreeCats";
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
        
        public static List<TenantPrdCatCount> getAllCats(Guid tenantId)
        {
            string key = tenantId.ToString() + "getAllCats";
            if (CacheResult.isKeyExist(key))
            {
                return (List<TenantPrdCatCount>)CacheResult.getData(key);
            }
            else
            {
                List<TenantPrdCat> data = DataManager.tenantPrdCatDao.getAllPrdCats(tenantId);
                List<TenantPrdCatCount> result = new List<TenantPrdCatCount>();
                foreach (TenantPrdCat item in data)
                {
                    TenantPrdCatCount raw = new TenantPrdCatCount();
                    raw.id = item.id;
                    raw.tenantId = item.tenantId;
                    raw.parentId = item.parentId == null ? 0 : item.parentId;
                    raw.name = item.name;
                    raw.sort = item.sort;
                    raw.status = item.status;
                    raw.createTime = item.createTime;
                    raw.updateTime = item.updateTime;
                    raw.creator = item.creator;
                    raw.updater = item.updater;
                    raw.count = DataManager.tenantPrdCatDao.getCountCatsByParentId(tenantId, (long)raw.parentId);
                    result.Add(raw);
                }
                CacheResult.setData(key, result);
                return result;
            }
        }
    }
}
