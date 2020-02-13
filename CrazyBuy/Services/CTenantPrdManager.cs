using System;
using System.Collections.Generic;
using CrazyBuy.DAO;
using CrazyBuy.Models;

namespace CrazyBuy.Services
{
    public class CTenantPrdManager
    {
        public static readonly string TYPE_MEMBER = "loginUser";
        public static readonly string TYPE_GUEST = "guest";

        public static IEnumerable<Dictionary<string, object>> getPrdListByCat(Guid tenantId, long catId, string userType)
        {
            List<TenantPrd> data = DataManager.tenantPrdDao.getTenandPrdByCatId(tenantId, catId);
            return getPrdList(data, userType);
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

        public static PrdPrice getPrdPrice(TenantPrd prd, string userType)
        {
            PrdPrice prdPrice = new PrdPrice();
            string type;
            int price;

            if (TYPE_MEMBER.Equals(userType))
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

        public static Dictionary<string, object> getPrdItem(TenantPrd prd, string userType)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            Dictionary<string, string> prices = new Dictionary<string, string>();
            string price;
            PrdPrice prdPrice = getPrdPrice(prd, userType);
            price = string.Format("${0}", prdPrice.price);
            prices.Add(prdPrice.type, price);
            data.Add("prices", prices);
            data.Add("id", prd.id);
            data.Add("name", prd.name);
            data.Add("prdCode", prd.prdCode);
            data.Add("tenantId", prd.tenantId);
            data.Add("summary", prd.summary);
            data.Add("prdImages", prd.prdImages);
            data.Add("paymentType", prd.paymentType);
            data.Add("shipType", prd.shipType);
            data.Add("tags", prd.SpecialRule);
            return data;
        }

    }
}
