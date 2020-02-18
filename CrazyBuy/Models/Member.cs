using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class Member
    {
        [Key]
        public int memberId { get; set; }
        public string memberCode { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string cellphone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string gender { get; set; }
        public DateTime? birthday { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string lineId { get; set; }
        public string lineLoginId { get; set; }
        public string fbId { get; set; }
        public string messengerId { get; set; }
        public string tenantType { get; set; }
        public string Notes { get; set; }
        public DateTime? dtLastLogin { get; set; }
        public string status { get; set; }
        public DateTime? createTime { get; set; }
        public DateTime? updateTime { get; set; }
        public int? creator { get; set; }
        public int? updater { get; set; }
        public DateTime? tenantDue { get; set; }
    }
}
