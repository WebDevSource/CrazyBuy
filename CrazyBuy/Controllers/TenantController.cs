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
        public ActionResult Register([FromBody]TenantRegister tenantRegister)
        {   
            ReturnMessage rm = new ReturnMessage();
            Guid tenantId = Guid.NewGuid();

            //Tenant tenant = new Tenant();

            try
            {
                Tenant tenant = DataManager.tenantDao.getTenantByTenantCode(tenantRegister.tenantCode);
                if (tenant == null)
                {
					
					Member member = new Member()
                    {
                        memberCode = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                        password = tenantRegister.memberPwd,
                        name = tenantRegister.memberName,
                        cellphone = tenantRegister.cellphone,
                        email = tenantRegister.email,
                        address = tenantRegister.address,
                        lineId = tenantRegister.lineId,
                        tenantType = tenantRegister.tenantType,
                        status = "正常",
                        creator = 0,
                        cityId = int.Parse(tenantRegister.cityId),
                        townId = int.Parse(tenantRegister.townId),
                        zipCode = int.Parse(tenantRegister.zipCode),
                        createTime = DateTime.Now,
                    };
                    int memberId = DataManager.memberDao.addMember(member);
                    Console.WriteLine("memberId = {0}", memberId);

                    Tenant addTenant = new Tenant()
                    {
                        tenantId = tenantId,
                        createdMemberId = memberId,
                        tenantCode = tenantRegister.tenantCode,
                        tenantName = tenantRegister.tenantName,
                        enterpriseName = tenantRegister.enterpriseName,
                        enterpriseId = tenantRegister.enterpriseId,
                        language = "zn-TW",
                        owner = tenantRegister.owner,
                        FBCommunity = tenantRegister.FBCommunity,
                        FBFan = tenantRegister.FBFan,
                        sortIndex = 1,
                        hasCustPriceGrade = false,
                        status = "待審核",
                        creator = memberId,
                        createTime = DateTime.Now
                    };
                    DataManager.tenantDao.addTenant(addTenant);

                    TenantMember tenantMember = new TenantMember()
                    { 
                        tenantId = tenantId,
                        memberId = memberId,
                        isBlockade = false,
                        status = "待審核",
                        createTime = DateTime.Now,
                        creator = memberId
                    };
                    DataManager.tenantMemberDao.addTenantMember(tenantMember);


                    //tenant.tenantId = tenantId;
                    //tenant.createdMemberId = 0;
                    //tenant.status = "待審核";
                    //tenant.createTime = DateTime.Now;
                    //tenant.updateTime = DateTime.Now;

                    //DataManager.tenantDao.addTenant(tenant);

                    rm.code = MessageCode.SUCCESS;
                    rm.data = tenantId;
                }
                else
                {
                    rm.code = MessageCode.ERROR;
                    rm.data = "tenantCode already exist.";
                    return Ok(rm);
                }
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        [Authorize]
        public ActionResult getTenantSetting()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                Guid tenantId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value);
                rm.code = MessageCode.SUCCESS;
                rm.data = DataManager.tenantDao.getAllTenantSetting(tenantId);
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
