using CrazyBuy.DAO;
using CrazyBuy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TenantController : Controller
    {
        [HttpGet("{tenantCode}")]
        public ActionResult isExist(string tenantCode)
        {
            ReturnMessage rm = new ReturnMessage();
            bool isExist = false;
            try
            {
                Tenant tenant = DataManager.tenantDao.getTenantByTenantCode(tenantCode);
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

        [HttpGet]
        [Authorize]
        public ActionResult getFAQ()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                Guid tenantId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value);
                rm.code = MessageCode.SUCCESS;
                rm.data = DataManager.tenantFAQDao.getFAQ(tenantId);
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e;
            }
            return Ok(rm);
        }

        [HttpGet]
        [Authorize]
        public ActionResult getBulletin()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                Guid tenantId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value);
                rm.code = MessageCode.SUCCESS;
                rm.data = DataManager.tenantBulletinDao.getBulletin(tenantId);
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }
        [HttpPost]
        public ActionResult Register([FromBody]int memberId, Tenant tenant)
        {
            ReturnMessage rm = new ReturnMessage();
            Guid tenantId = Guid.NewGuid();

            try
            {
                tenant.tenantId = tenantId;
                tenant.status = "審核中";
                tenant.createTime = DateTime.Now;
                tenant.updateTime = DateTime.Now;

                DataManager.tenantDao.addTenant(tenant);

                rm.code = MessageCode.SUCCESS;
                rm.data = tenantId;
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
