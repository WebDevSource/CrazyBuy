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
}
