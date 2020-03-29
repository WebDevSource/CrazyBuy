using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class City
    {
        [Key]        
        public int cityId { get; set; }
        public string cityName { get; set; }
    }

    public class Area
    {
        [Key]
        public string name { get; set; }
        public List<Town> areas { get; set; }
        public int id { get; set; }
    }
}
