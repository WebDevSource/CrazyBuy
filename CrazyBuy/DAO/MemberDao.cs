using System;
using System.Collections.Generic;
using System.Linq;
using CrazyBuy.Common;
using CrazyBuy.Models;

namespace CrazyBuy.DAO
{
    public class MemberDAO : CrazyBuyRerpository
    {
        public Member getMemberByCellPhone(Guid tenantId, string phone)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" select m.* from [Member] m ";
                sql += @" left join [TenantMember] t on t.memberId = m.memberId ";
                sql += @" where t.tenantId = '{0}' and m.cellphone = '{1}' and t.status = N'啟用' ";
                string query = String.Format(sql, tenantId.ToString(), phone);
                MDebugLog.debug("[getMemberByCellPhone]>" + query);
                return dbContext.Database.SqlQuery<Member>(query).SingleOrDefault();
            }
        }

        public Member getMemberByEmail(Guid tenantId, string mail)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" select m.* from [Member] m ";
                sql += @" left join [TenantMember] t on t.memberId = m.memberId ";
                sql += @" where t.tenantId = '{0}' and m.email = '{1}' and t.status = N'啟用'";
                string query = String.Format(sql, tenantId.ToString(), mail);
                return dbContext.Database.SqlQuery<Member>(query).SingleOrDefault();
            }
        }

        public Member getMemberByCellPhone(Guid tenantId, string phone, string pwd)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" select m.* from [Member] m ";
                sql += @" left join [TenantMember] t on t.memberId = m.memberId ";
                sql += @" where t.tenantId = '{0}' and m.cellphone = '{1}' and m.password = '{2}' and t.status = N'啟用' ";
                string query = String.Format(sql, tenantId.ToString(), phone, pwd);
                MDebugLog.debug("[getMemberByCellPhone]>" + query);
                return dbContext.Database.SqlQuery<Member>(query).SingleOrDefault();
            }
        }

        public Member getMemberByEmail(Guid tenantId, string mail, string pwd)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @" select m.* from [Member] m ";
                sql += @" left join [TenantMember] t on t.memberId = m.memberId ";
                sql += @" where t.tenantId = '{0}' and m.email = '{1}' and m.password = '{2}' and t.status = N'啟用' ";
                string query = String.Format(sql, tenantId.ToString(), mail, pwd);
                return dbContext.Database.SqlQuery<Member>(query).SingleOrDefault();
            }
        }

        public int addMember(Member member)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.Member.Add(member);
                dbContext.SaveChanges();
                return member.memberId;
            }
        }

        public void updateMember(Member member)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Member model = dbContext.Member.Where(
                m => m.memberId == member.memberId).SingleOrDefault();
                if (model != null)
                {
                    dbContext.Entry(model).CurrentValues.SetValues(member);
                    dbContext.SaveChanges();
                }
            }
        }

        public Member getMember(int id)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                Member model = dbContext.Member.Where(
                              m => m.memberId == id).SingleOrDefault();
                return model;
            }
        }

        public List<string> checkMemberCellPhoneIsExist(List<string> cellPhones)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                string strCellPhones = string.Join("',N'", cellPhones);
                var sql = @"SELECT DISTINCT cellphone FROM dbo.Member where cellphone IN (N'{0}')";
                sql = string.Format(sql, strCellPhones);
                return dbContext.Database.SqlQuery<string>(sql).ToList();
            }
        }

        public List<string> checkMemberEmailIsExist(List<string> emails)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                string strEmails = string.Join("',N'", emails);
                var sql = @"SELECT DISTINCT email FROM dbo.Member where email IN (N'{0}')";
                sql = string.Format(sql, strEmails);
                return dbContext.Database.SqlQuery<string>(sql).ToList();
            }
        }
    }
}
