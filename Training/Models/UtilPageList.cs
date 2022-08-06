namespace SAPI.Models
{
    using System.Collections;
    using SAPI.Common;
    using System.Web.Mvc;
    using System.Collections.Generic;

    public class OrderList
    {
        /// <summary>
        /// 返回行数据
        /// </summary>
        public int? Size { get; set; }
        /// <summary>
        /// 跳过行数
        /// </summary>
        public int? Since { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public int? ResourceId { get; set; }
        public string Mobile { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public PageModel PageModel { get; set; }
        public List<SelectListItem> ResourceList { get; set; }
        public List<App_Order> AllList { get; set; }
    }


    /// <summary>
    /// 用户列表
    /// </summary>
    public class UsersList
    {
        public string Login_Name { get; set; }
        public string User_Name { get; set; }
        public string Is_Group { get; set; }
        public string Is_Enabled { get; set; }
        public PageModel PageModel { get; set; }
     //   public List<User> AllList { get; set; }
    }

    /// <summary>
    /// 资源项列表
    /// </summary>
    public class ConfigsList
    {
        public string Config_Key { get; set; }
        public string Config_Name { get; set; }
        public string Is_Enabled { get; set; }
        public PageModel PageModel { get; set; }
        public List<PubConfig> AllList { get; set; }
    }

    /// <summary>
    /// 部门表列表
    /// </summary>
    public class DeptsList
    {
        public string Dept_ID { get; set; }
        public string Dept_Name { get; set; }
        public string Is_Enabled { get; set; }
        public string Up_Name { get; set; }
        public PageModel PageModel { get; set; }
        //  public List<PubDept> AllList { get; set; }
    }

    /// <summary>
    /// 档案列表
    /// </summary>
    public class PubFilesList
    {
        public string Dept_ID { get; set; }
        public string Is_AllDept { get; set; }
        public string Staff_ID { get; set; }
        public string Name { get; set; }
        public string ClassL2 { get; set; }
        public string File_Type { get; set; }
        public string Is_Enabled { get; set; }
        public string Create_Time { get; set; }
        public string Uploader { get; set; }
        public PageModel PageModel { get; set; }
        //  public List<PubFile> AllList { get; set; }
    }

}