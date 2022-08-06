using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Text;


using log4net;
using SAPI.Models;

namespace SAPI.Common
{
    /// <summary>
    /// 业务消息处理类
    /// </summary>
    public class MessageHelper
    {
        /// <summary>
        /// 设置错误消息
        /// </summary>
        /// <param name="response">错误消息</param>
        /// <param name="errorCode">错误码</param>
        public static void SetError(ResponseMessage response, string errorCode)
        {
            response.success = false;
            response.errcode = errorCode;
            response.message = ErrorMessageManager.GetProperty(errorCode);
            response.data = "";
        }
        /// <summary>
        /// 检查系统参数，非法性校验
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public static void Check(RequestMessage request, ResponseMessage response)
        {
            //默认进去就是正确请求
            string errorCode = "EGG0002";
            string message = ErrorMessageManager.GetProperty(errorCode);

            if (!"json".Equals(request.format))
            {
                response.success = false;
                message = string.Format(message, "format");
            }
            else if (!"1.0".Equals(request.version))
            {
                response.success = false;
                message = string.Format(message, "version");
            }
            else if (!"md5".Equals(request.sign_method))
            {
                response.success = false;
                message = string.Format(message, "sign_method");
            }
            else if (string.IsNullOrWhiteSpace(request.method))
            {
                request.method = "";
                response.success = false;
                message = string.Format(message, "method");
            }
            else if (string.IsNullOrWhiteSpace(request.timestamp))
            {
                request.timestamp = "";
                response.success = false;
                message = string.Format(message, "timestamp");
            }
            else if (string.IsNullOrWhiteSpace(request.sign))
            {
                request.sign = "";
                response.success = false;
                message = string.Format(message, "sign");
            }
            if (!response.success)
            {
                response.errcode = errorCode;
                response.message = message;
                response.data = "";
            }
        }
        /// <summary>
        /// 设置系统参数不能为空校验，EGG0002
        /// </summary>
        /// <param name="response"></param>
        /// <param name="field"></param>
        public static void SetSysParaEmpty(ResponseMessage response, string field)
        {
            //默认进去就是正确请求
            string errorCode = "EGG0002";
            string message = ErrorMessageManager.GetProperty(errorCode);
            response.success = false;
            message = string.Format(message, field);
            response.errcode = errorCode;
            response.message = message;
            response.data = "";
        }

        /// <summary>
        /// 系统参数{0}不存在或不合法，EGG0026
        /// </summary>
        /// <param name="response"></param>
        /// <param name="field"></param>
        public static void SetSysConfigEmpty(ResponseMessage response, string field)
        {
            //默认进去就是正确请求
            string errorCode = "EGG0026";
            string message = ErrorMessageManager.GetProperty(errorCode);
            response.success = false;
            message = string.Format(message, field);
            response.errcode = errorCode;
            response.message = message;
            response.data = "";
        }

        /// <summary>
        /// 设置fields参数缺失错误，EGG0015
        /// </summary>
        /// <param name="response"></param>
        public static void SetParseFieldsError(ResponseMessage response)
        {
            SetError(response, "EGG0015");
        }
        /// <summary>
        /// 交易成功，AAAAAAA
        /// </summary>
        /// <param name="response">错误消息</param>
        public static void Success(ResponseMessage response)
        {
            response.success = true;
            response.errcode = "AAAAAAA";
            response.message = "";
        }
        /// <summary>
        /// 参数信息必填项未填错误，EGG0017
        /// </summary>
        /// <param name="response"></param>
        public static void ParaValueNull(ResponseMessage response)
        {
            SetError(response, "EGG0017");
        }
        /// <summary>
        /// 用户信息不存在，EGG3008
        /// </summary>
        /// <param name="response"></param>
        public static void UserNotExist(ResponseMessage response)
        {
            SetError(response, "EGG3008");
        }
        /// <summary>
        /// 用户用户信息不符，非法操作，EGG3010
        /// </summary>
        /// <param name="response"></param>
        public static void UserNotCorrect(ResponseMessage response)
        {
            SetError(response, "EGG3010");
        }

