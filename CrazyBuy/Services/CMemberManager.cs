using System;
using System.Diagnostics;
using CrazyBuy.Common;
using CrazyBuy.DAO;
using CrazyBuy.Models;

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
                member.password = Utils.MD5_Encode(member.password);
                int memberId = DataManager.memberDao.addMember(member);

                TenantMember tenantMember = new TenantMember();
                tenantMember.tenantId = tenantId;
                tenantMember.memberId = memberId;
                tenantMember.isBlockade = false;
                tenantMember.status = "0";
                tenantMember.creator = member.memberId;
                tenantMember.createTime = now;
                tenantMember.updateTime = now;
                DataManager.tenantMemberDao.addTenantMember(tenantMember);

                isV = true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("[CMemberManager-addMember] error:" + e.Message);
            }
            return isV;
        }
    }
}
