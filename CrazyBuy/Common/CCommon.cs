    namespace CrazyBuy
{
    // 回傳狀態碼
    public class MessageCode
    {
        public readonly static int SUCCESS = 1;
        public readonly static int ERROR = -1;
    }

    public class UserType
    {
        public readonly static string ADMIN = "admin";
        public readonly static string NORMAL = "nomal";
        public readonly static string GUEST = "guest";
    }

    public class LoginType
    {
        public readonly static string LOGIN_USER = "loginUser";
        public readonly static string GUEST = "guest";
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
        public readonly static string PRICE_MEMBER = "member";
        public readonly static string PRICE_NORMAL = "normal";        
    }

}
