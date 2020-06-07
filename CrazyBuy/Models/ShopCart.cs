using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class ShopCart
    {
        [Key]
        public Guid id { get; set; }
        public int memberId { get; set; }
        public int productId { get; set; }
        public DateTime createTime { get; set; }
        public int count { get; set; }
        public int amount { get; set; }
        public Guid tenantId { get; set; }
        public string type { get; set; }
        public string prdSepc { get; set; }

        public int prdCustPriceId { get; set; }
        public string priceGradeType { get; set; }
    }

    public class ShopCartPrd
    {       
        public Guid id { get; set; }
        public int memberId { get; set; }
        public int productId { get; set; }
        public DateTime createTime { get; set; }
        public int count { get; set; }
        public int amount { get; set; }
        public Guid tenantId { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string prdImages { get; set; }
        public string summary { get; set; }
        public string SpecialRule { get; set; }
        public string prdSepc { get; set; }
        public string shipType { get; set; }
        public int stockNum { get; set; }
        public int prdCustPriceId { get; set; }
        public string priceGradeType { get; set; }
        public string paymentType { get; set; }
    }
}
