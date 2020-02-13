using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class ShopCart
    {
        [Key]
        public Guid id { get; set; }
        public Guid memberId { get; set; }
        public Guid productId { get; set; }
        public DateTime createTime { get; set; }
        public int count { get; set; }
        public int amount { get; set; }
        public Guid tenantId { get; set; }
    }

    public class ShopCartPrd
    {       
        public Guid id { get; set; }
        public Guid memberId { get; set; }
        public Guid productId { get; set; }
        public DateTime createTime { get; set; }
        public int count { get; set; }
        public int amount { get; set; }
        public Guid tenantId { get; set; }
        public string name { get; set; }
        public string prdImages { get; set; }
        public string summary { get; set; }
    }
}
