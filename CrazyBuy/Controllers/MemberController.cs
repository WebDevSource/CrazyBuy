using CrazyBuy.Common;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using CrazyBuy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]")]
    public class MemberController : Controller
    {
        [HttpPost("{memberId}")]
        [Authorize]
        public ActionResult update([FromBody]Member member, int memberId)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                member.memberId = memberId;
                member.password = Utils.ConverToMD5(member.password);
                DataManager.memberDao.updateMember(member);
                rm.code = MessageCode.SUCCESS;
                rm.data = memberId + " update success.";
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e;
            }
            return Ok(rm);
        }

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
        [HttpPut("{tenantId}")]
        public ActionResult Add([FromBody]Member member, string tenantId)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string phone = member.cellphone;
                string email = member.email;

                Member saveMemberByPhone = DataManager.memberDao.getMemberByCellPhone(Guid.Parse(tenantId), phone);
                if (saveMemberByPhone != null)
                {
                    rm.code = MessageCode.ERROR;
                    rm.data = "cellphone already exist.";
                    return Ok(rm);
                }

                Member saveMemberByMail = DataManager.memberDao.getMemberByEmail(Guid.Parse(tenantId), email);
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
