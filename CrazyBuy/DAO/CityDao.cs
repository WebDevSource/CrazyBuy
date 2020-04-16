using System;
using System.Collections.Generic;
using System.Linq;
using CrazyBuy.Models;

namespace CrazyBuy.DAO
{
    public class CityDao : CrazyBuyRerpository
    {
        public Dictionary<int, City> cityDic = new Dictionary<int, City>();
        public Dictionary<string, Town> townDic = new Dictionary<string, Town>();
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

        public City getCity(int id)
        {
            City result = null;

            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"select * from [City] where status = N'正常' and cityId = {0} ";
                var query = String.Format(sql, id);
                result = dbContext.Database.SqlQuery<City>(query).FirstOrDefault();
            }

            return result;
        }

        public Town getTown(string id)
        {
            Town result = null;
            using (CrazyBuyDbContext dbContext = ContextInit())
            {
                var sql = @"select * from [Town] where status = N'正常' and townId = {0} ";
                var query = String.Format(sql, (id == null ? "0":id));
                result = dbContext.Database.SqlQuery<Town>(query).FirstOrDefault();
            }

            return result;
        }
    }
}
