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
    [Route("api/[controller]/[action]")]
    public class PrdController : Controller
    {
        [HttpGet("{tenantId}")]
        [Authorize]
        public IEnumerable<Dictionary<string, object>> getHomePrdList(string tenantId)
        {
            string type = User.Claims.FirstOrDefault(p => p.Type == "type").Value;
            List<TenantPrd> data = DataManager.tenantPrdDao.getHomePrds(Guid.Parse(tenantId));
            IEnumerable<Dictionary<string, object>> prds = CTenantPrdManager.getPrdList(data, type);
            return prds;
        }

        [HttpGet("{prdId}")]
        [Authorize]
        public Dictionary<string, object> getHomePrdItem(string prdId)
        {
            string type = User.Claims.FirstOrDefault(p => p.Type == "type").Value;
            TenantPrd prd = DataManager.tenantPrdDao.getTenandPrd(Guid.Parse(prdId));
            return CTenantPrdManager.getPrdItem(prd, type);
        }
    }
}
