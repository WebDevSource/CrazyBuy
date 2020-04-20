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

        public static List<TenantPrdCatCount> getAllCats(Guid tenantId, int memberId)
        {
            TenantMember tenantMember = DataManager.tenantMemberDao.getTenantMemberByMemberId(memberId);

            string userLvType = UserLevelType.NORMAL;
            if (tenantMember != null)
            {
                userLvType = tenantMember.levelId == null ? UserLevelType.NORMAL : UserLevelType.ADVANCED;
            }
            List<TenantPrdCatCount> data = DataManager.tenantPrdCatDao.getAllPrdCats(tenantId, memberId);
            List<TenantPrdCatCount> result = new List<TenantPrdCatCount>();
            foreach (TenantPrdCatCount item in data)
            {
                if (isCatCanRead(item.id, memberId, userLvType))
                {
                    if (item.parentId != null)
                    {
                        item.count = item.count + item.pcount;
                    }
                    else
                    {
                        item.count = item.pcount + item.ccount;
                    }
                    result.Add(item);
                }
            }
            return result;
        }

        public static bool isCatCanRead(int catId, int memberId, string userLvType)
        {
            List<TenantPrdCatRead> list = DataManager.tenantPrdCatDao.getCatReads(catId);
            foreach (TenantPrdCatRead item in list)
            {
                if (UserLevelType.NORMAL.Equals(item.type))
                {
                    //normal
                    return true;
                }

                if (userLvType.Equals(item.type))
                {
                    // advanced
                    return true;
                }

                if (item.tenantMemId != null)
                {
                    //vip
                    if (memberId == item.tenantMemId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
