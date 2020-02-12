using CrazyBuy.Models;
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
