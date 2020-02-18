using System;

namespace CrazyBuy.Models
{
    public class OrderDetail
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public int prdId { get; set; }
        public int qty { get; set; }
        public int prdCustPriceId { get; set; }
        public int unitPrice { get; set; }
        public int amount { get; set; }
        public string status { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
    }
}
