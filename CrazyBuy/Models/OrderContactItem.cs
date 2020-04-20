using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class OrderContactItem
    {
        [Key]
        public int id { get; set; }
        public int orderId { get; set; }
        public DateTime dtContact { get; set; }
        public string ContactContent { get; set; }
        public string status { get; set; }
        public DateTime createTime { get; set; }
        public int creator { get; set; }

    }
}
