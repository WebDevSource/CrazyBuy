using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class OrderMaster
    {
        [Key]
        public int id { get; set; }
        public Guid tenantId { get; set; }
        public Guid memberId { get; set; }
        public string serialNo { get; set; }
        public DateTime dtOrder { get; set; }
        public string recipientName { get; set; }
        public string recipientGender { get; set; }
        public string recipientEmail { get; set; }
        public string recipientMobile { get; set; }
        public string recipientTel { get; set; }
        public string recipientZipCode { get; set; }
        public string recipientTownId { get; set; }
        public string recipientAddr { get; set; }
        public string hopeArrivalTime { get; set; }
        public string orderRemark { get; set; }
        public int orderAmount { get; set; }
        public int shippingAmount { get; set; }
        public bool isNeedInvoice { get; set; }
        public string invoiceType { get; set; }
        public string invoiceTitle { get; set; }
        public string invoiceBuinessNo { get; set; }
        public string payType { get; set; }
        public string payStatus { get; set; }
        public string shippingMethod { get; set; }
        public DateTime dtInStock { get; set; }
        public string shippingCompany { get; set; }
        public string shippingSerialNo { get; set; }
        public string shippingStatus { get; set; }
        public string frontRemark { get; set; }
        public string backendRemark { get; set; }
        public int sourceOrderId { get; set; }
        public string status { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
    }
}
