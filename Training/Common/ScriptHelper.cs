namespace SAPI.Common
{
    public class ScriptHelper
    {
        /// <summary>
        /// 并返回
        /// </summary>
        /// <returns></returns>
        public static string GoBack()
        {
            return "<script type='text/javascript'>history.go(-1);</script>";
        }

        /// <summary>
        /// 弹出消息并返回
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static string ShowAlert(string msg)
        {
            return string.Format("<script type='text/javascript'>alert('{0}');history.go(-1);</script>", msg);
        }

        /// <summary>
        /// 弹出消息并跳转
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="url">页面路径</param>
        /// <returns></returns>
        public static string ShowAlertAndHref(string msg, string url)
        {
            return string.Format("<script type='text/javascript'>alert('{0}');location.href='{1}';</script>", msg, url);
        }
    }
}