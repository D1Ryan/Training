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
    
    public partial class App_Advert
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string AdUrl { get; set; }
        public string Position { get; set; }
        public int OrderId { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime UpdateTime { get; set; }
        public bool IsActive { get; set; }
        public string Remark { get; set; }
        public string Area { get; set; }
    }
}
