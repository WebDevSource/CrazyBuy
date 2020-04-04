using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class TenantMemLevel
    {
        [Key]
        public int id { get; set; }
        public string levelName { get; set; }
        public int discount { get; set; }
    }
}
