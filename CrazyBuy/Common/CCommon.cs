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
        public const int PRD_NOT_ENOUGHT = -2;
    }

    public class UserType
    {
        public const string ADMIN = "admin";
        public const string MEMBER = "member";
        public const string SPC_MEMBER = "spc_member";
        public const string GUEST = "guest";
    }

    public class UserLevelType
    {
        public const string NORMAL = "所有會員";
        public const string ADVANCED = "進階會員";
        public const string VIP = "特定會員";        
    }

    public class UserGradeType
    {
        public const string TRANS = "轉批價";
        public const string CUSTOM = "自訂價";
    }

    public class UserDisType
    {
        public const string NO_DIS = "不適用任何優惠";
    }

    public class LoginType
    {
        public const string LOGIN_USER = "loginUser";
        public const string GUEST = "guest";
    }

    public class UserInfo
    {
        public Guid tnenatId;
        public int memberId;
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
        public int productId { get; set; }
        public int count { get; set; }
        public string sepc { get; set; }
    }

    public class PrdPrice
    {
        public int price { get; set; }
        public string type { get; set; }
    }

    public class OrderData
    {
        public OrderMasterUser master { get; set; }
        public List<OrderPrdDetail> detail { get; set; }
        public List<OrderContactItem> contactList { get; set; }
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

    public class SearchRequest
    {
        public int sortType { get; set; }
        public int count { get; set; }
        public int page { get; set; }
        public int catId { get; set; }
        public string search { get; set; }
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
            return tnenatId.ToString() + userType + catId + sortType + count + page;
        }
    }

    public class PrdSearchQuery
    {
        public PrdSearchQuery(SearchRequest value)
        {
            count = value.count;
            page = value.page - 1;
            sortType = value.sortType;
            name = value.search;
        }
        public Guid tnenatId { get; set; }
        public long catId { get; set; }
        public string search { get; set; }
        public string userType { get; set; }
        public int sortType { get; set; }
        public int count { get; set; }
        public int page { get; set; }
        public string name { get; set; }
    }

    public class PrdPageResponse
    {
        public int maxPage { get; set; }
        public int page { get; set; }
        public object result { get; set; }
    }

    public class CustSpcPrice
    {
        public int price { get; set; }
        public string name { get; set; }
    }

    public class TenantSettingType
    {
        public const string FreeFreight = "免運設定";
        public const string RoomTemperatureFreight = "低溫運費";
        public const string RefrigerationFreigh = "常溫運費";
    }

    public class TenantSettingTAG
    {
        public const string FREE = "免運費用";
        public const string FACE = "面交自取";
        public const string COOL = "低溫宅配";
        public const string NOMAL = "常溫宅配";
    }

    public class TenantSettingMapping
    {

        public static string getType(string type)
        {
            Dictionary<string, string> map = new Dictionary<string, string>();
            map.Add(TenantSettingType.RoomTemperatureFreight, TenantSettingTAG.COOL);
            map.Add(TenantSettingType.RefrigerationFreigh, TenantSettingTAG.NOMAL);
            map.Add(TenantSettingType.FreeFreight, TenantSettingTAG.FACE);
            return map.GetValueOrDefault(type);
        }
    }

    public class ShipMethod
    {
        public string method { get; set; }
        public int price { get; set; }
    }

    public class UploadFileModel
    {
        public string filename { get; set; }
        public string filetype { get; set; }
        public int filesize { get; set; }
    }
}
