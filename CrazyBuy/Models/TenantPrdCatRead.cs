namespace CrazyBuy.Models
{
    public class TenantPrdCatRead
    {
        public int catId { get; set; }
        public string type { get; set; }
        public int? tenantMemId { get; set; }
        public int? memLevelId { get; set; }
    }
}
