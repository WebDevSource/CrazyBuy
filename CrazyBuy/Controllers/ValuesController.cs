using System;
using System.Collections.Generic;
using System.Linq;
using CrazyBuy.Common;
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
            var name = User.Identity.Name;
            var jwt_id = User.Claims.FirstOrDefault(p => p.Type == "jti").Value;
            var all_Data = User.Claims.Select(p => new { p.Type, p.Value });
            return Ok(all_Data);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult getData([FromBody]Dictionary<string, string> data)
        {
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public ActionResult clearCache()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                CacheResult.clearCache();
                rm.code = MessageCode.SUCCESS;
                rm.data = "clear cache success.";
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }
    }
}
