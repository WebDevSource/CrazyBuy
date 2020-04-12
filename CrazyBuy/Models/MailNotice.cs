using System;
namespace CrazyBuy.Models
{
    public class MailNotice
    {
        public int id { get; set; }
        public Guid tenantId { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string sendTo { get; set; }
        public bool isAuto { get; set; }
        public DateTime? dtSend { get; set; }
        public bool isSend { get; set; }
        public DateTime? createTime { get; set; }        
        public string status { get; set; }
        public int creator { get; set; }
        public int? updater { get; set; }
    }
}
