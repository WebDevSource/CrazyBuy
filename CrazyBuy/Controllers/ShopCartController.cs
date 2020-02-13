﻿using CrazyBuy.DAO;
using CrazyBuy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]")]
    public class ShopCartController : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                Guid memberId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                rm.code = MessageCode.SUCCESS;
                rm.data = DataManager.shopCartDao.getItemsByMember(memberId);
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult Get(Guid id)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                rm.code = MessageCode.SUCCESS;
                rm.data = DataManager.shopCartDao.getItem(id);
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize]
        public ActionResult Post([FromBody]ShopCart value)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                DataManager.shopCartDao.updateItem(value);
                rm.code = MessageCode.SUCCESS;
                rm.data = value;
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        // PUT api/<controller>/5
        [HttpPut]
        [Authorize]
        public ActionResult Put([FromBody]ShopCartRequest value)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                Guid memberId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                Guid tenantId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value);
                Guid itemId = Guid.NewGuid();
                ShopCart shopCart = new ShopCart();
                shopCart.id = itemId;
                shopCart.memberId = memberId;
                shopCart.productId = value.productId;
                shopCart.createTime = DateTime.Now;
                shopCart.count = value.count;
                shopCart.amount = value.amount;
                shopCart.tenantId = tenantId;
                DataManager.shopCartDao.addItem(shopCart);
                rm.code = MessageCode.SUCCESS;
                rm.data = itemId;
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(Guid id)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                DataManager.shopCartDao.removeItem(id);
                rm.code = MessageCode.SUCCESS;
                rm.data = id.ToString() + " delete success.";
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        [HttpDelete]
        [Route("all")]
        [Authorize]
        public ActionResult DeleteAll()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                Guid memberId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                DataManager.shopCartDao.removeItemsByMember(memberId);
                rm.code = MessageCode.SUCCESS;
                rm.data = "all delete success.";
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