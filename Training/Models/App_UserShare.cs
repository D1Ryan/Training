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
    
    public partial class App_UserShare
    {
        public int ID { get; set; }
        public string ShareTitle { get; set; }
        public string ShareUrl { get; set; }
        public Nullable<int> ShareType { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public string Remark { get; set; }
    }
}
