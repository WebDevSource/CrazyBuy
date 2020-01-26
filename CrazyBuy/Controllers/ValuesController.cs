using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : RootController
    {
        // GET api/values/anonymous
        /// <summary>使用匿名登入，無視於身分驗證</summary>

        [HttpGet]
        [Authorize]
        public IActionResult Anonymous()
        {
            GetAuthorizeInfo();
            Debug.WriteLine("anonymous");
            return Ok();
        }

        // GET api/values/authorize
        /// <summary>使用身分驗證，HTTP 的 Authorization Header 必須設定合法的 JWT Bearer Token 才能使用</summary>
        [Authorize]
        [HttpGet("{function}")]
        public IActionResult All(string function)
        {
            Debug.WriteLine("authorize");
            return new ContentResult() { Content = "For all client who authorize." };
        }
    }
}
