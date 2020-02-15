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

        public Member getMemberByCellPhone(string phone, string pwd)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Member model = dbContext.Member.Where(
                 m => m.cellphone == phone && m.password == pwd).SingleOrDefault();
                return model;
            }
        }

        public Member getMemberByEmail(string mail, string pwd)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Member model = dbContext.Member.Where(
                 m => m.email == mail && m.password == pwd).SingleOrDefault();
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
