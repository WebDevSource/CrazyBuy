using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class TenantFAQ
    {
        [Key]
        public int id { get; set; }
        public Guid tenantId { get; set; }
        public string Type { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string status { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
        public int creator { get; set; }
        public int updater { get; set; }
    }
}
