using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Configuration;
using System.Web.Mvc;
using SAPI.Models;
using System.Web.Script.Serialization;

namespace SAPI.Common
{
    /// <summary>
    /// 公用方法
    /// </summary>
    public class CommonFunction
    {
        /// <summary>
        /// 获取当前程序运行坐在位置目录
        /// </summary>
        /// <returns></returns>
        public static string GetExecutingAssemblyLocation()
        {
            return System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);
            //return System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }

    /// <summary>
    /// 开发小工具包
    /// </summary>
    public class DevUtil
    {
        /// <summary>
        /// 获得后台访问referer
        /// </summary>
        public static string GetRefererCookie()
        {
            string adminReferer = WebHelper.UrlDecode(WebHelper.GetCookie("adminreferer"));
            if (adminReferer.Length == 0)
            {
                adminReferer = "/home/main";
            }
            return adminReferer;
        }

        /// <summary>
        /// 设置后台访问referer
        /// </summary>
        public static void SetRefererCookie(string url)
        {
            WebHelper.SetCookie("adminreferer", WebHelper.UrlEncode(url));
        }

        /// <summary>
        /// 获得子级referer
        /// </summary>
        public static string GetSubRefererCookie()
        {
            return WebHelper.UrlDecode(WebHelper.GetCookie("subreferer"));
        }

        /// <summary>
        /// 设置子级referer
        /// </summary>
        public static void SetSubRefererCookie(string url)
        {
            WebHelper.SetCookie("subreferer", WebHelper.UrlEncode(url));
        }

        /// <summary>
        /// 获取AppSettings
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSettingsValue(string key)
        {
            ConfigurationManager.RefreshSection("appSettings");
            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }

        /// <summary>
        /// 修改AppSettings
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool UpdateAppSettings(string key, string value)
        {
            var _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (!_config.HasFile)
            {
                throw new ArgumentException("程序配置文件缺失！");
            }
            KeyValueConfigurationElement _key = _config.AppSettings.Settings[key];
            if (_key == null)
            {
                _config.AppSettings.Settings.Add(key, value);
            }
            else
            {
                _config.AppSettings.Settings[key].Value = value;
            }
            _config.Save(ConfigurationSaveMode.Modified);
            return true;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ScriptSerialize<T>(T t)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(t);
        }

        /// <summary>
        /// 序列化列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="tlist">对象列表</param>
        /// <returns></returns>
        public static string ScriptSerializeList<T>(List<T> tlist)
        {
            string jstring = "";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            foreach (T t in tlist)
            {
                jstring += "," + serializer.Serialize(t);
            }
            if (!string.IsNullOrEmpty(jstring))
            {
                jstring = jstring.Substring(1);
            }
            return jstring;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T ScriptDeserialize<T>(string strJson)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(strJson);
        }
    }

