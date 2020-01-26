using System.Linq;
using CrazyBuy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValuesController : RootController
    {
        // GET api/values/anonymous
        /// <summary>使用匿名登入，無視於身分驗證</summary>

        [HttpGet]
        [Authorize]
        public IActionResult authorize()
        {
            GetAuthorizeInfo();
            var name = User.Identity.Name;
            var jwt_id = User.Claims.FirstOrDefault(p => p.Type == "CustomClaim").Value;
            var all_Data = User.Claims.Select(p => new { p.Type, p.Value });
            return Ok(all_Data);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult getData()
        {
            Tenant tenant = new Tenant();
            tenant.tenantCode = "8888-8888";

            ReturnMessage rm = new ReturnMessage();
            rm.code = 1;
            rm.data = tenant;
            return Ok(rm);
        }
    }
}
