using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class TenantMember
    {
        [Key]
        public int id { get; set; }
        public Guid tenantId { get; set; }
        public int memberId { get; set; }
        public int? levelId { get; set; }
        public string gradeType { get; set; }
        public int? custPriceGradeId { get; set; }
        public string Notes { get; set; }
        public DateTime? dtEnable { get; set; }
        public DateTime? dtExpiry { get; set; }
        public bool isBlockade { get; set; }
        public DateTime? dtLastOrder { get; set; }
        public string status { get; set; }
        public DateTime? createTime { get; set; }
        public DateTime? updateTime { get; set; }
        public int? creator { get; set; }
        public int? updater { get; set; }
    }
}
