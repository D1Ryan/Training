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
    
    public partial class App_Version
    {
        public int ID { get; set; }
        public string AppID { get; set; }
        public string AppName { get; set; }
        public string VersionCode { get; set; }
        public string VersionName { get; set; }
        public string Publisher { get; set; }
        public Nullable<System.DateTime> PublishTime { get; set; }
        public string UpdateContent { get; set; }
        public Nullable<int> IsDelete { get; set; }
        public Nullable<int> IsCurrent { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
    }
}