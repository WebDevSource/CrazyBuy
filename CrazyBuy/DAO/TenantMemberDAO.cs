using CrazyBuy.Models;

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
    }
}