        /// <summary>
        /// user_session信息错误，请重新获取，EGG0204
        /// </summary>
        /// <param name="response"></param>
        public static void UserSessionNotCorrect(ResponseMessage response)
        {
            SetError(response, "EGG0204");
        }
        /// <summary>
        /// 参数有误暂不支持此接口，EGG0019
        /// </summary>
        /// <param name="response"></param>
        public static void SysNotSup(ResponseMessage response)
        {
            SetError(response, "EGG0019");
        }
        /// <summary>
        /// 系统更新数据处理异常，EGG0011
        /// </summary>
        /// <param name="response"></param>
        public static void SysDbUpdateError(ResponseMessage response)
        {
            SetError(response, "EGG0011");
        }
        /// <summary>
        /// 无符合条件数据，EGG3014
        /// </summary>
        /// <param name="response"></param>
        public static void NoResult(ResponseMessage response)
        {
            SetError(response, "EGG3014");
        }
        /// <summary>
        /// 数字类型参数不合法，EGG0022
        /// </summary>
        /// <param name="response"></param>
        public static void SysNotNumber(ResponseMessage response)
        {
            SetError(response, "EGG0022");
        }

        /// <summary>
        /// 金额数字类型不合法，EGG0030
        /// </summary>
        /// <param name="response"></param>
        public static void SysNotDecimal(ResponseMessage response)
        {
            SetError(response, "EGG0030");
        }


        /// <summary>
        /// 上传文件失败，请检查文件内容是否正确，EGG0027
        /// </summary>
        /// <param name="response"></param>
        public static void SysUploadFail(ResponseMessage response)
        {
            SetError(response, "EGG0027");
        }

        /// <summary>
        /// 限制最多上传{0}张，EGG0028
        /// </summary>
        /// <param name="response"></param>
        /// <param name="limitsize"></param>
        public static void SysUploadLimit(ResponseMessage response, string limitsize)
        {
            string errorCode = "EGG0028";
            string message = ErrorMessageManager.GetProperty(errorCode);
            response.success = false;
            message = string.Format(message, limitsize);
            response.errcode = errorCode;
            response.message = message;
            response.data = "";
        }

        /// <summary>
        /// 身份证号码格式验证不正确，EGG2009
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public static void IndentityNoError(ResponseMessage response, string message)
        {
            string errorCode = "EGG2009";
            string errormessage = ErrorMessageManager.GetProperty(errorCode);
            response.success = false;
            errormessage = string.Format(errormessage, message);
            response.errcode = errorCode;
            response.message = errormessage;
            response.data = "";
        }


        /// <summary>
        /// 三方支付系统订单信息异常，EGG3064
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public static void ThirdPayDataError(ResponseMessage response, string message)
        {
            string errorCode = "EGG3064";
            string errormessage = ErrorMessageManager.GetProperty(errorCode);
            response.success = false;
            errormessage = string.Format(errormessage, message);
            response.errcode = errorCode;
            response.message = errormessage;
            response.data = "";
        }


        /// <summary>
        /// 请上传文件，EGG0029
        /// </summary>
        /// <param name="response"></param>
        public static void SysUploadEmpty(ResponseMessage response)
        {
            SetError(response, "EGG0029");
        }

        /// <summary>
        /// 无效的数组参数{0}，参数格式不正确，EGG0020
        /// </summary>
        public static void SysNotArray(ResponseMessage response, string field)
        {
            string errorCode = "EGG0020";
            string message = ErrorMessageManager.GetProperty(errorCode);
            response.success = false;
            message = string.Format(message, field);
            response.errcode = errorCode;
            response.message = message;
            response.data = "";
        }
        /// <summary>
        /// 数组{0}不能为空，EGG0021
        /// </summary>
        public static void SysArrayEmpty(ResponseMessage response, string field)
        {
            string errorCode = "EGG0021";
            string message = ErrorMessageManager.GetProperty(errorCode);
            response.success = false;
            message = string.Format(message, field);
            response.errcode = errorCode;
            response.message = message;
            response.data = "";
        }
        /// <summary>
        /// 项目信息不存在，EGG3015
        /// </summary>
        /// <param name="response"></param>
        public static void ProjNotExist(ResponseMessage response)
        {
            SetError(response, "EGG3015");
        }

        /// <summary>
        /// BP 信息不存在，EGG3017
        /// </summary>
        /// <param name="response"></param>
        public static void BpNotExist(ResponseMessage response)
        {
            SetError(response, "EGG3017");
        }
        /// <summary>
        /// 抱歉，您没有查看此权限，EGG3019
        /// </summary>
        /// <param name="response"></param>
        public static void BpForbidView(ResponseMessage response)
        {
            SetError(response, "EGG3019");
        }
        /// <summary>
        /// 动态信息不存在，EGG3026
        /// </summary>
        /// <param name="response"></param>
        public static void TrendNotExist(ResponseMessage response)
        {
            SetError(response, "EGG3026");
        }
        /// <summary>
        /// 活动信息不存在，EGG3028
        /// </summary>
        /// <param name="response"></param>
        public static void ActNotExist(ResponseMessage response)
        {
            SetError(response, "EGG3028");
        }

