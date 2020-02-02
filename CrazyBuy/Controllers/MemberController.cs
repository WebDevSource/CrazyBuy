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
        public ActionResult Add([FromBody]Member member,string tenantId)
        {
            ReturnMessage rm = new ReturnMessage();
            string account = member.account;
            string pwd = member.password;

            Member saveMember = DataManager.memberDao.getMember(account, pwd);
            if (saveMember == null)
            {
                bool isV = CMemberManager.addMember(member,Guid.Parse(tenantId));
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
            else
            {
                rm.code = MessageCode.ERROR;
                rm.data = "member already excited.";
            }
            return Ok(rm);
        }
    }
}
