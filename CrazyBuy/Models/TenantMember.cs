using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class TenantMember
    {
        [Key]
        public int id { get; set; }
        public int tenantId { get; set; }
        public int memberId { get; set; }
        public int levelId { get; set; }
        public string gradeType { get; set; }
        
    }
}
