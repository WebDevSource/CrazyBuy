using System.Linq;
using CrazyBuy.Models;

namespace CrazyBuy.DAO
{
    public class MemberDAO : CrazyBuyRerpository
    {
        public Member getMember(string account, string pwd)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Member model = dbContext.Member.Where(
                 m => m.account == account && m.password == pwd).SingleOrDefault();
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

        public void updateMember(Member member)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Member model = dbContext.Member.Where(
                m => m.memberCode == member.memberCode).SingleOrDefault();
                if (model != null)
                {
                    dbContext.Entry(model).CurrentValues.SetValues(member);
                    dbContext.SaveChanges();
                }
            }
        }
    }
}
