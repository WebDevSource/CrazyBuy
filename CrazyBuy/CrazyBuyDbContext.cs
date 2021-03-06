﻿using CrazyBuy.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;

namespace CrazyBuy
{
    public class CrazyBuyDbContext : DbContext
    {
        public DbSet<Tenant> Tenant { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<TenantMember> TenantMember { get; set; }
        public DbSet<TenantPrd> TenantPrd { get; set; }
        public DbSet<TenantPrdCat> TenantPrdCat { get; set; }
        public DbSet<ShopCart> ShopCart { get; set; }
        public DbSet<OrderMaster> OrderMaster { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<TenantFAQ> TenantFAQ { get; set; }
        public DbSet<TenantBulletin> TenantBulletin { get; set; }
        public DbSet<OrderContactItem> OrderContactItem { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Town> Town { get; set; }
        public DbSet<OrderAmountHistory> OrderAmountHistory { get; set; }
        public DbSet<MailNotice> MailNotice { get; set; }
        public DbSet<TenantGrade> TenantGrade { get; set; }
        public DbSet<TenantBill> TenantBill { get; set; }
        public DbSet<TenantBillDetail> TenantBillDetail { get; set; }


        public CrazyBuyDbContext(string connStr) : base(new SqlConnection(connStr), true)
        {
            Database.SetInitializer<CrazyBuyDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();  //取消自動轉換 Table Name 單複數            
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}
