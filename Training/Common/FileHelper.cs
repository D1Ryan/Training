using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Configuration;

namespace SAPI.Common
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public class FileHelper
    {
        // Http展示路径
        private static string httpUrl;
        // ftp登陆名
        private static string userName;
        // ftp登陆密码
        private static string passWord;
        // ftp地址
        private static string ftpUrl;
        //ftp模块名，主要区分不同的域名前辍，http时减去此部分
        private static string ftpModel;
        //销售图片文件夹
        private static string salesFolder;

        /// <summary>
        /// 靜態構造函數
        /// </summary>
        static FileHelper()
        {
            httpUrl = WebConfigurationManager.AppSettings["httpUrl"];
            userName = WebConfigurationManager.AppSettings["ftpUid"];
            passWord = WebConfigurationManager.AppSettings["ftpPwd"];
            ftpUrl = WebConfigurationManager.AppSettings["ftpUrl"];
            ftpModel = WebConfigurationManager.AppSettings["ftpModel"];
            salesFolder = WebConfigurationManager.AppSettings["salesFolder"];
        }

        /// <summary>
        /// 控件上传文件
        /// </summary>
        /// <param name="postfile">文件控件</param>
        /// <param name="modelName">文件类型，图片:Images</param>
        /// <returns></returns>
        public static UploadResult GetUploadFile(HttpPostedFileBase postfile, string modelName)
        {
            string fileName = postfile.FileName;
            //针对Edge，选择文件后，文件名是全路径时，去掉路径
            if (fileName.IndexOf(@"\") > 0)
            {
                fileName = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
            }
            //如果是图片，要重命名
            if (modelName == "Images")
            {
                fileName = fileName.Substring(fileName.LastIndexOf("."));
                fileName = Guid.NewGuid().ToString().Replace("-","") + fileName;
            }
            //指定的上传目录
            string ftpPath = ftpModel + salesFolder;
            // 校验FTP上目录有效性，如不存在则创建   
            Uri uri = new Uri(ftpUrl + ftpPath);
            bool isDirExist = IsDirectoryExist(uri.ToString(), userName, passWord);
            if (isDirExist == false)
            {
                CreateFullDirectoryAtFtp(uri.ToString(), userName, passWord);
                // WriteDefaultFile(uri.ToString(), userName, passWord);
            }
            var stream = postfile.InputStream;
            stream.Position = 0;
            return UploadToFtpServer(ftpPath, fileName, stream);
        }

        /// <summary>
        /// 本地文件直接上传
        /// </summary>
        /// <param name="fileName">本地路径</param>
        /// <param name="modelName">文件类型，图片:Images</param>
        /// <returns></returns>
        public static UploadResult GetUploadLocalFile(string fileName, string modelName)
        {
            string fullPath = fileName;
            UploadResult result = new UploadResult();
            //如果文件不存在，则返回
            if (!File.Exists(fileName))
            {
                result.ErrMsg = "FileNotExist";
                return result;
            }
            //针对Edge，选择文件后，文件名是全路径时，去掉路径
            if (fileName.IndexOf(@"\") > 0)
            {
                fileName = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
            }
            //如果是图片，要重命名
            if (modelName == "Images")
            {
                fileName = fileName.Substring(fileName.LastIndexOf("."));
                fileName = Guid.NewGuid().ToString() + fileName;
            }
            //指定的上传目录
            string ftpPath = ftpModel + salesFolder;
            // 校验FTP上目录有效性，如不存在则创建   
            Uri uri = new Uri(ftpUrl + ftpPath);
            bool isDirExist = IsDirectoryExist(uri.ToString(), userName, passWord);
            if (isDirExist == false)
            {
                CreateFullDirectoryAtFtp(uri.ToString(), userName, passWord);
                // WriteDefaultFile(uri.ToString(), userName, passWord);
            }
            //把本地文件转成字节流
            FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            Stream stream = new MemoryStream(bytes)
            {
                Position = 0
            };
            return UploadToFtpServer(ftpPath, fileName, stream);
        }

        /// <summary>
        /// 上傳文檔到ftp
        /// </summary>
        /// <param name="ftpPath">文件目录</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fs"></param>
        protected static UploadResult UploadToFtpServer(string ftpPath, string fileName, Stream fs)
        {
            UploadResult result = new UploadResult();
            try
            {
                Uri uri = new Uri(ftpUrl + ftpPath + fileName);
                FtpWebRequest uploadRequest = (FtpWebRequest)WebRequest.Create(uri);
                uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
                uploadRequest.Credentials = new NetworkCredential(userName, passWord);
                //執行一個命令后關閉連接.
                uploadRequest.KeepAlive = false;
                uploadRequest.UseBinary = true;
                Stream requestStream = uploadRequest.GetRequestStream();
                byte[] buffer = new byte[1024];
                int bytesRead;
                while (true)
                {
                    bytesRead = fs.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    requestStream.Write(buffer, 0, bytesRead);
                }
                //關閉流
                requestStream.Close();
                result.FileLength = fs.Length.ToString();
                result.ErrMsg = string.Empty;
                result.FileName = fileName;
                result.FilePath = ftpPath + fileName;
                result.FTPPath = ftpUrl + ftpPath + fileName;
                //http时，因域名不同，ftpPath需去掉ftpModel的值
                ftpPath = ftpPath.Substring(ftpModel.Length);
                result.HttpPath = httpUrl + ftpPath + fileName;
            }
            catch (Exception ex)
            {
                result.ErrMsg = ex.InnerException.ToString();
            }
            finally
            {
                fs.Close();
            }
            return result;
        }

        /// <summary>
        /// 在ftp上创建文件夹（若目录不存在则依序创建）
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="_userName"></param>
        /// <param name="_password"></param>
        public static void CreateFullDirectoryAtFtp(string directoryPath, string _userName, string _password)
        {
            Uri uriDir = new Uri(directoryPath);
            directoryPath = uriDir.AbsolutePath;
            directoryPath = directoryPath.Replace(@"\", "/");
            directoryPath = directoryPath.Replace("//", "/");
            string[] aryDirctoryName = directoryPath.Split('/');
            string realPath = "";
            realPath = ftpUrl;
            for (int i = 0; i < aryDirctoryName.Length; i++)
            {
                if (aryDirctoryName[i] != string.Empty)
                {
                    realPath = realPath + "/" + aryDirctoryName[i];
                    if (!IsDirectoryExist(realPath, _userName, _password))
                    {
                        CreateDirectoryAtFtp(realPath, _userName, _password);
                    }
                }
            }
        }

        /// <summary>
        /// 在ftp上創建文件夹
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="_userName"></param>
        /// <param name="_password"></param>
        public static void CreateDirectoryAtFtp(string directoryName, string _userName, string _password)
        {
            try
            {
                Uri uri = new Uri(directoryName);
                FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(uri);
                listRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                listRequest.Credentials = new NetworkCredential(_userName, _password);
                listRequest.KeepAlive = false;
                //執行一個命令后關閉連接
                FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 判断指定路径是否存在于ftp服务器上面
        /// </summary>
        /// <param name="fullDirectory">ftp文件目录</param> 
        /// <param name="_userName">ftp用户名</param> 
        /// <param name="_password">ftp密码</param> 
        public static bool IsDirectoryExist(string fullDirectory, string _userName, string _password)
        {
            if (!fullDirectory.EndsWith("/"))
            {
                fullDirectory += "/";
            }
            bool result = false;
            //执行ftp命令，活动目录下所有文件列表
            Uri uriDir = new Uri(fullDirectory);
            FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(uriDir);
            listRequest.Method = WebRequestMethods.Ftp.ListDirectory;
            listRequest.Credentials = new NetworkCredential(_userName, _password);
            listRequest.KeepAlive = false;
            //执行一个命令后关闭连接
            FtpWebResponse listResponse = null;
            try
            {
                listResponse = (FtpWebResponse)listRequest.GetResponse();
                result = true;
            }
            catch
            {
                result = false;
            }
            finally
            {
                if (listResponse != null)
                {
                    listResponse.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// httpPath与ftpPath互转
        /// </summary>
        /// <param name="ftpUrl">拟转地址</param>
        /// <param name="isToHttp">是否转成httpPath</param>
        /// <returns></returns>
        public static string ReplaceUrl(string ftpUrl, bool isToHttp)
        {
            if (!string.IsNullOrEmpty(ftpUrl))
            {
                ftpUrl = ftpUrl.ToLower();
                //是否从ftpPath转为httpPath
                if (isToHttp)
                {
                    if (ftpUrl.Contains(FileHelper.ftpUrl))
                    {
                        ftpUrl = ftpUrl.Replace(FileHelper.ftpUrl, httpUrl);
                    }
                }
                else
                {
                    if (ftpUrl.Contains(httpUrl))
                    {
                        ftpUrl = ftpUrl.Replace(httpUrl, FileHelper.ftpUrl);
                    }
                }
                return ftpUrl;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 写一个默认文件
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <param name="_userName"></param>
        /// <param name="_password"></param>
        /// <returns></returns>
        protected static bool WriteDefaultFile(string ftpPath, string _userName, string _password)
        {
            Uri uri = new Uri(ftpPath);
            try
            {
                FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(uri);
                listRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                listRequest.Credentials = new NetworkCredential(_userName, _password);
                //執行一個命令后關閉連接
                listRequest.KeepAlive = false;
                FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse();
                string fullPath = ftpPath + @"/ftpPath.ini";
                Stream write = GetWriteStream(fullPath, _userName, _password);
                //在ftp上創建文件
                byte[] context = System.Text.Encoding.Default.GetBytes("ftpPath=" + ftpPath);
                write.Write(context, 0, context.Length);
                write.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取一个写文件流
        /// </summary>
        /// <param name="fileFullName">FTP全路径</param>
        /// <param name="_userName"></param>
        /// <param name="_password"></param>
        /// <returns></returns>
        protected static Stream GetWriteStream(string fileFullName, string _userName, string _password)
        {
            FtpWebRequest uploadRequest = (FtpWebRequest)WebRequest.Create(new Uri(fileFullName));
            uploadRequest.Method = WebRequestMethods.Ftp.UploadFile;
            uploadRequest.Credentials = new NetworkCredential(_userName, _password);
            //執行一個命令后關閉連接.
            uploadRequest.KeepAlive = false;
            uploadRequest.UseBinary = true;
            return uploadRequest.GetRequestStream();
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ftpPath"></param>
        public static bool DeleteFile(string ftpPath)
        {
            bool opResult = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                string uri = ftpPath;
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(userName, passWord);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = string.Empty;
                result = streamReader.ReadToEnd();
                opResult = true;
            }
            catch
            {
                opResult = false;
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (ftpResponseStream != null)
                {
                    ftpResponseStream.Close();
                }
                if (ftpWebResponse != null)
                {
                    ftpWebResponse.Close();
                }
            }
            return opResult;
        }

        /// <summary>
        /// 判断文件是否存在检查
        /// </summary>
        /// <param name="ftpPath"></param>
        /// <returns></returns>
        public static bool IsFileExist(string ftpPath)
        {
            bool opResult = false;
            string fileName = ftpPath.Substring(ftpPath.LastIndexOf(@"/") + 1);
            FtpWebRequest ftpWebRequest = null;
            WebResponse webResponse = null;
            StreamReader reader = null;
            try
            {
                string url = ftpPath;
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(url));
                ftpWebRequest.Credentials = new NetworkCredential(userName, passWord);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line == fileName)
                    {
                        opResult = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch
            {
                opResult = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }
            return opResult;
        }
    }

    /// <summary>
    /// 上传文件类型
    /// </summary>
    [Serializable]
    public class UploadResult
    {
        /// <summary>
        /// 错误信息，成功为空
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 文档名称 xxx.jpg
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileLength { get; set; }

        /// <summary>
        /// 文档的路径名 img/20150203/xxx.jpg
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// http文档的全路径名 http://cdn.xx.com/img/20150203/xxx.jpg
        /// </summary>
        public string HttpPath { get; set; }

        /// <summary>
        /// 文件所在FTP的路径 ftp://cdn.xx.com/img/20150203/xxx.jpg
        /// </summary>
        public string FTPPath { get; set; }
    }
}