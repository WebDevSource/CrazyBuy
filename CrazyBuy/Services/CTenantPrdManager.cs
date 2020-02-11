using System.Collections.Generic;
using CrazyBuy.Models;

namespace CrazyBuy.Services
{
    public class CTenantPrdManager
    {
        public static readonly string TYPE_MEMBER = "loginUser";
        public static readonly string TYPE_GUEST = "guest";

        public static IEnumerable<Dictionary<string, object>> getPrdList(List<TenantPrd> prds, string userType)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (TenantPrd prd in prds)
            {
                result.Add(getPrdItem(prd, userType));
            }
            return result;
        }

        public static Dictionary<string, object> getPrdItem(TenantPrd prd, string userType)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            Dictionary<string, string> prices = new Dictionary<string, string>();
            string price;
            if (TYPE_MEMBER.Equals(userType))
            {
                price =string.Format("${0}", prd.memberPrice);
                prices.Add(CHType.PRICE_MEMBER, price);
            }
            else
            {
                price = string.Format("${0}", prd.fixedprice);
                prices.Add(CHType.PRICE_NORMAL, price);
            }

            data.Add("prices", prices);
            data.Add("id", prd.id);
            data.Add("name", prd.name);
            data.Add("prdCode", prd.prdCode);
            data.Add("tenantId", prd.tenantId);
            data.Add("summary", prd.summary);
            data.Add("prdImages", prd.prdImages);
            data.Add("tags", prd.SpecialRule);
            return data;
        }

    }
}
