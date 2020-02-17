using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class TenantBulletin
    {
        [Key]
        public int id { get; set; }
        public Guid tenantId { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string Layout { get; set; }
        public bool isTop { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string status { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
        public int creator { get; set; }
        public int updater { get; set; }
    }
}
