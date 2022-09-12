using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using log4net.Layout.Pattern;
using log4net.Core;

namespace SAPI.Common
{
    /// <summary>
    /// 定义扩展日志类
    /// </summary>
    public class Log4Info
    {
        public Log4Info(string msg, string sname, string lname, string uname, string did, string dname, string sid, string name)
        {
            this.Log_Message = msg;
            this.System_Name = sname;

            if (!string.IsNullOrWhiteSpace(lname))
            {
                this.Login_Name = lname;
            }
            if (!string.IsNullOrWhiteSpace(uname))
            {
                this.User_Name = uname;
            }
            if (!string.IsNullOrWhiteSpace(did))
            {
                this.Dept_ID = did;
            }
            if (!string.IsNullOrWhiteSpace(dname))
            {
                this.Dept_Name = dname;
            }
            if (!string.IsNullOrWhiteSpace(sid))
            {
                this.Staff_ID = sid;
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                this.Staff_Name = name;
            }
        }
        public string Log_Message { get; set; }
        public string System_Name { get; set; }
        public string Login_Name { get; set; }
        public string User_Name { get; set; }
        public string Dept_ID { get; set; }
        public string Dept_Name { get; set; }
        public string Staff_ID { get; set; }
        public string Staff_Name { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ActionConverter : PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, LoggingEvent loggingEvent)
        {
            var actionInfo = loggingEvent.MessageObject as Log4Info;
            if (actionInfo == null)
            {
                return;
            }
            else
            {
                switch (this.Option.ToLower())
                {
                    case "msg":
                        writer.Write(actionInfo.Log_Message);
                        break;
                    case "sname":
                        writer.Write(actionInfo.System_Name);
                        break;
                    case "lname":
                        writer.Write(actionInfo.Login_Name);
                        break;
                    case "uname":
                        writer.Write(actionInfo.User_Name);
                        break;
                    case "did":
                        writer.Write(actionInfo.Dept_ID);
                        break;
                    case "dname":
                        writer.Write(actionInfo.Dept_Name);
                        break;
                    case "sid":
                        writer.Write(actionInfo.Staff_ID);
                        break;
                    case "name":
                        writer.Write(actionInfo.Staff_Name);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ActionLayoutPattern : log4net.Layout.PatternLayout
    {
        public ActionLayoutPattern()
        {
            AddConverter(new log4net.Util.ConverterInfo
            {
                Name = "actionInfo",
                Type = typeof(ActionConverter)
            });
        }
    }
}