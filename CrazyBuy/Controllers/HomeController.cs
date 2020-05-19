using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CrazyBuy.Common;
using CrazyBuy.DAO;
using CrazyBuy.Models;

namespace CrazyBuy.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("{tenantCode}", Name = "Get")]
        public ActionResult Get(string tenantCode)
        {
            string url = "https://www.winpower365.com/";
            Tenant tenant = DataManager.tenantDao.getTenantByTenantCode(tenantCode);

            if (tenant != null)
            {
                url = string.Format("/CrazyBuy/index.html?tenantCode={0}", tenantCode);
            }

            return Redirect(url);
        }
    }
}