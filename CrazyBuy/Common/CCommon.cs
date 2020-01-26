namespace CrazyBuy
{
    public class MessageCode
    {
        public readonly static int SUCCESS = 1;
        public readonly static int ERROR = -1;
    }

    public class ReturnMessage
    {
        public int code { get; set; }
        public object data { get; set; }
    }

    public class AuthorizeInfo
    {
        public string headerAuthorization { get; set; }
        public string remoteIp { get; set; }
    }

    public class ServiceUrl
    {
        public string localServiceUrl { get; set; }
        public string publicServiceUrl { get; set; }
    }

    public class PageParam
    {
        public int pageSize { get; set; }
        public int currentPage { get; set; }
    }

    public class PageInfo
    {
        public int total { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
    }

}
