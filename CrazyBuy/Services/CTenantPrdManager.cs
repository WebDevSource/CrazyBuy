using System;
using System.Collections.Generic;
using CrazyBuy.Common;
using CrazyBuy.DAO;
using CrazyBuy.Models;

namespace CrazyBuy.Services
{
    public class CTenantPrdManager
    {
        public static IEnumerable<Dictionary<string, object>> getPrdSearchListByCat(PrdSearchQuery query)
        {
            List<TenantPrd> data = DataManager.tenantPrdDao.getSearchTenandPrdByCatId(query);
            IEnumerable<Dictionary<string, object>> result = getPrdList(data, query.userType);
            return result;
        }

        public static IEnumerable<Dictionary<string, object>> getPrdListByCat(PrdPageQuery query)
        {
            string key = query.getKey() + "getPrdListByCat";
            if (CacheResult.isKeyExist(key))
            {
                return (IEnumerable<Dictionary<string, object>>)CacheResult.getData(key);
            }
            else
            {
                List<TenantPrd> data = DataManager.tenantPrdDao.getTenandPrdByCatId(query);
                IEnumerable<Dictionary<string, object>> result = getPrdList(data, query.userType);
                CacheResult.setData(key, result);
                return result;
            }
        }

        public static IEnumerable<Dictionary<string, object>> getPrdList(List<TenantPrd> prds, string userType)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (TenantPrd prd in prds)
            {
                result.Add(getPrdItem(prd, userType));
            }
            return result;
        }

        //顯示獨立價格
        public static PrdPrice getPrdPrice(TenantPrd prd, string userType)
        {
            PrdPrice prdPrice = new PrdPrice();
            string type;
            int price;

            if (LoginType.LOGIN_USER.Equals(userType))
            {
                price = prd.memberPrice;
                type = CHType.PRICE_MEMBER;
            }
            else
            {
                price = prd.fixedprice;
                type = CHType.PRICE_NORMAL;
            }

            prdPrice.price = price;
            prdPrice.type = type;
            return prdPrice;
        }

        //畫面呈現價格
        public static List<PrdPrice> getPrdPrices(TenantPrd prd, string userType)
        {
            List<PrdPrice> prices = new List<PrdPrice>();
            switch (userType)
            {
                case LoginType.LOGIN_USER:
                    PrdPrice prdPriceUser = new PrdPrice();
                    prdPriceUser.price = prd.fixedprice;
                    prdPriceUser.type = CHType.PRICE_NORMAL;
                    prices.Add(prdPriceUser);

                    prdPriceUser = new PrdPrice();
                    prdPriceUser.price = prd.memberPrice;
                    prdPriceUser.type = CHType.PRICE_MEMBER;
                    prices.Add(prdPriceUser);

                    break;
                case UserType.ADMIN:
                    PrdPrice prdPriceAdmin = new PrdPrice();
                    prdPriceAdmin.price = prd.fixedprice;
                    prdPriceAdmin.type = CHType.PRICE_NORMAL;
                    prices.Add(prdPriceAdmin);

                    prdPriceAdmin = new PrdPrice();
                    prdPriceAdmin.price = prd.memberPrice;
                    prdPriceAdmin.type = CHType.PRICE_MEMBER;
                    prices.Add(prdPriceAdmin);

                    prdPriceAdmin = new PrdPrice();
                    prdPriceAdmin.price = prd.transferPrice;
                    prdPriceAdmin.type = CHType.PRICE_NTRANS;
                    prices.Add(prdPriceAdmin);

                    break;
            }
            return prices;
        }

        public static int getTenantPrdCount(Guid tenantId, long catId)
        {
            string key = tenantId.ToString() + catId + "getTenantPrdCount";
            if (CacheResult.isKeyExist(key))
            {
                return (int)CacheResult.getData(key);
            }
            else
            {
                int count = DataManager.tenantPrdDao.getCountByCatId(tenantId, catId);
                CacheResult.setData(key, count);
                return count;
            }
        }

        public static Dictionary<string, object> getPrdItem(TenantPrd prd, string userType)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            Dictionary<string, string> prices = new Dictionary<string, string>();
            string price;

            List<PrdPrice> prdPrices = getPrdPrices(prd, userType);
            foreach (PrdPrice prdPrice in prdPrices)
            {
                price = string.Format("${0}", prdPrice.price);
                prices.Add(prdPrice.type, price);
            }

            data.Add("prices", prices);
            data.Add("id", prd.id);
            data.Add("name", prd.name);
            data.Add("prdCode", prd.prdCode);
            data.Add("tenantId", prd.tenantId);
            data.Add("summary", prd.summary);
            data.Add("desc", prd.desc);
            data.Add("prdImages", prd.prdImages);
            data.Add("paymentType", prd.paymentType);
            data.Add("shipType", prd.shipType);
            data.Add("tags", prd.SpecialRule);
            return data;
        }

    }
}
