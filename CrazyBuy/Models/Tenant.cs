using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class Tenant
    {
        [Key, Required]        
        public Guid tenantId { get; set; }

        public string tenantCode { get; set; }

        public string tenantGrade { get; set; }
    }
}
