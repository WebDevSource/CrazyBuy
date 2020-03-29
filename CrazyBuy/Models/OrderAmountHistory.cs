using System;
namespace CrazyBuy.Models
{
    public class OrderAmountHistory
    {
        public int id { get; set; }
        public int orderId { get; set; }
        public string changeDesc { get; set; }
        public int changeAmount { get; set; }
        public int cumulativeAmount { get; set; }
        public string status { get; set; }
        public DateTime? createTime { get; set; }
        public DateTime? updateTime { get; set; }
        public int creator { get; set; }
        public int? updater { get; set; }
    }
}
