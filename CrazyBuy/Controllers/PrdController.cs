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
        public ActionResult getHomePrdList(string tenantId)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string type = User.Claims.FirstOrDefault(p => p.Type == "type").Value;
                List<TenantPrd> data = DataManager.tenantPrdDao.getHomePrds(Guid.Parse(tenantId));
                IEnumerable<Dictionary<string, object>> prds = CTenantPrdManager.getPrdList(data, type);
                rm.code = MessageCode.SUCCESS;
                rm.data = prds;
            }
            catch(Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }            
            return Ok(rm);
        }

        [HttpGet("{prdId}")]
        [Authorize]
        public ActionResult getHomePrdItem(string prdId)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string type = User.Claims.FirstOrDefault(p => p.Type == "type").Value;
                TenantPrd prd = DataManager.tenantPrdDao.getTenandPrd(Guid.Parse(prdId));
                rm.code = MessageCode.SUCCESS;
                rm.data = CTenantPrdManager.getPrdItem(prd, type);
            }
            catch(Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }           
            return Ok(rm);
        }
    }
}
