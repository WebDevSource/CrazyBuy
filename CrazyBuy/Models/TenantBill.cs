using System;
using System.Collections.Generic;


namespace CrazyBuy.Models
{
	public class TenantBill
	{
		public int id { get; set; }
		public Guid tenantId { get; set; }
		public int tenantGradeId { get; set; }
		public string name { get; set; }
		public DateTime dtDeadline { get; set; }
		public DateTime dtDue { get; set; }
		public int dueAmount { get; set; }
		public string status { get; set; }
		public DateTime createTime { get; set; }
		public DateTime? updateTime { get; set; }
		public string remark { get; set; }
		public DateTime? dtStart { get; set; }
	}

	public class TenantBillDetail
	{ 
		public int id { get; set; }
		public int billId { get; set; }
		public string name { get; set; }
		public int unitPrice { get; set; }
		public int qty { get; set; }
		public int amount { get; set; }
		public string remark { get; set; }
		public string status { get; set; }
		public DateTime createTime { get; set; }
		public DateTime? updateTime { get; set; }
	}

	public class TenantGrade
	{ 
		public int id { get; set; }
		public Guid tenantId { get; set; }
		public string tenantGrade { get; set; }
		public DateTime? dtStart { get; set; }
		public Boolean isLoop { get; set; }
		public string status { get; set; }
		public DateTime createTime { get; set; }
		public DateTime? updateTime { get; set; }
	}
}
