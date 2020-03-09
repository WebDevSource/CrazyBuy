using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]")]
    public class CommonController : Controller
    {
        private readonly IConfiguration _config;

        [HttpGet("DownloadImgFile")]
        public ActionResult GetDownloadLogoFile(int id, string filename)
        {
            //string strFileRootPath = _config["PrdPath"];
            //string filePath = string.Empty;
            //TenantPrd prd = DataManager.tenantPrdDao.getTenandPrd(id);
            //try
            //{
            //    List<UploadFileModel> lstlogofile = JsonConvert.DeserializeObject<List<UploadFileModel>>(prd.prdImages);
            //    if (lstlogofile != null && lstlogofile.Count > 0)
            //    {
            //        UploadFileModel logofile = lstlogofile.Where(x => x.filename == filename).FirstOrDefault();
            //        filePath = strFileRootPath + "/" + prd.id.ToString() + "/" + logofile.filename;
            //        var memoryStream = System.IO.File.OpenRead(filePath);
            //        return new FileStreamResult(memoryStream, _contentTypes[Path.GetExtension(filePath).ToLowerInvariant()]);
            //    }
            //    else
            //    {
            //        return Ok();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, ex);
            //}
            return Ok();
        }
    }
}
