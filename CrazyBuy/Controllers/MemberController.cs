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
            string account = member.account;
            string pwd = member.password;

            Member saveMember = DataManager.memberDao.getMember(account, pwd);
            if (saveMember == null)
            {
                member.memberId = DataManager.memberDao.getMemberId();
                member.memberCode = Guid.NewGuid().ToString();
                member.createTime = DateTime.Now;
                member.updateTime = DateTime.Now;
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
