using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]")]
    public class RootController : Controller
    {
        public void GetAuthorizeInfo()
        {
            string headerAuthorization = Request.Headers["Authorization"].ToString();
            string remoteIp = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            Debug.WriteLine("headerAuthorization:" + headerAuthorization);
            Debug.WriteLine("remoteIp:" + remoteIp);
        }
    }
}
