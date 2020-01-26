using CrazyBuy.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;

namespace CrazyBuy
{
    public class PlusOneDbContext : DbContext
    {
        public DbSet<Tenant> getTenant { get; set; }
        public PlusOneDbContext(string connStr) : base(new SqlConnection(connStr), true)
        {
            Database.SetInitializer<PlusOneDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();  //取消自動轉換 Table Name 單複數

            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}
