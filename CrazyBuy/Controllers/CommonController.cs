using System;
using System.Collections.Generic;
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
    }
}
