using CrazyBuy.DAO;
using CrazyBuy.Models;
using Microsoft.AspNetCore.Mvc;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TenantController : Controller
    {
        [HttpGet("{tenantId}")]
        public ActionResult isExist(string tenantId)
        {
            ReturnMessage rm = new ReturnMessage();
            bool isExist = false;
            try
            {
                Guid id = Guid.Parse(tenantId);
                Tenant tenant = DataManager.tenantDao.getTenant(id);
                if (tenant != null)
                {
                    isExist = true;
                }
                rm.code = MessageCode.SUCCESS;
                rm.data = isExist;
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
