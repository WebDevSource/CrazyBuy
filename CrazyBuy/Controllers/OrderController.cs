using CrazyBuy.DAO;
using CrazyBuy.Models;
using CrazyBuy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        [HttpPut]
        [Authorize]
        public ActionResult addOrder([FromBody]OrderMaster value)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string type = User.Claims.FirstOrDefault(p => p.Type == "type").Value;
                int memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                Guid tenantId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value);

                UserInfo info = new UserInfo();
                info.memberId = memberId;
                info.tnenatId = tenantId;
                info.userType = type;

                rm.code = MessageCode.SUCCESS;
                rm.data = COrderManager.addOrder(value, info);
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
        public ActionResult getOrder(int id)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                rm.code = MessageCode.SUCCESS;
                rm.data = COrderManager.getOrderData(id);
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult deleteOrder(int id)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                DataManager.orderDao.removeOrder(id);
                rm.code = MessageCode.SUCCESS;
                rm.data = id + " remove success.";
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
        public ActionResult getOrderByMember()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                int memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                rm.code = MessageCode.SUCCESS;
                List<OrderMaster> data = DataManager.orderDao.getOrderByMember(memberId);
                rm.data = data;
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        [HttpPost]
        [Authorize]
        public ActionResult getOrderByMemberSearch([FromBody]OrderSearch orderSearch)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                int memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                rm.code = MessageCode.SUCCESS;
                List<OrderMaster> data = DataManager.orderDao.getOrderByMemberSearch(memberId, orderSearch);
                rm.data = data;
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
