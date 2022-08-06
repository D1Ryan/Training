//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class App_Consume
    {
        public long ID { get; set; }
        public string OrderId { get; set; }
        public string AccountID { get; set; }
        public string AccountNo { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ResourceId { get; set; }
        public int ResourceType { get; set; }
        public string Title { get; set; }
        public string PurchaseName { get; set; }
        public Nullable<int> PaidAmount { get; set; }
        public Nullable<int> PayStatus { get; set; }
        public string PayWay { get; set; }
        public Nullable<System.DateTime> PayTime { get; set; }
        public Nullable<System.DateTime> RefundTime { get; set; }
        public Nullable<System.DateTime> SettleTime { get; set; }
        public Nullable<System.DateTime> ExpireTime { get; set; }
        public string ClientType { get; set; }
        public string Creator { get; set; }
        public string Auditor { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public Nullable<System.DateTime> AuditTime { get; set; }
        public string Remark { get; set; }
    }
}
