namespace CrazyBuy
{
    // 回傳狀態碼
    public class MessageCode
    {
        public readonly static int SUCCESS = 1;
        public readonly static int ERROR = -1;
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

}
