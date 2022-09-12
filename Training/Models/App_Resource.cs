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
    
    public partial class App_Resource
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ResourceType { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string ImgUrl { get; set; }
        public string CImgUrl { get; set; }
        public int Status { get; set; }
        public int SaleStatus { get; set; }
        public int Price { get; set; }
        public int LinePrice { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public Nullable<int> TotalChapters { get; set; }
        public string Author { get; set; }
        public Nullable<int> SubSetCount { get; set; }
        public Nullable<int> AudioLength { get; set; }
        public string DetailDesc { get; set; }
        public string ContentUrl { get; set; }
        public string Creator { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public bool IsActive { get; set; }
        public string Remark { get; set; }
    }
}