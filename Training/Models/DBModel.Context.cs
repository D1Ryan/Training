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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ScheduleEntities : DbContext
    {
        public ScheduleEntities()
            : base("name=ScheduleEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<App_Account> App_Account { get; set; }
        public virtual DbSet<App_Advert> App_Advert { get; set; }
        public virtual DbSet<App_AliPay> App_AliPay { get; set; }
        public virtual DbSet<App_ApiAccount> App_ApiAccount { get; set; }
        public virtual DbSet<App_Consume> App_Consume { get; set; }
        public virtual DbSet<App_Order> App_Order { get; set; }
        public virtual DbSet<App_OrderAllot> App_OrderAllot { get; set; }
        public virtual DbSet<App_OrderStudent> App_OrderStudent { get; set; }
        public virtual DbSet<App_Register> App_Register { get; set; }
        public virtual DbSet<App_Resource> App_Resource { get; set; }
        public virtual DbSet<App_Sales> App_Sales { get; set; }
        public virtual DbSet<App_SalesAllot> App_SalesAllot { get; set; }
        public virtual DbSet<App_SMS> App_SMS { get; set; }
        public virtual DbSet<App_StudyHistory> App_StudyHistory { get; set; }
        public virtual DbSet<App_UserCalender> App_UserCalender { get; set; }
        public virtual DbSet<App_UserInfo> App_UserInfo { get; set; }
        public virtual DbSet<App_UserLog> App_UserLog { get; set; }
        public virtual DbSet<App_UserNotice> App_UserNotice { get; set; }
        public virtual DbSet<App_UserRecord> App_UserRecord { get; set; }
        public virtual DbSet<App_UserShare> App_UserShare { get; set; }
        public virtual DbSet<App_Version> App_Version { get; set; }
        public virtual DbSet<App_Video> App_Video { get; set; }
        public virtual DbSet<App_WechatPay> App_WechatPay { get; set; }
        public virtual DbSet<PubConfig> PubConfigs { get; set; }
        public virtual DbSet<App_OrderSS> App_OrderSS { get; set; }
        public virtual DbSet<App_LiveBook> App_LiveBook { get; set; }
        public virtual DbSet<App_LiveVideo> App_LiveVideo { get; set; }
    }
}