        /// <summary>
        /// 该活动已经停止报名，EGG3029
        /// </summary>
        /// <param name="response"></param>
        public static void ActEndApply(ResponseMessage response)
        {
            SetError(response, "EGG3029");
        }
    }

    /// <summary>
    /// 错误消息属性配置文件管理器
    /// </summary>
    public static class ErrorMessageManager
    {
        private static IDictionary<string, string> _properties = null;
        private static ILog _logger = LogManager.GetLogger(typeof(ErrorMessageManager));
        private static string _configFileName = "ErrorMessage.xml";

        /// <summary>
        /// 构造函数，启动调用加载所有配置文件
        /// </summary>
        static ErrorMessageManager()
        {
            string ConfigFile = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", _configFileName));

            LoadProperties(ConfigFile);

            if (_properties != null)
            {
                _logger.Info("错误消息码一共加载：" + _properties.Count + "条;");
            }
            else
            {
                _logger.Error("重要提示：错误消息码没有加载;");
            }
        }

        /// <summary>
        /// 根据制定属性配置文件加载所有属性
        /// </summary>
        /// <param name="propertyFile">属性配置文件</param>
        private static void LoadProperties(string propertyFile)
        {
            if (string.IsNullOrWhiteSpace(propertyFile))
                throw new Exception("错误信息配置文件名为空!");

            if (File.Exists(propertyFile))
            {
                var fs = File.OpenRead(propertyFile);
                try
                {
                    _properties = XDocument.Load(fs)
                        .Descendants("property")
                        .ToDictionary(p => p.Attribute("key").Value, p => p.Attribute("value").Value);

                    _logger.Info("==================================================");
                    _logger.Info("Config File Name is [" + _configFileName + "]");
                    _logger.Info("==================================================");

                    int i = 1;
                    foreach (var p in _properties)
                    {
                        _logger.Info(string.Format("[{0}]  KEY=[{1}],  VALUE=[{2}]", i++, p.Key, p.Value.ToString()));
                    }
                    _logger.Info("==================================================");
                }
                catch (Exception e)
                {
                    _properties = new Dictionary<string, string>();
                    _logger.Error(e);
                }
                finally
                {
                    fs.Close();
                }
            }
            else
            {
                _properties = new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// 获取指定KEY的错误属性描述，没有则为空
        /// </summary>
        /// <param name="key">指定属性KEY</param>
        /// <returns>属性描述</returns>
        public static string GetProperty(string key)
        {
            string value = null;
            try
            {
                _properties.TryGetValue(key, out value);
            }
            catch (Exception)
            {
                _logger.Error("错误信息 " + key + " 未定义!");
                throw new Exception("错误信息 " + key + " 未定义!");
            }

            if (value == null) return string.Empty;

            return value;
        }

        /// <summary>
        /// 获取指定KEY的错误属性描述，没有则返回自定义错误属性描述
        /// </summary>
        /// <param name="key">指定属性KEY</param>
        /// <param name="defaultValue">自定义错误属性信息</param>
        /// <returns>属性描述</returns>
        public static string GetProperty(string key, string defaultValue = null)
        {
            string value = null;
            if (_properties.TryGetValue(key, out value))
                return value;
            return defaultValue;
        }

        /// <summary>
        /// 获取所有属性信息，慎用
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, string> GetAllProperty()
        {
            return _properties;
        }
    }


    /// <summary>
    /// 进程配置类
    /// </summary>
    public class MsgProcessorConfigInfo
    {
        public string TranNO = string.Empty;
        public string AssembleNameStr = string.Empty;
        public string TypeNameStr = string.Empty;
        public MsgProcessorConfigInfo(string _TranNO, string _AssembleName, string _TypeName)
        {
            this.TranNO = _TranNO;
            this.AssembleNameStr = _AssembleName;
            this.TypeNameStr = _TypeName;
        }
    }

    /// <summary>
    /// 接口IService
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// 服务外部调用的接口方法
        /// </summary>
        /// <param name="method">外部请求方法</param>
        /// <param name="requese">外部接口请求数据</param>
        /// <param name="response">输入响应数据</param>
        /// <returns>执行是否成功</returns>
        bool Execute(string method, RequestMessage requese, ref ResponseMessage response);
    }

    
}