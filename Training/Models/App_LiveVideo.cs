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
    
    public partial class App_LiveVideo
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string FrontPage { get; set; }
        public string CourseDetail { get; set; }
        public Nullable<System.DateTime> SaleTime { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public string LiveUrl { get; set; }
        public string RePlayUrl { get; set; }
        public bool IsTop { get; set; }
        public bool IsEnabled { get; set; }
        public Nullable<int> Booked { get; set; }
        public Nullable<int> Watched { get; set; }
        public Nullable<int> LiveStatus { get; set; }
        public string Creator { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public string Updator { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public string Reserve1 { get; set; }
    }
}
