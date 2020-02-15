using CrazyBuy.Models;
using System;
using System.Collections.Generic;

namespace CrazyBuy
{
    // 回傳狀態碼
    public class MessageCode
    {
        public const int SUCCESS = 1;
        public const int ERROR = -1;
    }

    public class UserType
    {
        public const string ADMIN = "admin";
        public const string MEMBER = "member";
        public const string GUEST = "guest";
    }

    public class LoginType
    {
        public const string LOGIN_USER = "loginUser";
        public const string GUEST = "guest";
    }

    public class UserInfo
    {
        public Guid tnenatId;
        public Guid memberId;
        public string userType;
    }

    // 回傳格式
    public class ReturnMessage
    {
        public int code { get; set; }
        public object data { get; set; }
    }

    // Auth格式
    public class AuthorizeInfo
    {
        public string headerAuthorization { get; set; }
        public string remoteIp { get; set; }
    }

    public class CHType
    {
        public const string PRICE_MEMBER = "member";
        public const string PRICE_NORMAL = "normal";
        public const string PRICE_NTRANS = "NTrans";
        public const string PRICE_HTRANS = "HTrans";
        public const string PRICE_VIPTRANS = "VIPTrans";
    }

    public class ShopCartRequest
    {
        public Guid productId { get; set; }
        public int count { get; set; }
    }

    public class PrdPrice
    {
        public int price { get; set; }
        public string type { get; set; }
    }

    public class OrderData
    {
        public OrderMaster master { get; set; }
        public List<OrderDetail> detail { get; set; }
    }

    public class SortType
    {
        public const int DATE_ASC = 0;
        public const int NAME_ASC = 1;
        public const int NAME_DESC = 2;
        public const int PRICE_ASC = 3;
        public const int PRICE_DESC = 4;
        public static string getOrderBy(int type)
        {
            Dictionary<int, string> map = new Dictionary<int, string>();
            map.Add(DATE_ASC, @" order by p.createTime asc ");
            map.Add(NAME_ASC, @" order by p.name asc ");
            map.Add(NAME_DESC, @" order by p.name desc ");
            map.Add(PRICE_ASC, @" order by p.memberPrice asc ");
            map.Add(PRICE_DESC, @" order by p.memberPrice desc ");
            return map.GetValueOrDefault(type);
        }

    }

    public class SortTypeRequest
    {
        public int sortType { get; set; }
        public int count { get; set; }
        public int page { get; set; }
    }

    public class SqlQueryTotal
    {
        public int total { get; set; }
    }

    public class PrdPageQuery
    {
        public PrdPageQuery(SortTypeRequest value)
        {
            count = value.count;
            page = value.page - 1;
            sortType = value.sortType;
        }
        public Guid tnenatId { get; set; }
        public long catId { get; set; }
        public string userType { get; set; }
        public int sortType { get; set; }
        public int count { get; set; }
        public int page { get; set; }
        public string getKey()
        {
            return tnenatId.ToString() + catId + sortType + count + page;
        }
    }

    public class PrdPageResponse
    {
        public int maxPage { get; set; }
        public int page { get; set; }
        public object result { get; set; }
    }
}
