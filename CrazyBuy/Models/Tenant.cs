using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class Tenant
    {
        [Key]        
        public int tenantId { get; set; }

        public string tenantCode { get; set; }
        
    }
}
