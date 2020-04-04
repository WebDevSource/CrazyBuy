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

        public TenantMemLevel getMemberLevel(int memberId)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" SELECT l.id,l.levelName,l.discount FROM dbo.TenantMemLevel l ";
                sql += @" left join dbo.TenantMember m on m.levelId = l.id ";
                sql += @" where m.memberId = {0} and l.status = N'正常' ";
                sql = string.Format(sql, memberId);
                return dbContext.Database.SqlQuery<TenantMemLevel>(sql).SingleOrDefault();
            }
        }

        public TenantMemLevel getMemberLevelById(int id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" SELECT l.id,l.levelName,l.discount FROM dbo.TenantMemLevel l where id = {0}";
                sql = string.Format(sql, id);
                return dbContext.Database.SqlQuery<TenantMemLevel>(sql).SingleOrDefault();
            }
        }
    }
}
