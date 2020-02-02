using System;
using System.Collections.Generic;
using System.Linq;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using CrazyBuy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]")]
    public class PrdController : Controller
    {        
        [HttpGet("{tenantId}")]
        [Authorize]
        public IEnumerable<Dictionary<string, object>> Get(string tenantId)
        {
            string type = User.Claims.FirstOrDefault(p => p.Type == "type").Value;
            List<TenantPrd> data = DataManager.tenantPrdDao.getHomePrds(Guid.Parse(tenantId));
            IEnumerable<Dictionary<string, object>> prds = CTenantPrdManager.getPrdList(data,type);
            return prds;
        }

    }
}
