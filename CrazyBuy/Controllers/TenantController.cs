using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CrazyBuy.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]")]
    public class TenantController : Controller
    {

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Tenant> Get()
        {
            return TenantManager.getTenantTop(10).ToArray();
        }

        [HttpGet("{function}")]
        public string Get(string function)
        {
            return "function:" + function;
        }

        // POST api/<controller>
        [HttpPost]
        public bool Post([FromBody]Tenant tenant)
        {
            bool isSuccess = false;
            try
            {
                Debug.WriteLine("add:" + tenant.tenantCode);
                TenantManager.updateTenant(tenant);
                isSuccess = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("exception:" + e.Message);
            }
            return isSuccess;
        }

        // PUT api/<controller>/5
        [HttpPut]
        public bool Put([FromBody]Tenant tenant)
        {
            bool isSuccess = false;
            try
            {
                Debug.WriteLine("update:" + tenant.tenantCode);
                TenantManager.saveTenant(tenant);
                isSuccess = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("exception:" + e.Message);
            }
            return isSuccess;
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public ActionResult Delete([FromBody] Tenant tenant)
        {
            TenantManager.removeTenant(tenant);
            return Ok();
        }
    }
}
