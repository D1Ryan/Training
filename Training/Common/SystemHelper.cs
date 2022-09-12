using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Web;
using System.Reflection;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Configuration;

using log4net;
using SAPI.Models;

namespace SAPI.Common
{
    /// <summary>
    /// 缓存内容枚举
    /// </summary>
    public enum MemoryKeyEnum
    {
        /// <summary>
        /// 系统参数配置
        /// </summary>
        SysConfig = 0
    }

    /// <summary>
    /// 缓存辅助类
    /// </summary>
    public class MemoryKeyHelper
    {
        private static Dictionary<MemoryKeyEnum, string> dictionary = new Dictionary<MemoryKeyEnum, string>();

        private static Dictionary<MemoryKeyEnum, string> GetInstance()
        {
            if (dictionary == null || dictionary.Count < 1)
            {
                dictionary.Add(MemoryKeyEnum.SysConfig, "SysConfig");
            }
            return dictionary;
        }

        public MemoryKeyHelper()
        {
            GetInstance();
        }

        public static string GetValue(MemoryKeyEnum keyEnum)
        {
            GetInstance();
            return dictionary[keyEnum];
        }
    }

    /// <summary>
    /// 验证图片类
    /// </summary>
    public class SystemHelper
    {
        #region 私有字段
        private string text;
        private Bitmap image;
        private int letterWidth = 16;  //单个字体的宽度范围
        private int letterHeight = 20; //单个字体的高度范围
        private static byte[] randb = new byte[4];
        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
        private Font[] fonts =
    {
       new Font(new FontFamily("Times New Roman"),10 +Next(1),System.Drawing.FontStyle.Regular),
       new Font(new FontFamily("Georgia"), 10 + Next(1),System.Drawing.FontStyle.Regular),
       new Font(new FontFamily("Arial"), 10 + Next(1),System.Drawing.FontStyle.Regular),
       new Font(new FontFamily("Comic Sans MS"), 10 + Next(1),System.Drawing.FontStyle.Regular)
    };
        #endregion

        #region 公有属性
        /// <summary>
        /// 验证码
        /// </summary>
        public string Text
        {
            get { return this.text; }
        }

