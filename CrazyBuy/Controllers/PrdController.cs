﻿using System;
using System.Collections.Generic;
using System.Linq;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using CrazyBuy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PrdController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult isProductEnough()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                int memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);

                rm = COrderManager.isProductEnough(memberId);
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
        public ActionResult getHomePrdList()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string type = User.Claims.FirstOrDefault(p => p.Type == "type").Value;
                string tenantId = User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value;
                int memberId = -1;
                if (!UserType.GUEST.Equals(type))
                {
                    memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                }
                List<TenantPrd> data = DataManager.tenantPrdDao.getHomePrds(Guid.Parse(tenantId), memberId);
                IEnumerable<Dictionary<string, object>> prds = CTenantPrdManager.getPrdList(data, type, memberId);
                rm.code = MessageCode.SUCCESS;
                rm.data = prds;
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        [HttpGet("{prdId}")]
        [Authorize]
        public ActionResult getHomePrdItem(int prdId)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string type = User.Claims.FirstOrDefault(p => p.Type == "type").Value;
                string tenantId = User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value;
                int memberId = -1;
                if (!UserType.GUEST.Equals(type))
                {
                    memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                }
                TenantGrade tenantGrade = DataManager.tenantDao.GetTenantGrade(Guid.Parse(tenantId));
                string tenantGradeType = "團媽";
                if (tenantGrade != null)
                {
                    tenantGradeType = tenantGrade.tenantGrade;
                }
                TenantPrd prd = DataManager.tenantPrdDao.getTenandPrd(prdId);
                rm.code = MessageCode.SUCCESS;
                rm.data = CTenantPrdManager.getPrdItem(prd, type, memberId, tenantGradeType);
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
        public ActionResult getRootCatList()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string tenantId = User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value;
                rm.code = MessageCode.SUCCESS;
                rm.data = CTenantPrdCatManager.getRootCats(Guid.Parse(tenantId));
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
        public ActionResult getTreeCatList(long id)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string tenantId = User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value;
                rm.code = MessageCode.SUCCESS;
                rm.data = CTenantPrdCatManager.getTreeCats(Guid.Parse(tenantId), id);
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
        public ActionResult getPrdCats()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string tenantId = User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value;
                string userType = User.Claims.FirstOrDefault(p => p.Type == "type").Value;
                int memberId = -1;
                if (!UserType.GUEST.Equals(userType))
                {
                    memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                }
                rm.code = MessageCode.SUCCESS;
                rm.data = CTenantPrdCatManager.getAllCats(Guid.Parse(tenantId), memberId);
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
        public ActionResult getPrdByCatId(long id, [FromQuery] SortTypeRequest request)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                string tenantId = User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value;
                string type = User.Claims.FirstOrDefault(p => p.Type == "type").Value;
                int memberId = -1;
                if (!UserType.GUEST.Equals(type))
                {
                    memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                }
                PrdPageQuery query = new PrdPageQuery(request);
                query.tnenatId = Guid.Parse(tenantId);
                query.catId = id;
                query.userType = type;


                int totalCount = CTenantPrdManager.getTenantPrdCount(Guid.Parse(tenantId), id, memberId);
                int maxPage = totalCount % request.count == 0 ? totalCount / request.count : totalCount / request.count + 1;
                maxPage = totalCount <= request.count ? 1 : maxPage;

                PrdPageResponse response = new PrdPageResponse();
                response.page = request.page;
                response.maxPage = maxPage;
                response.result = CTenantPrdManager.getPrdListByCat(query, memberId);
                rm.code = MessageCode.SUCCESS;
                rm.data = response;
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
        public ActionResult getPrdSearchByCatId([FromBody] SearchRequest request)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                int userId = -1;
                string tenantId = User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value;
                string type = User.Claims.FirstOrDefault(p => p.Type == "type").Value;
                if (!UserType.GUEST.Equals(type))
                {
                    userId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                }
                PrdSearchQuery query = new PrdSearchQuery(request);
                query.tnenatId = Guid.Parse(tenantId);
                query.catId = request.catId;
                query.userType = type;

                int totalCount = CTenantPrdManager.getTenantPrdCount(Guid.Parse(tenantId), request.catId, userId);
                int maxPage = totalCount % request.count == 0 ? totalCount / request.count : totalCount / request.count + 1;
                maxPage = totalCount <= request.count ? 1 : maxPage;

                PrdPageResponse response = new PrdPageResponse();
                response.page = request.page;
                response.maxPage = maxPage;
                response.result = CTenantPrdManager.getPrdSearchListByCat(query, userId);
                rm.code = MessageCode.SUCCESS;
                rm.data = response;
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
