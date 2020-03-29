using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class Town
    {
        [Key]
        public int townId { get; set; }
        public int cityId { get; set; }
        public string townName { get; set; }
        public string zipCode { get; set; }
    }
}
