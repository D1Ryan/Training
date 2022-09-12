using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;

using SAPI.Common;
using SAPI.Models;
using log4net;

namespace SAPI.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 当前数据库
        /// </summary>
        public ScheduleEntities db = new ScheduleEntities();
        /// <summary>
        /// 当前用户信息
        /// </summary>
        //public UserLogin LoginUser { get; set; }
        /// <summary>
        /// 超级操作员的用户编号
        /// </summary>
        public long SuperId { get; set; }
        /// <summary>
        /// 超级操作员的用户编码
        /// </summary>
        public const string SuperCode = "001";
        /// <summary>
        /// 数据未指定主键
        /// </summary>
        public const string DataWasNoDefined = "数据未指定主键";
        /// <summary>
        /// 记录不存在或已失效
        /// </summary>
        public const string DataWasDisabledOrDoesNotExist = "记录不存在或已失效";

        /// <summary>
        /// 列表暂无数据项
        /// </summary>
        public const string DataListDoesNotExistAnyItems = "列表暂无数据项";

        /// <summary>
        /// Sales图片地址
        /// </summary>
        public const string SalesImgUrl = "http://sapp.anyue.net/images/sales/";

        ///上传图片大小
        public int UpImageLength { get; set; }

        protected ILog iLogger = LogManager.GetLogger("BaseController");

        //private string deptCacheKey { get { return "deptCache" + this.LoginUser.User_ID.ToString(); } }

        /////// <summary>
        /////// 获取当前用户缓存的部门列表
        /////// </summary>
        /////// <param name="aTypes"></param>
        /////// <returns></returns>
        ////protected List<DeptOptions> GetCacheDepts(string uid)
        ////{
        ////    List<DeptOptions> deptList;
        ////    //缓存调整类型项     
        ////    bool isDeptCached = CacheHelper.Exists(deptCacheKey);
        ////    if (isDeptCached == false)
        ////    {
        ////        deptList = DAL.PubDeptDal.GetDeptOptions(uid);
        ////        CacheHelper.Set(deptCacheKey, deptList);
        ////    }
        ////    else
        ////    {
        ////        deptList = (List<DeptOptions>)CacheHelper.Get(deptCacheKey);
        ////    }
        ////    return deptList;
        ////}

        ///// <summary>
        ///// 执行控制器方法之前先执行该方法
        ///// 获取自定义SessionId的值，然后从其中取出用户信息，反系列化
        ///// </summary>
        ///// <param name="filterContext"></param>
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    string lsessionId = "";
        //    string lucontent = "";
        //    try
        //    {
        //        lsessionId = Session["lsessionId"].ToString();
        //        lucontent = Session[lsessionId].ToString();
        //        LoginUser = DevUtil.ScriptDeserialize<UserLogin>(lucontent);
        //        SuperId = Convert.ToInt64(DevUtil.GetAppSettingsValue("superuserid"));
        //        UpImageLength = Convert.ToInt32(DevUtil.GetAppSettingsValue("upimagelength"));
        //    }
        //    catch
        //    {
        //        Session.RemoveAll();
        //        Log4Info info = new Log4Info("登录状态验证失败.", "System", "", "", "", "", "", "");
        //        iLogger.Debug(info);
        //        filterContext.HttpContext.Response.Redirect("/Login/Index");
        //    }
        //    base.OnActionExecuting(filterContext);
        //}

        ///// <summary>
        ///// 从上传文件中获取DataTable
        ///// </summary>
        ///// <param name="fileUpload">上传控件</param>
        ///// <param name="errBack">错误信息</param>
        ///// <returns></returns>
        //protected DataTable GetDataTable(HttpPostedFileBase fileUpload, out string errBack)
        //{
        //    DataTable impDatas = null;
        //    const string fileErr = "表格列或数据不正确，请核对后重新导入！";
        //    if (fileUpload == null)
        //    {
        //        errBack = ScriptHelper.ShowAlert("文件为空，请重新选择文件!");
        //        return null;
        //    }
        //    try
        //    {
        //        string fileName;
        //        //将本地路径转化为服务器路径的文件流
        //        string filePath2 = Path.GetFileName(fileUpload.FileName);
        //        fileName = Path.Combine(Request.MapPath("~/UpFiles"), filePath2);
        //        fileUpload.SaveAs(fileName);
        //        //NPOI得到EXCEL的数据   
        // //       impDatas = NPOIHelper.ExcelToDataTable(fileName);
        //        ///导入完成，删掉上传文件
        //        System.IO.File.Delete(fileName);
        //        //判断读取的有效性
        //        int rowcount = impDatas.Rows.Count;
        //        //核对表格数据有效性
        //        string first = impDatas.Rows[0][0].CovertString();
        //        if (string.IsNullOrWhiteSpace(first))
        //        {
        //            errBack = ScriptHelper.ShowAlert(fileErr);
        //            return null;
        //        }
        //    }
        //    catch
        //    {
        //        errBack = ScriptHelper.ShowAlert(fileErr);
        //        return null;
        //    }
        //    errBack = string.Empty;
        //    return impDatas;
        //}

        /// <summary>
        /// 释放系统资源
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}