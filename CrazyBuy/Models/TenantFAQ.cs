using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class TenantFAQ
    {
        [Key]
        public int id { get; set; }
        public Guid tenantId { get; set; }
        public string type { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string status { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
        public int creator { get; set; }
        public int updater { get; set; }
    }
}