    /// <summary>
    /// 页面调试类
    /// </summary>
    public class DevController : Controller
    {
        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns></returns>
        public string DebugGetErrMsg()
        {
            var msg = string.Empty;
            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    if (value.Errors.Count > 0)
                    {
                        foreach (var error in value.Errors)
                        {
                            msg = msg + error.ErrorMessage;
                        }
                    }
                }

            }
            return msg;
        }
    }

    /// <summary>
    /// 加密字串
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5String(string str)
        {
            if (str == null)
            {
                return null;
            }
            MD5 md5 = new MD5CryptoServiceProvider();
            Encoding e = Encoding.GetEncoding("UTF-8");
            byte[] fromData = e.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                string strHex = targetData[i].ToString("X2").ToLower();
                byte2String += strHex;
            }
            return byte2String;
        }

        /// <summary>
        /// 基于Sha1的自定义加密字符串方法：输入一个字符串，返回一个由40个字符组成的十六进制的哈希散列（字符串）。
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的十六进制的哈希散列（字符串）</returns>
        public static string Sha1(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            var data = SHA1.Create().ComputeHash(buffer);
            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// 验证码类
    /// </summary>
    public class Rand
    {
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }

        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        public static string Str(int Length)
        {
            return Str(Length, false);
        }

        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Str(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }

        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="Length">生成长度</param>
        public static string Str_char(int Length)
        {
            return Str_char(Length, false);
        }

        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Str_char(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
      }

    /// <summary>
    /// Linq泛型部分更新
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DevLinq<TEntity> where TEntity : class
    {
        ScheduleEntities dbContext = new ScheduleEntities();

        /// <summary>
        /// 更新指定字段
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="fileds">更新字段数组</param>
        public int UpdateEntityFields(TEntity entity, List<string> fileds)
        {
            int scount = 0;
            if ((entity != null) && (fileds != null))
            {
                dbContext.Set<TEntity>().Attach(entity);
                var SetEntry = ((IObjectContextAdapter)dbContext).ObjectContext.ObjectStateManager.GetObjectStateEntry(entity);
                foreach (var t in fileds)
                {
                    SetEntry.SetModifiedProperty(t);
                }
                scount = dbContext.SaveChanges();
            }
            return scount;
        }


        /// <summary>
        /// 修改实体对象指定字段
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public int UpdateModelFields(TEntity model)
        {
            if (dbContext.Entry<TEntity>(model).State == EntityState.Detached)
            {
                dbContext.Entry(model).State = EntityState.Modified;
            }
            return dbContext.SaveChanges();
        }

        /// <summary>
        /// 修改实体对象,排除部分字段
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public int UpdateModelExcept(TEntity model, List<string> listRemoveField = null)
        {
            // 排除不需更新的字段            
            foreach (string field in listRemoveField)
            {
                if (field != "Id")
                {
                    dbContext.Entry(model).Property(field).IsModified = false;
                }
            }

            if (dbContext.Entry<TEntity>(model).State == EntityState.Detached)
            {
                //将model追加到EF容器                
                dbContext.Entry(model).State = EntityState.Modified;
            }
            return dbContext.SaveChanges();
        }

        public List<TEntity> GetList(TEntity t)
        {
            return new List<TEntity>();
            // Linq中查询一个表中指定的几个字段：

            //var ts = t.FindAllItems().Where(P => P.CompanyID == CurSiteUser.CompanyId).Select(s => new
            //{
            //    BillPeriod = s.BillPeriod，FieldB = s.FieldB
            //}).Distinct().ToList().OrderByDescending(s => s.BillPeriod).Take(24);

            //return ts;
            //   FindAllItems()为查询对应表的所有数据的方法；
            // Where 里面为查询条件
            // Select 为查询的筛选条件  new{}  里面就是要查询的字段
            //Distinct() 为去除重复的查询
            //ToList() 为将查询转换为List<>
            //OrderByDescending()  表示排序字段及排序方法（倒序排列）
            //Take(N)  表示查询前N条数据；
        }
    }
}

/// <summary>
/// 参数化语句
/// </summary>
public class SqlParameterSelf
{
    /// <summary>
    /// 用参数名称和新 System.Data.SqlClient.SqlParameter 的一个值初始化 System.Data.SqlClient.SqlParameter类的新实例
    /// </summary>
    /// <param name="parameterName">要映射的参数的名称</param>
    /// <param name="dbType">System.Data.SqlDbType 值之一</param>
    /// <param name="value">一个 System.Object，它是 System.Data.SqlClient.SqlParameter 的值</param>
    /// <returns></returns>
    public static SqlParameter CreateSqlParameter(string parameterName, SqlDbType dbType, object value)
    {
        SqlParameter sqlParameter = new SqlParameter(parameterName, dbType);
        sqlParameter.Value = value;
        return sqlParameter;
    }

    /// <summary>
    /// 用参数名称、System.Data.SqlDbType 和大小初始化 System.Data.SqlClient.SqlParameter 类的新实例
    /// </summary>
    /// <param name="parameterName">要映射的参数的名称</param>
    /// <param name="dbType">System.Data.SqlDbType 值之一。</param>
    /// <param name="size">参数的长度。</param>
    /// <param name="value">一个 System.Object，它是 System.Data.SqlClient.SqlParameter 的值</param>
    /// <returns></returns>
    public static SqlParameter CreateSqlParameter(string parameterName, SqlDbType dbType, int size, object value)
    {
        SqlParameter sqlParameter = new SqlParameter(parameterName, dbType, size);
        sqlParameter.Value = value;
        return sqlParameter;
    }
}

/// <summary>
/// 数据库字段长度验证
/// </summary>
public static class SqlFieldHelper
{
    /// <summary>
    /// 验证字段字节长度
    /// </summary>
    /// <param name="field">字段长度</param>
    /// <param name="byteLenght">数据库字段最大长度</param>
    /// <returns>true - 验证通过，false - 验证失败</returns>
    private static bool validate(String field, Int32 byteLenght)
    {
        int count = System.Text.Encoding.UTF8.GetByteCount(field);
        return count <= byteLenght;
    }

    /// <summary>
    /// 验证用户邮箱账号，验证数据库最大长度为40
    /// </summary>
    /// <param name="email">邮箱账号</param>
    /// <returns></returns>
    public static bool Email(String email)
    {
        return validate(email, 40);
    }

    /// <summary>
    /// 验证用户昵称，验证数据库最大长度为40
    /// </summary>
    /// <param name="nick_name">用户昵称</param>
    /// <returns></returns>
    public static bool Nick_Name(String nick_name)
    {
        return validate(nick_name, 40);
    }

