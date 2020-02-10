using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class TenantHomePrd
    {
        [Key]
        public Guid id { get; set; }
        public Guid tenantId { get; set; }
        public Guid prdId { get; set; }
        public DateTime dtStart { get; set; }
        public DateTime dtEnd { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
    }
}
