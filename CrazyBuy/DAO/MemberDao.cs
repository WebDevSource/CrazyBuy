using System;
using System.Linq;
using CrazyBuy.Models;

namespace CrazyBuy.DAO
{
    public class MemberDAO : CrazyBuyRerpository
    {
        public Member getMember(string userId, string pwd, Guid tenantId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Member model = dbContext.Member.Where(
                 m => m.userId == userId && m.userPassword == pwd
                 && m.tenantId == tenantId).SingleOrDefault();
                return model;
            }
        }

        public void addMember(Member member)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.Member.Add(member);
                dbContext.SaveChanges();
            }
        }
    }
}
