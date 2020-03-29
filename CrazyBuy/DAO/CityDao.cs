using System.Collections.Generic;
using System.Linq;
using CrazyBuy.Models;

namespace CrazyBuy.DAO
{
    public class CityDao : CrazyBuyRerpository
    {
        public List<City> getCitys()
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"select * from [City] where status = N'正常' order by sort asc ";               
                return dbContext.Database.SqlQuery<City>(sql).ToList();
            }
        }

        public List<Town> getTowns()
        {
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"select * from [Town] where status = N'正常' order by sort asc ";
                return dbContext.Database.SqlQuery<Town>(sql).ToList();
            }
        }
    }
}
