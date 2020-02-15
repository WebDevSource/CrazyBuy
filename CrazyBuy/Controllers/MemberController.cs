using System;
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
        // POST api/values
        [HttpPost("{tenantId}")]
        public ActionResult Add([FromBody]Member member, string tenantId)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string account = member.account;
                string phone = member.cellphone;
                string email = member.email;
                string pwd = member.password;

                Member saveMemberByAccount = DataManager.memberDao.getMember(account, pwd);
                if (saveMemberByAccount != null)
                {
                    rm.code = MessageCode.ERROR;
                    rm.data = "account already exist.";
                    return Ok(rm);
                }

                Member saveMemberByPhone = DataManager.memberDao.getMemberByCellPhone(phone, pwd);
                if (saveMemberByPhone != null)
                {
                    rm.code = MessageCode.ERROR;
                    rm.data = "cellphone already exist.";
                    return Ok(rm);
                }

                Member saveMemberByMail = DataManager.memberDao.getMemberByEmail(email, pwd);
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
