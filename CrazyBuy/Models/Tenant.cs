using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class Tenant
    {
        [Key]
        public Guid tenantId { get; set; }
        public int createdMemberId { get; set; }
        public string tenantCode { get; set; }
        public string tenantName { get; set; }
        public string enterpriseName { get; set; }
        public string enterpriseId { get; set; }
        public string language { get; set; }
        public string owner { get; set; }
        public string FBCommunity { get; set; }
        public string FBFan { get; set; }
        public int sortIndex { get; set; }
        public bool hasCustPriceGrade { get; set; }
        public string status { get; set; }
        public int? creator { get; set; }
        public int? updater { get; set; }
        public DateTime createTime { get; set; }
        public DateTime? updateTime { get; set; }
        public string notes { get; set; }
    }

    public class TenantSetting
    {
        public string title { get; set; }
        public string content { get; set; }
    }

    public class TenantRegister
    {
        public string memberName { get; set; }
        public string lineId { get; set; }
        public string enterpriseName { get; set; }
        public string enterpriseId { get; set; }
        public string owner { get; set; }
        public string cityId { get; set; }
        public string townId { get; set; }
        public string zipCode { get; set; }
        public string address { get; set; }
        public List<ServiceItem> serviceItem { get; set; }
    }

    public class ServiceItem
    { 
        public string tenantType { get; set; }
        public string cellphone { get; set; }
        public string email { get; set; }
        public string memberPwd { get; set; }
        public string tenantCode { get; set; }
        public string tenantName { get; set; }
        public string FBCommunity { get; set; }
        public string FBFan { get; set; }
        public string LineOfficialAccount { get; set; }
        public string notes { get; set; }
    }


}
