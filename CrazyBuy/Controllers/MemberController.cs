using System;
using System.Linq;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using CrazyBuy.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]")]
    public class MemberController : Controller
    {
        [HttpGet]
        public ActionResult getMember()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                int memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                Member member = DataManager.memberDao.getMember(memberId);
                member.password = "";

                rm.code = MessageCode.SUCCESS;
                rm.data = member;
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        // POST api/values
        [HttpPost("{tenantId}")]
        public ActionResult Add([FromBody]Member member, string tenantId)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string phone = member.cellphone;
                string email = member.email;
                string pwd = member.password;

                Member saveMemberByPhone = DataManager.memberDao.getMemberByCellPhone(Guid.Parse(tenantId), phone, pwd);
                if (saveMemberByPhone != null)
                {
                    rm.code = MessageCode.ERROR;
                    rm.data = "cellphone already exist.";
                    return Ok(rm);
                }

                Member saveMemberByMail = DataManager.memberDao.getMemberByEmail(Guid.Parse(tenantId), email, pwd);
                if (saveMemberByMail != null)
                {
                    rm.code = MessageCode.ERROR;
                    rm.data = "mail already exist.";
                    return Ok(rm);
                }

                bool isV = CMemberManager.addMember(member, Guid.Parse(tenantId));
                if (isV)
                {
                    rm.code = MessageCode.SUCCESS;
                    rm.data = "member add success.";
                }
                else
                {
                    rm.code = MessageCode.ERROR;
                    rm.data = "service error.";
                }
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
