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
    
    public partial class PubConfig
    {
        public long Config_ID { get; set; }
        public string Config_Type { get; set; }
        public string Config_Key { get; set; }
        public string Config_Name { get; set; }
        public string Config_Value { get; set; }
        public string Upper_Type { get; set; }
        public Nullable<int> Show_Index { get; set; }
        public bool Is_Protected { get; set; }
        public bool Is_Enabled { get; set; }
        public Nullable<System.DateTime> Create_Time { get; set; }
        public Nullable<System.DateTime> Update_Time { get; set; }
        public string Operator { get; set; }
        public string Reserve1 { get; set; }
    }
}
