using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class Tenant
    {
        [Key]        
        public Guid tenantId { get; set; }

        public string tenantCode { get; set; }
        
    }
}
