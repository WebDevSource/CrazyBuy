using System;
using System.Collections.Generic;
using System.Diagnostics;
using CrazyBuy.Common;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using Newtonsoft.Json;

namespace CrazyBuy.Services
{
    public class CMemberManager
    {

        public static bool addMember(Member member, Guid tenantId)
        {
            bool isV = false;
            try
            {
                DateTime now = DateTime.Now;
                member.createTime = now;
                member.updateTime = now;
                member.password = Utils.ConverToMD5(member.password);
                int memberId = DataManager.memberDao.addMember(member);
                Tenant tenant = DataManager.tenantDao.getTenant(tenantId);

                //判斷是否需要寄送mailNotice
                TenantSetting setting = DataManager.tenantDao.getTenantSetting(tenantId, "MemCheckType");
                if (setting != null)
                {
                    TenantSetting mailInfo = null;
                    MailInfo mail = null;
                    string type = null;
                    List<MailSend> sendList = new List<MailSend>();
                    switch (setting.content)
                    {
                        case "Auto":
                            mailInfo = DataManager.tenantDao.getTenantSetting(tenantId, "MemPassMailInfo");
                            mail = JsonConvert.DeserializeObject<MailInfo>(mailInfo.content);
                            type = "會員自動審核";
                            break;
                        case "Manual":
                            mailInfo = DataManager.tenantDao.getTenantSetting(tenantId, "MemReviewMailInfo");
                            mail = JsonConvert.DeserializeObject<MailInfo>(mailInfo.content);
                            type = "會員審核提醒";
                            break;
                    }
                    if (mail != null)
                    {
                        MailSend mailSend = new MailSend
                        {
                            memberId = memberId,
                            tenantId = tenantId.ToString(),
                            mail = member.email,
                            tenantName = tenant.tenantName
                        };
                        sendList.Add(mailSend);
                        Debug.WriteLine("asdsada" + mail.content.Replace("\n", "</br>"));
                        MailNotice mailNotice = new MailNotice
                        {
                            tenantId = tenantId,
                            type = type,
                            title = mail.subject,
                            content = mail.content.Replace("\n", "</br>"),
                            sendTo = JsonConvert.SerializeObject(sendList),
                            isAuto = true,
                            dtSend = DateTime.Now.AddMinutes(10),
                            isSend = false,
                            status = "正常",
                            createTime = DateTime.Now,
                            creator = memberId
                        };
                        DataManager.mailNoticeDao.add(mailNotice);
                    }
                }

                TenantMember tenantMember = new TenantMember();
                tenantMember.tenantId = tenantId;
                tenantMember.memberId = memberId;
                tenantMember.isBlockade = false;
                tenantMember.status = "待審核";
                tenantMember.creator = member.memberId;
                tenantMember.createTime = now;
                tenantMember.updateTime = now;
                DataManager.tenantMemberDao.addTenantMember(tenantMember);

                isV = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("[CMemberManager-addMember] error:" + e);
            }
            return isV;
        }
    }
}
