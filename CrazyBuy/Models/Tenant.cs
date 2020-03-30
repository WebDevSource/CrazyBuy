using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class Tenant
    {
        [Key]
        public Guid tenantId { get; set; }

        public string tenantCode { get; set; }

        public string owner { get; set; }

        public int createdMemberId { get; set; }

        public DateTime updateTime { get; set; }

        public DateTime createTime { get; set; }
        
        public string status { get; set; }
    }

    public class TenantSetting
    {
        public string title { get; set; }
        public string content { get; set; }
    }
}
