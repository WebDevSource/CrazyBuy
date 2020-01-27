using System;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]")]
    public class MemberController : Controller
    {
        // POST api/values
        [HttpPost]
        public ActionResult Add([FromBody]Member member)
        {
            ReturnMessage rm = new ReturnMessage();

            Guid tenantId = member.tenantId;
            string userId = member.userId;
            string pwd = member.userPassword;

            Member saveMember = DataManager.memberDao.getMember(userId, pwd, tenantId);
            if (saveMember == null)
            {
                member.id = Guid.NewGuid();
                DataManager.memberDao.addMember(member);
                rm.code = MessageCode.SUCCESS;
                rm.data = "member add success.";
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