        /// <summary>
        /// 验证码图片
        /// </summary>
        public Bitmap Image
        {
            get { return this.image; }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemHelper()
        {
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.AddHeader("pragma", "no-cache");
            HttpContext.Current.Response.CacheControl = "no-cache";
            this.text = Rand.Number(4);
            CreateImage();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        private static int Next(int max)
        {
            rand.GetBytes(randb);
            int value = BitConverter.ToInt32(randb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        private static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 绘制验证码
        /// </summary>
        public void CreateImage()
        {
            int int_ImageWidth = this.text.Length * letterWidth;
            Bitmap image = new Bitmap(int_ImageWidth, letterHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            for (int i = 0; i < 2; i++)
            {
                int x1 = Next(image.Width - 1);
                int x2 = Next(image.Width - 1);
                int y1 = Next(image.Height - 1);
                int y2 = Next(image.Height - 1);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            int _x = -12, _y = 0;
            for (int int_index = 0; int_index < this.text.Length; int_index++)
            {
                _x += Next(12, 16);
                _y = Next(-2, 2);
                string str_char = this.text.Substring(int_index, 1);
                str_char = Next(1) == 1 ? str_char.ToLower() : str_char.ToUpper();
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePos = new Point(_x, _y);
                g.DrawString(str_char, fonts[Next(fonts.Length - 1)], newBrush, thePos);
            }
            for (int i = 0; i < 10; i++)
            {
                int x = Next(image.Width - 1);
                int y = Next(image.Height - 1);
                image.SetPixel(x, y, Color.FromArgb(Next(0, 255), Next(0, 255), Next(0, 255)));
            }
            image = TwistImage(image, true, Next(1, 3), Next(4, 6));
            g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, int_ImageWidth - 1, (letterHeight - 1));
            this.image = image;
        }

        /// <summary>
        /// 字体随机颜色
        /// </summary>
        public Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
            int int_Red = RandomNum_First.Next(180);
            int int_Green = RandomNum_Sencond.Next(180);
            int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高,一般为3</param>
        /// <param name="dPhase">波形的起始相位,取值区间[0-2*PI)</param>
        public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            double PI = 6.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI * (double)j) / dBaseAxisLen : (PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }
        #endregion
    }

    /// <summary>
    /// 服务器配置文件读取器
    /// </summary>
    public class ServerHelper
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ServerHelper));
        /// <summary>
        /// 根据key获取对应的配置内容
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static string GetContentByKey(XmlDocument xmlDoc, string Key)
        {
            return xmlDoc.LastChild.SelectSingleNode(Key).Attributes["value"].Value;
        }

        ///<summary>
        ///获取配置的交易处理器信息
        ///</summary>
        ///<param name="xmlDoc"></param>
        ///<param name="DefaultMsgProcess"></param>
        ///<returns></returns>
        public static Dictionary<string, MsgProcessorConfigInfo> GetMsgProcessorInfo(XmlDocument xmlDoc, ref MsgProcessorConfigInfo DefaultMsgProcess)
        {
            XmlNodeList nodeList = xmlDoc.LastChild.SelectNodes("MsgProcessorList/MsgProcessor");
            DefaultMsgProcess = null;
            Dictionary<string, MsgProcessorConfigInfo> MsgProcessorConfigInfo_List = new Dictionary<string, MsgProcessorConfigInfo>();

            string TranNO = string.Empty;
            string AssemblyName = string.Empty;
            string TypeName = string.Empty;
            bool IsDefault = false;
            foreach (XmlNode Item in nodeList)
            {
                TranNO = Item.Attributes["TranNO"].Value;
                AssemblyName = Item.Attributes["AssemblyName"].Value;
                TypeName = Item.Attributes["TypeName"].Value;
                IsDefault = Item.Attributes["IsDefault"].Value.ConvertBool();
                MsgProcessorConfigInfo _MsgProcessorConfigInfo = new MsgProcessorConfigInfo(TranNO,
                    AssemblyName, TypeName);
                if (IsDefault)
                {
                    if (DefaultMsgProcess != null)
                    {
                        _logger.Error("重要提示：交易处理器，系统配置了多个默认的交易处理器");
                    }
                    DefaultMsgProcess = _MsgProcessorConfigInfo;
                }
                if (!MsgProcessorConfigInfo_List.ContainsKey(TranNO))
                {
                    MsgProcessorConfigInfo_List.Add(TranNO, _MsgProcessorConfigInfo);
                }
                else
                {
                    MsgProcessorConfigInfo_List[TranNO] = _MsgProcessorConfigInfo;
                    _logger.Error("重要提示：交易处理器，交易(" + TranNO + ")有相同的配置项");
                }
            }
            return MsgProcessorConfigInfo_List;
        }

        /// <summary>
        /// 获取ServiceConfig.xml配置文件路径
        /// </summary>
        public static string GetServiceConfig()
        {
            string BaseFile = CommonFunction.GetExecutingAssemblyLocation();
            return Path.GetFullPath(Path.Combine(BaseFile, "Config/ServiceConfig.xml"));
        }
        /// <summary>
        /// 获取ServiceConfig.xml配置文件内容
        /// </summary>
        /// <returns></returns>
        public static XmlDocument GetServiceConfigContent()
        {
            string ServiceConfigFilePath = GetServiceConfig();
            if (!File.Exists(ServiceConfigFilePath))
            {
                _logger.Error("重要提示：服务配置文件不存在，服务无法启动，请检查" + ServiceConfigFilePath);
                throw new Exception("服务配置文件不存在，服务无法启动，请检查" + ServiceConfigFilePath);
            }
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(ServiceConfigFilePath);
            }
            catch (Exception ee)
            {
                _logger.Error("重要提示：读取配置文件出错，服务无法启动，请检查" + ServiceConfigFilePath, ee);
                throw new Exception("读取配置文件出错，服务无法启动，请检查" + ServiceConfigFilePath);
            }
            return xmlDoc;
        }
    }

    /// <summary>
    /// 接口消息处理类
    /// </summary>
    public class MsgHandler
    {
        private static ILog _logger = LogManager.GetLogger(typeof(MsgHandler));
        /// <summary>
        /// 接入接口处理器队列，通过配置文件，通过反射获取处理队列
        /// key为交易号
        /// </summary>
        private static Dictionary<string, IService> _service_List = null;
        /// <summary>
        /// 默认的交易报文处理器
        /// </summary>
        private static IService Default_Service = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        static MsgHandler()
        {
            //InitMsgProcessor();
        }

        /// <summary>
        /// 处理接口请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        public static void Execute(RequestMessage request, ref ResponseMessage response)
        {
            //接口处理器
            IService _IService = null;

            #region 获取交易处理器
            ServerCache.Init();
            if (_service_List != null)
            {
                _service_List.TryGetValue(request.method, out _IService);
            }

            if (_IService != null)
            {
                bool issuccess = _IService.Execute(request.method, request, ref response);
            }
            else
            {
                MessageHelper.SetError(response, "EGG0004");
            }
            #endregion
        }

