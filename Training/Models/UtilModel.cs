using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace SAPI.Models
{

    /// <summary>
    /// 销售类型
    /// </summary>
    public enum SalesType
    {
        None, Social, Phone, Search
    }

    /// <summary>
    /// 登录用户信息(cache)
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long User_ID { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Login_Name { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string User_Name { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        public string Inner_Code { get; set; }
    }

    /// <summary>
    /// 用户编号姓名
    /// </summary>
    public class UserBrief
    {
        public int User_Id { get; set; }
        public string User_Name { get; set; }
    }

    /// <summary>
    /// 资源项列表
    /// </summary>
    public class ConfigOptions
    {
        public string Config_Name { get; set; }
        public string Config_Value { get; set; }
    }

    /// <summary>
    /// 部门下拉项
    /// </summary>
    public class DeptOptions
    {
        public string Inner_Code { get; set; }
        public string Dept_ID { get; set; }
        public string Dept_Name { get; set; }
    }

    /// <summary>
    /// 消息请求实体类
    /// </summary>
    public class RequestMessage
    {
        /// <summary>
        /// 分配给应用的AppKey ，创建应用时获得
        /// </summary>
        public string app_key { get; set; }
        /// <summary>
        /// 分配给应用的会话，系统控制超时限制
        /// </summary>
        public string app_session { get; set; }
        /// <summary>
        /// 约定应用的app_version ，创建应用时获得
        /// </summary>
        public string app_version { get; set; }
        /// <summary>
        /// 动态接口请求参数
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        [DefaultValue("")]
        public dynamic fields { get; set; }
        /// <summary>
        /// 可选，指定响应格式。目前支持格式为json以后可能支持xml
        /// </summary>
        public string format { get; set; }
        /// <summary>
        /// API接口名称 如 shop.get 
        /// </summary>
        public string method { get; set; }
        /// <summary>
        /// 签名加密
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 参数的加密方法选择，可选值是：md5
        /// </summary>
        public string sign_method { get; set; }
        /// <summary>
        /// 时间戳，格式为yyyy-mm-dd HH:mm:ss
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// API接口名称 API协议版本，可选值为:1.0 
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 应用描述信息
        /// </summary>
        public string app_desc { get; set; }
    }

    /// <summary>
    /// 消息响应实体类
    /// </summary>
    public class ResponseMessage
    {
        /// <summary>
        /// 接口成功或者失败标志
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string errcode { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 消息体内容
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// 是否删除清空日志的请求大文件信息
        /// </summary>
        [JsonIgnore]
        public bool Is_Delete_Req { get; set; }
    }

    /// <summary>
    /// ToolTipModel
    /// </summary>
    public class ToolTipModel
    {
        private string _backurl = "";//返回地址
        private string _message = "";//提示信息
        private int _countdownmodel = 0;//倒计时模型
        private int _countdowntime = 1;//倒计时时间
        private bool _isshowbacklink = true;//是否显示返回地址
        private bool _isautoback = true;//是否自动返回

        public ToolTipModel(string message)
        {
            _message = message;
            _isshowbacklink = false;
            _isautoback = false;
        }

        public ToolTipModel(string backUrl, string message)
        {
            _backurl = backUrl;
            _message = message;
        }

        public ToolTipModel(string backUrl, string message, bool isAutoBack)
        {
            _backurl = backUrl;
            _message = message;
            _isautoback = isAutoBack;
        }

        /// <summary>
        /// 返回地址
        /// </summary>
        public string BackUrl
        {
            get { return _backurl; }
            set { _backurl = value; }
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        /// <summary>
        /// 倒计时模型
        /// </summary>
        public int CountdownModel
        {
            get { return _countdownmodel; }
            set { _countdownmodel = value; }
        }

        /// <summary>
        /// 倒计时时间
        /// </summary>
        public int CountdownTime
        {
            get { return _countdowntime; }
            set { _countdowntime = value; }
        }

        /// <summary>
        /// 是否显示返回地址
        /// </summary>
        public bool IsShowBackLink
        {
            get { return _isshowbacklink; }
            set { _isshowbacklink = value; }
        }

        /// <summary>
        /// 是否自动返回
        /// </summary>
        public bool IsAutoBack
        {
            get { return _isautoback; }
            set { _isautoback = value; }
        }
    }
}