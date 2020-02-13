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
        [HttpGet]
        [Authorize]
        public ActionResult getHomePrdList()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string type = User.Claims.FirstOrDefault(p => p.Type == "type").Value;
                string tenantId = User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value;
                List<TenantPrd> data = DataManager.tenantPrdDao.getHomePrds(Guid.Parse(tenantId));
                IEnumerable<Dictionary<string, object>> prds = CTenantPrdManager.getPrdList(data, type);
                rm.code = MessageCode.SUCCESS;
                rm.data = prds;
            }
            catch (Exception e)
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
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        [HttpGet]
        [Authorize]
        public ActionResult getRootCatList()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string tenantId = User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value;
                rm.code = MessageCode.SUCCESS;
                rm.data = CTenantPrdCatManager.getRootCats(Guid.Parse(tenantId));
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult getTreeCatList(long id)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string tenantId = User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value;
                rm.code = MessageCode.SUCCESS;
                rm.data = CTenantPrdCatManager.getTreeCats(Guid.Parse(tenantId), id);
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        [HttpGet]
        [Authorize]
        public ActionResult getPrdCats()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string tenantId = User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value;
                rm.code = MessageCode.SUCCESS;
                rm.data = DataManager.tenantPrdCatDao.getAllPrdCats(Guid.Parse(tenantId));
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
