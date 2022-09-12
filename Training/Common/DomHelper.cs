using SAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SAPI.Common
{
    /// <summary>
    /// 菜单数据对象
    /// </summary>
    public class CheckboxTreeItem
    {
        /// <summary>
        /// 显示的文本
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 显示文本对应的值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 级别代码（上下级关联）
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 是否为直系继承子节点
        /// </summary>
        public bool IsSon { get; set; }
        /// <summary>
        /// 子菜单集合
        /// </summary>
        public IList<CheckboxTreeItem> Items { get; set; }
    }

    /// <summary>
    /// 复选框树控件
    /// </summary>
    public static class CheckboxTreeHelper
    {
        /// <summary>
        /// 节点自上而下生成树节点（内循环）
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="Model"></param>
        /// <param name="isSon">是否直系子孙</param>
        /// <returns></returns>
        public static string BuildTreeNode(this HtmlHelper helper, CheckboxTreeItem Model, bool isSon)
        {
            if (null == Model)
            {
                return string.Empty;
            }
            string checkBoxId = "chk" + Model.Id;
            string checkBoxValue = Model.Value;
            StringBuilder sb = new StringBuilder();
            if (isSon)
            {
                sb.Append("<li>");
            }
            else
            {
                sb.Append("<li class=\"subnode\">");
            }
            sb.Append("<label style=\"cursor:pointer;\">");
            sb.AppendFormat("<input type=\"checkbox\" onchange=\"ChangeNodesCheck(this);\" id =\"{0}\" value=\"{1}\" />", checkBoxId, checkBoxValue);
            sb.AppendFormat("<span>{0}</span>", Model.Text);
            sb.Append("</label>");
            //如果有子项，循环生成子节点
            if ((null != Model.Items) && (Model.IsSon))
            {
                //class赋值，方便显示/隐藏
                sb.Append("<ul class=\"subnode\">");
                foreach (CheckboxTreeItem item in Model.Items)
                {
                    sb.Append(BuildTreeNode(helper, item, true));
                }
                sb.Append("</ul>");
            }
            sb.Append("</li>");
            return sb.ToString();
        }

        /// <summary>
        /// 生成树根节点
        /// </summary>
        /// <param name="helper"></param>  
        /// <param name="Models"></param>
        /// <returns></returns>
        public static string BuildCheckboxTree(this HtmlHelper helper, IList<CheckboxTreeItem> Models)
        {
            if (null == Models)
            {
                return string.Empty;
            }
            //创建树根
            StringBuilder sb = new StringBuilder();
            sb.Append("<div>");
            sb.Append("<ul id=\"treeroot\">");
            //从顶级节点开始自上而下循环生成子树
            foreach (CheckboxTreeItem item in Models)
            {
                sb.Append(BuildTreeNode(helper, item, item.IsSon));
            }
            sb.Append("</ul>");
            sb.Append("</div>");
            return sb.ToString();
        }

        ///// <summary>
        ///// 生成部门树(根)
        ///// </summary>
        ///// <param name="uDepts"></param>
        ///// <param name="upperCode"></param>
        ///// <returns></returns>
        //public static List<CheckboxTreeItem> GetDeptCheckTree(List<CheckboxTreeItem> root, List<UserDepts> uDepts)
        //{
        //    string upperCode = "0";
        //    //先循环生成直系子孙树
        //    List<UserDepts> suDepts = uDepts.Where(w => w.Upper_Code == upperCode).ToList();
        //    CheckboxTreeItem subtree;
        //    foreach (UserDepts dept in suDepts)
        //    {
        //        //标注子节点已生成
        //        dept.Is_Enabled = false;
        //        subtree = new CheckboxTreeItem() { Value = dept.Dept_ID, Text = dept.Dept_Name, Id = dept.Inner_Code, IsSon = true };
        //        root.Add(subtree);
        //        GetDeptSubTrees(subtree, uDepts, dept.Inner_Code);
        //    }
        //    //对非顶级节点及其子孙，生成一般节点，,不生成子树
        //    List<UserDepts> nuDepts = uDepts.Where(w => w.Is_Enabled == true).ToList();
        //    foreach (UserDepts dept in nuDepts)
        //    {
        //        dept.Is_Enabled = false;
        //        subtree = new CheckboxTreeItem() { Value = dept.Dept_ID, Text = dept.Dept_Name, Id = dept.Inner_Code, IsSon = false };
        //        root.Add(subtree);
        //    }
        //    return root;
        //}

        ///// <summary>
        ///// 循环生成部门子树（节点）
        ///// </summary>
        ///// <param name="item">树节点</param>
        ///// <param name="uDepts">数据集</param>
        ///// <param name="upperCode">上级编码</param>
        ///// <returns></returns>
        //public static CheckboxTreeItem GetDeptSubTrees(CheckboxTreeItem treeNode, List<UserDepts> uDepts, string upperCode)
        //{
        //    List<UserDepts> suDepts = uDepts.Where(w => w.Upper_Code == upperCode).ToList();
        //    CheckboxTreeItem subtree;
        //    foreach (UserDepts dept in suDepts)
        //    {
        //        dept.Is_Enabled = false;
        //        subtree = new CheckboxTreeItem() { Value = dept.Dept_ID, Text = dept.Dept_Name, Id = dept.Inner_Code, IsSon = true };
        //        if (treeNode.Items == null)
        //        {
        //            treeNode.Items = new List<CheckboxTreeItem>();
        //        }
        //        treeNode.Items.Add(subtree);
        //        GetDeptSubTrees(subtree, uDepts, dept.Inner_Code);
        //    }
        //    return treeNode;
        //}

        ///// <summary>
        ///// 生成系统权限树(根)
        ///// </summary>
        ///// <param name="uAuths"></param>
        ///// <param name="upperCode"></param>
        ///// <returns></returns>
        //public static List<CheckboxTreeItem> GetAuthCheckTree(List<CheckboxTreeItem> root, List<UserAuths> uAuths)
        //{
        //    string upperCode = "0";
        //    //先循环生成直系子孙树
        //    List<UserAuths> suAuths = uAuths.Where(w => w.Upper_Code == upperCode).ToList();
        //    CheckboxTreeItem subtree;
        //    foreach (UserAuths auth in suAuths)
        //    {
        //        auth.Is_Enabled = false;
        //        subtree = new CheckboxTreeItem() { Value = auth.Auth_ID.ToString(), Text = auth.Auth_Name, Id = auth.Inner_Code, IsSon = true };
        //        root.Add(subtree);
        //        GetAuthSubTrees(subtree, uAuths, auth.Inner_Code);
        //    }
        //    //对非顶级节点及其子孙，生成一般节点，,不生成子树
        //    List<UserAuths> nuAuths = uAuths.Where(w => w.Is_Enabled == true).ToList();
        //    foreach (UserAuths auth in nuAuths)
        //    {
        //        auth.Is_Enabled = false;
        //        subtree = new CheckboxTreeItem() { Value = auth.Auth_ID.ToString(), Text = auth.Auth_Name, Id = auth.Inner_Code, IsSon = false };
        //        root.Add(subtree);
        //    }
        //    return root;
        //}

        ///// <summary>
        ///// 循环生成系统权限子树（节点）
        ///// </summary>
        ///// <param name="item">树节点</param>
        ///// <param name="uAuths">数据集</param>
        ///// <param name="upperCode">上级编码</param>
        ///// <returns></returns>
        //public static CheckboxTreeItem GetAuthSubTrees(CheckboxTreeItem treeNode, List<UserAuths> uAuths, string upperCode)
        //{
        //    List<UserAuths> suAuths = uAuths.Where(w => w.Upper_Code == upperCode).ToList();
        //    CheckboxTreeItem subtree;
        //    foreach (UserAuths auth in suAuths)
        //    {
        //        auth.Is_Enabled = false;
        //        subtree = new CheckboxTreeItem() { Value = auth.Auth_ID.ToString(), Text = auth.Auth_Name, Id = auth.Inner_Code, IsSon = true };
        //        if (treeNode.Items == null)
        //        {
        //            treeNode.Items = new List<CheckboxTreeItem>();
        //        }
        //        treeNode.Items.Add(subtree);
        //        GetAuthSubTrees(subtree, uAuths, auth.Inner_Code);
        //    }
        //    return treeNode;
        //}
    }

    /// <summary>
    /// DataSet通用方法
    /// </summary>
    public class DataSetHelper
    {
        /// <summary>
        /// 导出DataSet表数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetExportFromDataSet(DataSet set)
        {
            StringBuilder strHtml = new StringBuilder();
            strHtml.Append("<table>");
            if ((set != null) && (set.Tables[0] != null))
            {
                DataTable table = set.Tables[0];
                int cols = table.Columns.Count;
                int rows = table.Rows.Count;
                //生成列标题（列数未知）
                strHtml.Append("<tr>");
                for (int c = 0; c < cols; c++)
                {
                    strHtml.Append("<td>" + table.Columns[c].ColumnName + "</td>");
                }
                strHtml.Append("</tr>");
                //每行数据处理
                foreach (DataRow row in table.Rows)
                {
                    strHtml.Append("<tr>");
                    //行内每列处理
                    for (int rc = 0; rc < cols; rc++)
                    {
                        strHtml.Append("<td>" + row[rc].CovertString() + "</td>");
                    }
                    strHtml.Append("</tr>");
                }
            }
            strHtml.Append("</table>");
            return strHtml.ToString();
        }
    }
}