    /// <summary>
    /// 处理插入字符串类型数据库字段
    /// </summary>
    /// <param name="field">插入数据库的字段</param>
    /// <returns>返回经过处理的字符串</returns>
    public static string SetString(string field)
    {
        return string.IsNullOrWhiteSpace(field) ? string.Empty : field.Trim();
    }

    /// <summary>
    /// 处理插入长整理类型数据库字段
    /// （处理后最小值为0）
    /// </summary>
    /// <param name="field">插入数据库的字段</param>
    /// <returns>返回经过处理的字符串</returns>
    public static long SetLong(long field)
    {
        return field < 0 ? 0 : field;
    }

    /// <summary>
    /// 处理插入十进制类型数九字段
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static decimal SetDecimal(decimal field)
    {
        return field < 0 ? decimal.Zero : field;
    }

    /// <summary>
    /// 处理Int32类型的字段
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static long SetInt32(int field)
    {
        return field < 0 ? 0 : field;
    }

    /// <summary>
    /// 处理BIT类型的字段
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public static bool SetBit(bool field)
    {
        return true == field;
    }
}

/// <summary>
/// 
/// </summary>
public static class ExtensionDataTable
{
    public static List<T> List<T>(this DataTable dt)
    {
        var list = new List<T>();
        Type t = typeof(T);
        var plist = new List<PropertyInfo>(typeof(T).GetProperties());

        foreach (DataRow item in dt.Rows)
        {
            T s = System.Activator.CreateInstance<T>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                if (info != null)
                {
                    if (!Convert.IsDBNull(item[i]))
                    {
                        info.SetValue(s, item[i], null);
                    }
                }
            }
            list.Add(s);
        }
        return list;
    }
}

/// <summary>
/// 扩展对象，数据类型转换
/// </summary>
public static class ExtensionObject
{
    public static string ErrorToString(this Exception err)
    {
        StringBuilder sb = new StringBuilder();
        try
        {
            if (err != null)
            {
                if (err.Message != null)
                    sb.Append("Message:" + err.Message + Environment.NewLine);
                if (err.Source != null)
                    sb.Append("Source:" + err.Source + Environment.NewLine);
                if (err.HelpLink != null)
                    sb.Append("HelpLink:" + err.HelpLink + Environment.NewLine);
                if (err.StackTrace != null)
                    sb.Append("StackTrace:" + err.StackTrace + Environment.NewLine);
                if (err.InnerException != null)
                {
                    if (err.InnerException.Message != null)
                        sb.Append("InnerException Message:" + err.InnerException.Message + Environment.NewLine);
                    if (err.InnerException.Source != null)
                        sb.Append("InnerException Source:" + err.InnerException.Source + Environment.NewLine);
                    if (err.InnerException.HelpLink != null)
                        sb.Append("InnerException HelpLink:" + err.InnerException.HelpLink + Environment.NewLine);
                    if (err.InnerException.StackTrace != null)
                        sb.Append("InnerException StackTrace:" + err.InnerException.StackTrace + Environment.NewLine);
                    if (err.InnerException.TargetSite != null)
                        sb.Append("InnerException TargetSite:" + err.InnerException.TargetSite + Environment.NewLine);
                }
            }
        }
        catch (Exception)
        {
        }
        return sb.ToString();
    }
    public static int ConvertInt(this object obj)
    {
        int ReturnValue = int.MinValue;
        if (obj != null)
        {
            if (int.TryParse(obj.ToString(), out ReturnValue))
            {
                return ReturnValue;
            }
        }
        return ReturnValue;
    }
    public static bool ConvertBool(this object obj)
    {
        if (obj != null)
        {
            bool ReturnValue = false;
            if (bool.TryParse(obj.ToString(), out ReturnValue))
            {
                return ReturnValue;
            }
        }
        return false;
    }
    public static decimal CovertDecimal(this object obj)
    {
        if (obj == null)
        {
            return 0.0M;
        }
        else
        {
            decimal LinPoint = 0.0M;
            decimal.TryParse(obj.ToString(), out LinPoint);
            return LinPoint;
        }
    }
    public static string CovertString(this object obj)
    {
        if (obj == null)
        {
            return string.Empty;
        }
        else
        {
            return obj.ToString().Trim();
        }
    }
    public static DateTime CovertDateTime(this object obj)
    {
        string LinStr = obj.CovertString();
        if (LinStr.Length <= 0)
        {
            return DateTime.MinValue;
        }
        DateTime LinDateTime = DateTime.MinValue;
        DateTime.TryParse(LinStr, out LinDateTime);
        return LinDateTime;
    }
}
