using CrazyBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrazyBuy.DAO
{
	public class TenantBillDAO : CrazyBuyRerpository
	{
        public int addTenantGrade(TenantGrade tenantGrade)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.TenantGrade.Add(tenantGrade);
                dbContext.SaveChanges();
                return tenantGrade.id;
            }
        }

        public int addTenantBill(TenantBill tenantBill)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.TenantBill.Add(tenantBill);
                dbContext.SaveChanges();
                return tenantBill.id;
            }
        }

        public void addTenantBillDetail(TenantBillDetail tenantBillDetail)
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                dbContext.TenantBillDetail.Add(tenantBillDetail);
                dbContext.SaveChanges();
            }
        }
    }
}
