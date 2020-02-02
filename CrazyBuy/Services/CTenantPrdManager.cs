using System.Collections.Generic;
using CrazyBuy.Models;

namespace CrazyBuy.Services
{
    public class CTenantPrdManager
    {
        public static readonly string TYPE_MEMBER = "loginUser";
        public static readonly string TYPE_GUEST = "guest";

        public static IEnumerable<Dictionary<string, object>> getPrdList(List<TenantPrd> prds,string userType)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            foreach (TenantPrd prd in prds)
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                int price;
                if (TYPE_MEMBER.Equals(userType))
                {
                    price = prd.memberPrice;
                }
                else
                {
                    price = prd.fixedprice;
                }

                data.Add("price",price);
                data.Add("id", prd.id);
                data.Add("name", prd.name);
                data.Add("prdCode", prd.prdCode);
                data.Add("tenantId", prd.tenantId);
                data.Add("summary", prd.summary);
                data.Add("prdImages", prd.prdImages);
                result.Add(data);
            }
            return result;
        }
    }
}
