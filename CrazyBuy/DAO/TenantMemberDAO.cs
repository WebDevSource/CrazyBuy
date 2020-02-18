using CrazyBuy.Models;
using System;
using System.Linq;

namespace CrazyBuy.DAO
{
    public class TenantMemberDAO : CrazyBuyRerpository
    {
        public void addTenantMember(TenantMember tenantMember)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.TenantMember.Add(tenantMember);
                dbContext.SaveChanges();
            }
        }

        public TenantMember getTenantMemberByMemberId(int memberId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                TenantMember model = dbContext.TenantMember.Where(
                m => m.memberId == memberId).SingleOrDefault();
                return model;
            }
        }
    }
}
