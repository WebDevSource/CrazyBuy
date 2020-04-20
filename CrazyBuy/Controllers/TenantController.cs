using CrazyBuy.Common;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TenantController : Controller
    {
        [HttpGet("{tenantCode}")]
        public ActionResult isExist(string tenantCode)
        {
            ReturnMessage rm = new ReturnMessage();
            bool isExist = false;
            try
            {
                Tenant tenant = DataManager.tenantDao.getTenantByTenantCode(tenantCode);
                if (tenant != null)
                {
                    isExist = true;
                }
                rm.code = MessageCode.SUCCESS;
                rm.data = isExist;
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
        public ActionResult getFAQ()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                Guid tenantId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value);
                rm.code = MessageCode.SUCCESS;
                rm.data = DataManager.tenantFAQDao.getFAQ(tenantId);
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e;
            }
            return Ok(rm);
        }

        [HttpGet]
        [Authorize]
        public ActionResult getBulletin()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                Guid tenantId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value);
                rm.code = MessageCode.SUCCESS;
                rm.data = DataManager.tenantBulletinDao.getBulletin(tenantId);
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        [HttpPost]
        public ActionResult Register([FromBody]TenantRegister tenantRegister)
        {
            ReturnMessage rm = new ReturnMessage();

            try
            {
                #region 判斷 商店網址、手機、Email 是否有重複
                string strError = "";
                List<string> tenantCodes = new List<string>();
                //List<string> cellphones = new List<string>();
                //List<string> emails = new List<string>();

                foreach (ServiceItem item in tenantRegister.serviceItem)
                {
                    if (item.tenantType != "LINE公告系統")
                        tenantCodes.Add(item.tenantCode);

                    //cellphones.Add(item.cellphone);
                    //emails.Add(item.email);
                }

                //判斷 商店網址、手機、Email 是否有重複
                List<string> repeatTenantCodes = DataManager.tenantDao.checkTenantCodeIsExist(tenantCodes);
                //List<string> repeatCellphones = DataManager.memberDao.checkMemberCellPhoneIsExist(cellphones);
                //List<string> repeatEmails = DataManager.memberDao.checkMemberEmailIsExist(emails);

                foreach (string itemTenantCode in repeatTenantCodes)
                {
                    strError += string.Format("{0} 商店網址已存在，請重新輸入\n", itemTenantCode);
                }
                //foreach (string itemCellphone in repeatCellphones)
                //{
                //    strError += string.Format("{0} 手機號碼已存在，請重新輸入\n", itemCellphone);
                //}
                //foreach (string itemEmail in repeatEmails)
                //{
                //    strError += string.Format("{0} Email已存在，請重新輸入\n", itemEmail);
                //}

                if (strError != "")
                {
                    rm.code = -999;
                    rm.data = strError;
                    return Ok(rm);
                }

                #endregion

                #region 開始新增資料
                foreach (ServiceItem serviceItem in tenantRegister.serviceItem)
                {
                    Guid tenantId = Guid.NewGuid();
                    DateTime dtNow = DateTime.Now;

                    //會員
                    Member member = new Member()
                    {
                        memberCode = dtNow.ToString("yyyyMMddHHmmssfff"),
                        password = Utils.ConverToMD5(serviceItem.memberPwd),
                        name = tenantRegister.memberName,
                        cellphone = serviceItem.cellphone,
                        email = serviceItem.email,
                        address = tenantRegister.address,
                        lineId = tenantRegister.lineId,
                        tenantType = serviceItem.tenantType,
                        status = "正常",
                        creator = 0,
                        cityId = int.Parse(tenantRegister.cityId),
                        townId = int.Parse(tenantRegister.townId),
                        zipCode = int.Parse(tenantRegister.zipCode),
                        createTime = dtNow,
                    };
                    int memberId = DataManager.memberDao.addMember(member);

                    //租戶
                    string notes = "";
                    notes += (string.IsNullOrEmpty(serviceItem.FBCommunity)) ? "" : string.Format("Facebook社團名稱：{0} \n", serviceItem.FBCommunity);
                    notes += (string.IsNullOrEmpty(serviceItem.FBFan)) ? "" : string.Format("Facebook粉絲專頁：{0} \n", serviceItem.FBFan);
                    notes += (string.IsNullOrEmpty(serviceItem.LineOfficialAccount)) ? "" : string.Format("Line官方帳號：{0} \n", serviceItem.LineOfficialAccount);

                    Tenant addTenant = new Tenant()
                    {
                        tenantId = tenantId,
                        createdMemberId = memberId,
                        tenantCode = (serviceItem.tenantType != "LINE公告系統") ? serviceItem.tenantCode : "00000000-0000-0000",
                        tenantName = (serviceItem.tenantType != "LINE公告系統") ? serviceItem.tenantName : "LineNotify",
                        enterpriseName = tenantRegister.enterpriseName,
                        enterpriseId = tenantRegister.enterpriseId,
                        language = "zn-TW",
                        owner = tenantRegister.owner,
                        FBCommunity = serviceItem.FBCommunity,
                        FBFan = serviceItem.FBFan,
                        sortIndex = 1,
                        hasCustPriceGrade = false,
                        status = "待審核",
                        creator = memberId,
                        createTime = dtNow,
                        notes = notes
                    };
                    DataManager.tenantDao.addTenant(addTenant);

                    ////租戶會員(租戶申請，不需要填寫這一Table)
                    //TenantMember tenantMember = new TenantMember()
                    //{
                    //    tenantId = tenantId,
                    //    memberId = memberId,
                    //    isBlockade = false,
                    //    status = "待審核",
                    //    createTime = dtNow,
                    //    creator = memberId
                    //};
                    //DataManager.tenantMemberDao.addTenantMember(tenantMember);

                    //通知 mail 設定
                    List<MailSend> sendList = new List<MailSend>();
                    MailSend mailSend = new MailSend
                    {
                        memberId = memberId,
                        tenantId = tenantId.ToString(),
                        mail = member.email,
                        tenantName = addTenant.tenantName
                    };
                    sendList.Add(mailSend);

                    MailNotice mailNotice = new MailNotice()
                    {
                        tenantId = tenantId,
                        type = "會員審核提醒",
                        title = "評估審核中",
                        content = "評估審核中",
                        sendTo = JsonConvert.SerializeObject(sendList),
                        isAuto = false,
                        dtSend = dtNow,
                        isSend = false,
                        status = "正常",
                        createTime = dtNow,
                        creator = memberId,
                    };
                    DataManager.mailNoticeDao.add(mailNotice);

                    //租戶帳單相關
                    //取得租戶帳單資訊預設值
                    int amount = 0;
                    var tenantSettings = DataManager.tenantDao.GetDefaultTenantSettings();
                    switch (member.tenantType)
                    {
                        case "團媽":
                            amount = int.Parse(tenantSettings.FirstOrDefault(m => m.title == "GroupBuyMonthlyFee").content);
                            break;
                        case "轉批媽":
                            amount = int.Parse(tenantSettings.FirstOrDefault(m => m.title == "TransferMonthlyFee").content);
                            break;
                        case "批發商":
                            amount = int.Parse(tenantSettings.FirstOrDefault(m => m.title == "WholesaleMonthlyFee").content);
                            break;
                        case "LINE公告系統":
                            amount = int.Parse(tenantSettings.FirstOrDefault(m => m.title == "LineNotify").content);
                            break;
                    }

                    TenantGrade tenantGrade = new TenantGrade()
                    {
                        tenantId = addTenant.tenantId,
                        tenantGrade = member.tenantType,
                        dtStart = dtNow.Date,
                        isLoop = false,
                        status = "正常",
                        createTime = dtNow
                    };
                    int tenantGradeId = DataManager.tenantBillDAO.addTenantGrade(tenantGrade);

                    TenantBill tenantBill = new TenantBill()
                    {
                        tenantId = addTenant.tenantId,
                        tenantGradeId = tenantGradeId,
                        name = string.Format("{0}~{1} 區間的帳單", dtNow.ToString("yyyy/MM/dd"), dtNow.AddDays(30).ToString("yyyy/MM/dd")),
                        dtStart = dtNow.Date,
                        dtDeadline = dtNow.Date.AddDays(30),
                        dtDue = dtNow.Date.AddDays(10),
                        dueAmount = amount,
                        status = "等待繳費",
                        createTime = dtNow
                    };
                    int billId = DataManager.tenantBillDAO.addTenantBill(tenantBill);

                    TenantBillDetail tenantBillDetail = new TenantBillDetail()
                    { 
                        billId = billId,
                        name = tenantBill.name,
                        unitPrice = amount,
                        qty = 1,
                        amount = amount * 1,
                        status = "正常",
                        createTime = dtNow
                    };
                    DataManager.tenantBillDAO.addTenantBillDetail(tenantBillDetail);

                }
                #endregion

                rm.code = MessageCode.SUCCESS;
                rm.data = null;
            }
            catch (Exception e)
            {
                rm.code = MessageCode.ERROR;
                rm.data = e.Message;
            }
            return Ok(rm);
        }

        [Authorize]
        public ActionResult getTenantSetting()
        {
            ReturnMessage rm = new ReturnMessage();
            try
            {
                Guid tenantId = Guid.Parse(User.Claims.FirstOrDefault(p => p.Type == "tenantId").Value);
                rm.code = MessageCode.SUCCESS;
                rm.data = DataManager.tenantDao.getAllTenantSetting(tenantId);
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
