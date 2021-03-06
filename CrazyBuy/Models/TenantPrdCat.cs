﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CrazyBuy.Models
{
    public class TenantPrdCat
    {
        [Key]
        public int id { get; set; }

        public Guid tenantId { get; set; }
        public int? parentId { get; set; }
        public string name { get; set; }
        public int sort { get; set; }
        public string status { get; set; }
        public DateTime? createTime { get; set; }
        public DateTime? updateTime { get; set; }
        public int? creator { get; set; }
        public int? updater { get; set; }
    }

    public class TenantPrdCatCount
    {
        [Key]
        public int id { get; set; }

        public Guid tenantId { get; set; }
        public int? parentId { get; set; }
        public string? name { get; set; }
        public int? sort { get; set; }
        public string? status { get; set; }
        public DateTime? createTime { get; set; }
        public DateTime? updateTime { get; set; }
        public int? creator { get; set; }
        public int? updater { get; set; }
        public int? count { get; set; }
        public int? pcount { get; set; }
        public int? ccount { get; set; }
    }
}