        /// <summary>
        /// 读取接口服务配置文件，初始化交易处理器
        /// </summary>
        public static void InitMsgProcessor()
        {
            string BaseFile = CommonFunction.GetExecutingAssemblyLocation();
            string TempAssembleFilePath = string.Empty;
            //读取接口服务配置文件
            XmlDocument ServerConfigFile = ServerHelper.GetServiceConfigContent();

            #region 初始化交易处理器

            //初始化值
            if (_service_List == null)
                _service_List = new Dictionary<string, IService>();
            Default_Service = null;

            //读取配置文件
            MsgProcessorConfigInfo DefaultMsgProcess = null;
            Dictionary<string, MsgProcessorConfigInfo> MsgProcessSetting = ServerHelper.GetMsgProcessorInfo(ServerConfigFile, ref DefaultMsgProcess);
            if (MsgProcessSetting != null || DefaultMsgProcess != null)
            {
                #region 加载非默认的接口处理器
                if (MsgProcessSetting != null)
                {
                    foreach (string Keys in MsgProcessSetting.Keys)
                    {
                        MsgProcessorConfigInfo _MsgProcessorConfigInfo = MsgProcessSetting[Keys];
                        if (_MsgProcessorConfigInfo != null)
                        {
                            try
                            {
                                TempAssembleFilePath = Path.GetFullPath(Path.Combine(BaseFile, "bin", _MsgProcessorConfigInfo.AssembleNameStr));
                                if (!File.Exists(TempAssembleFilePath))
                                {
                                    _logger.Error("重要提示：加载交易（" + Keys + "）的配置时，发现" + TempAssembleFilePath + "不存在");
                                    continue;
                                }
                                Assembly TempAssembly = Assembly.LoadFrom(TempAssembleFilePath);
                                if (TempAssembly == null)
                                {
                                    _logger.Error("重要提示：加载交易（" + Keys + "）的配置时，发现" + TempAssembleFilePath + "构建程序集失败");
                                    continue;
                                }
                                IService _IMsgInfo = (IService)TempAssembly.CreateInstance(_MsgProcessorConfigInfo.TypeNameStr);
                                if (_IMsgInfo != null)
                                {
                                    _logger.Warn("交易号:" + Keys + "，处理器:" + _MsgProcessorConfigInfo.AssembleNameStr + ":" + _MsgProcessorConfigInfo.TypeNameStr + "加载成功");
                                    if (!MsgHandler._service_List.ContainsKey(Keys))
                                    {
                                        MsgHandler._service_List.Add(Keys, _IMsgInfo);
                                    }
                                }
                                else
                                {
                                    _logger.Error("交易号:" + Keys + "，处理器:" + _MsgProcessorConfigInfo.AssembleNameStr + ":" + _MsgProcessorConfigInfo.TypeNameStr + "加载失败");
                                }
                            }
                            catch (Exception ee)
                            {
                                _logger.Error("加载交易号:" + Keys + "交易处理器失败", ee);
                            }
                        }
                        else
                        {
                            _logger.Error("交易号:" + Keys + "，处理器没有获取到配置信息");
                        }
                    }
                }

                #endregion
            }
            else
            {
                _logger.Error("重要提示：系统没有发现任何配置的交易处理器，请检查");
                throw new Exception("重要提示：系统没有发现任何配置的交易处理器，请检查");
            }
            if (_service_List == null && _service_List.Count == 0 && Default_Service == null)
            {
                _logger.Error("重要提示：系统没有加载交易处理器，请检查");
                throw new Exception("重要提示：系统没有加载交易处理器，请检查");
            }

            #endregion

        }
    }

    /// <summary>
    /// 服务器缓存类
    /// </summary>
    public class ServerCache
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ServerCache));

        /// <summary>
        /// 服务端缓存是否初始化
        /// </summary>
        private static bool IsInit = false;
        private static object objAny = new object();

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static string _dbConnectStr = "";

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string DBConnectStr
        {
            get
            {
                if (_dbConnectStr.Length == 0)
                {
                    _dbConnectStr = ConfigurationManager.ConnectionStrings["SCDB"].ConnectionString.ToString();
                }
                return _dbConnectStr;
            }
        }

        /// <summary>
        /// 读取交易服务配置文件，初始化交易处理器
        /// </summary>
        public static void Init()
        {
            if (IsInit)
                return;

            lock (objAny)
            {
                if (IsInit)
                    return;

                #region 报文处理器初始化
                //读取交易服务配置文件，初始化交易处理器
                MsgHandler.InitMsgProcessor();
                #endregion

                IsInit = true;
            }
        }
    }
}