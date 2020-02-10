using System;
using System.Diagnostics;
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
                Guid id = Guid.NewGuid();
                DateTime now = DateTime.Now;

                member.memberId = id;
                member.memberCode = id.ToString();
                member.createTime = now;
                member.updateTime = now;
                DataManager.memberDao.addMember(member);

                TenantMember tenantMember = new TenantMember();
                tenantMember.id = Guid.NewGuid();
                tenantMember.tenantId = tenantId;
                tenantMember.memberId = id;
                tenantMember.isBlockade = false;
                tenantMember.status = "0";
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
