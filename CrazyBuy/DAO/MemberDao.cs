using System.Collections.Generic;
using System.Linq;
using CrazyBuy.Models;

namespace CrazyBuy.DAO
{
    public class MemberDao : CrazyBuyRerpository
    {
        public List<Member> GetTable(int limit)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                IQueryable<Member> result = dbContext.Member;
                result = result.Take(limit);
                return result.ToList();
            }
        }
    }
}
