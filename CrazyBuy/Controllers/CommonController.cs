using System;
using System.Collections.Generic;
using System.Linq;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CommonController : Controller
    {
        [HttpGet]
        [Authorize]
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
                shipList.Add(TenantSettingTAG.FACE);
                foreach (ShopCartPrd item in items)
                {
                    total += item.amount;
                    if (item.shipType.Contains(TenantSettingTAG.NOMAL))
                    {
                        shipList.Add(TenantSettingTAG.NOMAL);
                    }

                    if (item.shipType.Contains(TenantSettingTAG.COOL))
                    {
                        shipList.Add(TenantSettingTAG.COOL);
                    }
                }


                //step2 整理運費方式及運費
                List<ShipMethod> methods = new List<ShipMethod>();
                int freePrice = 0;
                foreach (string method in shipList)
                {
                    if (type.ContainsKey(method))
                    {
                        if (method.Equals(TenantSettingTAG.FACE))
                        {
                            freePrice = type.GetValueOrDefault(method);

                            ShipMethod shipMethod = new ShipMethod();
                            shipMethod.method = method;
                            shipMethod.price = 0;
                            methods.Add(shipMethod);
                        }
                        else
                        {
                            ShipMethod shipMethod = new ShipMethod();
                            shipMethod.method = method;
                            shipMethod.price = total > freePrice ? 0 : type.GetValueOrDefault(method);
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
    }
}
