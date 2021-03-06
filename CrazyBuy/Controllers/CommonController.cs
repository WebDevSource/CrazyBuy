﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CrazyBuy.Common;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CommonController : Controller
    {
        private readonly IConfiguration _config;

        public CommonController(IConfiguration configuration)
        {
            _config = configuration;
        }

        [HttpGet]
        public IActionResult DownloadImgFile([FromQuery]int id, [FromQuery]string filename, [FromQuery]string type)
        {

            //TenantPrd prd = DataManager.tenantPrdDao.getTenandPrd(id);
            try
            {
                string strFileRootPath = string.Empty;
                if (type == "prd")
                {
                    strFileRootPath = _config["PrdPath"];
                }
                else if (type == "bulletin")
                {
                    strFileRootPath = _config["BulletinPath"];
                }
                else
                {
                    throw new Exception("type Error");
                }
                //string filePath = string.Empty;
                string filePath = strFileRootPath + "/" + id + "/" + filename;
                var memoryStream = System.IO.File.OpenRead(filePath);
                return new FileStreamResult(memoryStream, _contentTypes[Path.GetExtension(filePath).ToLowerInvariant()]);
                //if (lstlogofile != null && lstlogofile.Count > 0)
                //{
                //    UploadFileModel logofile = lstlogofile.Where(x => x.filename == filename).FirstOrDefault();
                //    filePath = strFileRootPath + "/" + prd.id.ToString() + "/" + logofile.filename;
                //    var memoryStream = System.IO.File.OpenRead(filePath);
                //    return new FileStreamResult(memoryStream, _contentTypes[Path.GetExtension(filePath).ToLowerInvariant()]);
                //}
                //else
                //{
                //    return Ok();
                //}

            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CMemberManager-addMember] error:" + ex);
                Console.WriteLine("[CMemberManager-addMember] error:" + ex);

                return NotFound();
            }
        }

        [HttpGet]
        public ActionResult getPlaces()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                List<City> citys = DataManager.cityDao.getCitys();
                List<Town> towns = DataManager.cityDao.getTowns();
                List<Area> areas = new List<Area>();
                foreach (City city in citys)
                {
                    List<Town> list = new List<Town>();
                    foreach (Town town in towns)
                    {
                        if (town.cityId == city.cityId)
                        {
                            list.Add(town);
                        }
                    }
                    Area area = new Area
                    {
                        name = city.cityName,
                        id = city.cityId,
                        areas = list
                    };
                    areas.Add(area);
                }

                rm.code = MessageCode.SUCCESS;
                rm.data = areas;
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
        public ActionResult getMembmerLevel()
        {
            ReturnMessage rm = new ReturnMessage();
            int memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
            TenantMemLevel level = DataManager.tenantMemberDao.getMemberLevel(memberId);
            if (level != null)
            {
                rm.code = MessageCode.SUCCESS;
                rm.data = level;
            }
            else
            {
                rm.code = MessageCode.ERROR;
                rm.data = "not level member.";
            }
            return Ok(rm);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult getMembmerLevelById(int id)
        {
            ReturnMessage rm = new ReturnMessage();
            TenantMemLevel level = DataManager.tenantMemberDao.getMemberLevelById(id);
            if (level != null)
            {
                rm.code = MessageCode.SUCCESS;
                rm.data = level;
            }
            else
            {
                rm.code = MessageCode.ERROR;
                rm.data = "not level member.";
            }
            return Ok(rm);
        }

        [HttpGet]
        [Authorize]
        public ActionResult getFreight()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                int memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                Guid tenantId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value);

                List<ShopCartPrd> items = DataManager.shopCartDao.getItemsByMember(memberId);
                List<TenantSetting> settings = DataManager.tenantDao.getTenantSetting(tenantId);
                Dictionary<string, int> type = new Dictionary<string, int>();
                TenantMemLevel level = DataManager.tenantMemberDao.getMemberLevel(memberId);
                foreach (TenantSetting setting in settings)
                {
                    if (!string.IsNullOrEmpty(setting.content))
                    {
                        type.Add(TenantSettingMapping.getType(setting.title), int.Parse(setting.content));
                    }
                }

                //step1 統整可選運費的方式
                HashSet<string> shipList = new HashSet<string>();
                int total = 0;
                bool hasOnlyFree = false;

                foreach (ShopCartPrd item in items)
                {
                    total += item.amount;
                    hasOnlyFree = hasOnlyFree || item.SpecialRule.Contains("單一件免運");

                    //if (item.face2faceSet.Contains(TenantSettingTAG.FACE))
                    //{
                    //    shipList.Add(TenantSettingTAG.FACE);
                    //}

                    if (item.shipType.Contains(TenantSettingTAG.FACE))
                    {
                        shipList.Add(TenantSettingTAG.FACE);
                    }

                    if (item.shipType.Contains(TenantSettingTAG.NOMAL))
                    {
                        shipList.Add(TenantSettingTAG.NOMAL);
                    }

                    if (item.shipType.Contains(TenantSettingTAG.COOL))
                    {
                        shipList.Add(TenantSettingTAG.COOL);
                    }

                }
                double discount = 1;
                if (level != null)
                {
                    discount = level.discount * 0.01;
                }

                double totalPrice = Math.Round(Convert.ToDouble(total) * discount, 0);
                //step2 整理運費方式及運費
                List<ShipMethod> methods = new List<ShipMethod>();
                int freePrice = type.GetValueOrDefault(TenantSettingTAG.FACE);
                foreach (string method in shipList)
                {
                    if (type.ContainsKey(method))
                    {
                        if (method.Equals(TenantSettingTAG.FACE))
                        {
                            ShipMethod shipMethod = new ShipMethod();
                            shipMethod.method = method;
                            shipMethod.price = 0;
                            methods.Add(shipMethod);
                        }
                        else
                        {
                            ShipMethod shipMethod = new ShipMethod();
                            shipMethod.method = method;
                            shipMethod.price = totalPrice > freePrice || hasOnlyFree ? 0 : type.GetValueOrDefault(method);
                            methods.Add(shipMethod);
                        }
                    }
                }

                rm.code = MessageCode.SUCCESS;
                rm.data = methods;
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
        public ActionResult getPaymentType()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                int memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                Guid tenantId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value);

                List<ShopCartPrd> items = DataManager.shopCartDao.getItemsByMember(memberId);

                List<String> methods = new List<String>();
                foreach (ShopCartPrd item in items)
                {



                    if (item.paymentType.Contains(TenantSettingTAG.PaymentType_ATM))
                    {
                        methods.Add(TenantSettingType.PaymentType_ATM);
                    }

                    if (item.shipType.Contains(TenantSettingTAG.PaymentType_Face))
                    {
                        methods.Add(TenantSettingType.PaymentType_Face);
                    }


                }

                if (methods.Count == 0)
                {
                    methods.Add(TenantSettingType.PaymentType_Face);
                }

                rm.code = MessageCode.SUCCESS;
                rm.data = methods;
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
        public ActionResult checkMember([FromQuery] VerifyMemberReqest request)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                Guid tenantId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value);
                Member member = DataManager.memberDao.VerifyMemberIsExist(tenantId, request.phone, request.email);
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

        [HttpGet]
        [Authorize]
        public ActionResult RePassWord([FromQuery] RePassMemberReqest request)
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                Member member = DataManager.memberDao.getMember(request.memberId);
                member.password = Utils.ConverToMD5(request.password);
                DataManager.memberDao.updateMember(member);
                rm.code = MessageCode.SUCCESS;
                rm.data = member.memberId + " update success.";
                //rm.code = MessageCode.SUCCESS;
                //rm.data = member;
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
        public ActionResult getBankInfo()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                int memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                Guid tenantId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value);
                List<TenantSetting> settings = DataManager.tenantDao.getAllTenantSetting(tenantId);
                Dictionary<string, string> type = new Dictionary<string, string>();
                foreach (TenantSetting setting in settings)
                {
                    if (!string.IsNullOrEmpty(setting.content))
                    {
                        type.Add(TenantSettingMapping.getType(setting.title), setting.content);
                    }
                }
                var data = new
                {
                    bank = new
                    {
                        bankTitle = type.GetValueOrDefault("bankTitle"),
                        bankCode = type.GetValueOrDefault("bankCode"),
                        bankAccount = type.GetValueOrDefault("bankAccount"),
                        bankName = type.GetValueOrDefault("bankName"),
                        subBankName = type.GetValueOrDefault("subBankName")
                    }
                };


                rm.code = MessageCode.SUCCESS;
                rm.data = data;
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        // GET: api/<controller>
        [HttpGet]
        [Authorize]
        public ActionResult getTimeOutItems()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                int memberId = int.Parse(User.Claims.FirstOrDefault(p => p.Type == "jti").Value);
                rm.code = MessageCode.SUCCESS;
                rm.data = DataManager.shopCartDao.getTimeOutItem(memberId);
                DataManager.shopCartDao.deleteTimeOutItem(memberId);
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
        public ActionResult getBackendUrl()
        {
            ReturnMessage rm = new ReturnMessage();

            rm.code = MessageCode.SUCCESS;
            rm.data = _config["BackendUrl"];
            return Ok(rm);
        }

        private readonly static Dictionary<string, string> _contentTypes = new Dictionary<string, string>
        {
            {".png", "image/png"},
            {".jpg", "image/jpeg"},
            {".jpeg", "image/jpeg"},
            {".gif", "image/gif"}
        };
    }
}
