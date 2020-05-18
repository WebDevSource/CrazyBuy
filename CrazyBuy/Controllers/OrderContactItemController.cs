using CrazyBuy.DAO;
using CrazyBuy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]")]
    public class OrderContactItemController : Controller
    {
        [HttpGet("{orderId}")]
        [Authorize]
        public ActionResult Get(int orderId)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                rm.code = MessageCode.SUCCESS;
                rm.data = DataManager.orderContactItemDAO.getListByOrderId(orderId);
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }
        [HttpPut]
        [Authorize]
        public ActionResult add([FromBody]OrderContactItem args)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                int memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                DateTime now = DateTime.Now;
                args.dtContact = now;
                args.createTime = now;
                args.creator = memberId;
                args.status = "正常";
                DataManager.orderContactItemDAO.add(args);
                rm.code = MessageCode.SUCCESS;
                rm.data = "add success.";

                OrderMaster master = DataManager.orderDao.getOrderMaster(args.orderId);
                if(master.payStatus != "已收到貨款")
                {
                    master.payStatus = "貨款確認中";
                    DataManager.orderDao.updateOrderMaster(master);
                }
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e;
            }
            return Ok(rm);
        }
    }
}
