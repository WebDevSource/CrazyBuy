using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class TenantPrd
    {
        [Key]
        public int id { get; set; }
        public Guid tenantId { get; set; }
        public int? sourceId { get; set; }
        public string prdCode { get; set; }
        public string name { get; set; }
        public string summary { get; set; }
        public string desc { get; set; }
        public int? costPrice { get; set; }
        public int? fixedprice { get; set; }
        public int? memberPrice { get; set; }
        public int? transferPrice { get; set; }
        public DateTime? dtSellStart { get; set; }
        public DateTime? dtSellEnd { get; set; }
        public string prdImages { get; set; }
        public string prdSepc { get; set; }
        public int? stockNum { get; set; }
        public bool zeroStockStopSell { get; set; }
        public string zeroStockMessage { get; set; }
        public string takeOffMethod { get; set; }
        public string takeOffMessage { get; set; }
        public string paymentType { get; set; }
        public string shipType { get; set; }
        public string SpecialRule { get; set; }
        public string SpecialRuleOthersText { get; set; }
        public bool isOpenOrder { get; set; }
        public string status { get; set; }
        public DateTime? createTime { get; set; }
        public DateTime? updateTime { get; set; }
        public int? creator { get; set; }
        public int? updater { get; set; }
        public string remark { get; set; }
 
    }
}
