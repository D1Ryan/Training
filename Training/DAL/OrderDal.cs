
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SAPI.Common;
using SAPI.Models;

namespace SAPI.DAL
{
    public class OrderDal
    {
        /// <summary>
        /// 获取查询订单条件及参数
        /// </summary>
        /// <param name="filter">条件</param>
        /// <param name="queryCondition">查询条件</param>
        /// <returns></returns>
        private static List<SqlParameter> GetQueryCondition(OrderList filter, out string queryCondition)
        {
            List<SqlParameter> sqlParaList = new List<SqlParameter>();
            queryCondition = " WHERE 1 = 1";
            //商品编号
            if (filter.ResourceId.HasValue)
            {
                queryCondition += " AND ResourceId = @rId";
                sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@rId", SqlDbType.Int, SqlFieldHelper.SetInt32(filter.ResourceId.Value)));
            }
            //手机号
            if (!string.IsNullOrWhiteSpace(filter.Mobile))
            {
                queryCondition += " AND User_Phone=@phone";
                sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@phone", SqlDbType.NVarChar, SqlFieldHelper.SetString(filter.Mobile)));
            }
            //日期范围
            if (!string.IsNullOrWhiteSpace(filter.StartDate))
            {
                //结束日期
                if (!string.IsNullOrWhiteSpace(filter.EndDate))
                {
                    queryCondition += " AND Create_Time BETWEEN @stime AND @etime";
                    sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@stime", SqlDbType.NVarChar, SqlFieldHelper.SetString(filter.StartDate)));
                    sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@etime", SqlDbType.NVarChar, SqlFieldHelper.SetString(filter.EndDate + " 23:59:59")));
                }
                else
                {
                    queryCondition += " AND DATEDIFF(dd,Create_Time,@stime)=0";
                    sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@stime", SqlDbType.NVarChar, SqlFieldHelper.SetString(filter.StartDate)));
                }
            }
            return sqlParaList;
        }

        /// <summary>
        /// 查询订单数
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public static int QueryOrderCount(OrderList filter)
        {
            string queryCondition;
            string queryFields = @"SELECT COUNT(Row_ID) FROM App_Order";
            List<SqlParameter> sqlParaList = GetQueryCondition(filter, out queryCondition);
            return (int)new DataHelper().ExecuteScalar(queryFields + queryCondition, sqlParaList.ToArray(), CommandType.Text);
        }

        /// <summary>
        /// 查询订单列表
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        public static List<App_Order> QueryOrderList(OrderList filter)
        {
            string queryCondition;
            List<App_Order> list = new List<App_Order>();
            string queryFields = "SELECT RowId,OrderId,OpenId,UserName,ResourceId,Title,Price,PayId,Pay_Time,User_Phone,Creator,Create_Time,SalesId FROM App_Order";
            List<SqlParameter> sqlParaList = GetQueryCondition(filter, out queryCondition);
            //排序和分页
            string order = " ORDER BY RowId DESC";
            if (filter.Size.HasValue)
            {
                order += " OFFSET (" + filter.Since.Value + ") ROWS FETCH NEXT (" + filter.Size.Value + ") ROWS ONLY";
            }
            DataTable dt = new DataHelper().ExecuteTable(queryFields + queryCondition + order, sqlParaList.ToArray(), CommandType.Text);
            if (dt != null && dt.Rows.Count > 0)
            {
                list = dt.List<App_Order>();
            }
            return list;
        }

        /// <summary>
        /// 销售分配排序移动
        /// </summary>
        /// <param name="rowId">销售分配行号</param>
        /// <param name="movedir">移动方向,0:上移;1:下移</param>
        /// <returns></returns>
        public static string MoveSalesAllot(int rowId,int movedir)
        {
            /*
            PROCEDURE SP_SAPP_MoveSalesAllot
            @rowId		int,				--App_SalesAllot.RowId
            @movedir	int,				--移动方向,0:上移;1:下移
            @msg		NVARCHAR(500) OUT   
            */

            ArrayList nParams = new ArrayList() { "@rowId", "movedir" };
            ArrayList dType = new ArrayList() { SqlDbType.Int, SqlDbType.Int };
            ArrayList dLength = new ArrayList() {10,10};
            ArrayList vParams = new ArrayList() { rowId,movedir };
            SqlParameter backParam = new SqlParameter("@msg", SqlDbType.NVarChar, 500);
            backParam.Direction = ParameterDirection.Output;
            return new DataHelper().GetBackValueBySP("SP_SAPP_MoveSalesAllot", nParams, dType, dLength, vParams, backParam).ToString();
        }

        ///// <summary>
        ///// 获取订单列表By年份
        ///// </summary>
        ///// <param name="year"></param>
        ///// <returns></returns>
        //public static string[] QueryOrderByYear(string year)
        //{
        //    string[] list;
        //    string queryFields = string.Format("SELECT SUBSTRING(Holiday,6,5)+','+CONVERT(VARCHAR(5),Holiday_Type)+','+Name FROM App_Order WHERE Holiday BETWEEN '{0}-01-01' AND '{0}-12-31' ORDER BY Holiday", year);
        //    DataTable dt = new DataHelper().ExecuteTable(queryFields, null, CommandType.Text);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        int iCount = dt.Rows.Count;
        //        list = new string[iCount];
        //        for (int i = 0; i < iCount; i++)
        //        {
        //            list[i] = dt.Rows[i][0].ToString();
        //        }
        //    }
        //    else { list = new string[0] { }; }
        //    return list;
        //}

        ///// <summary>
        ///// 导出订单列表
        ///// </summary>
        ///// <param name="list"></param>
        ///// <returns></returns>
        //public static string GetExportHoliday(List<App_Order> list)
        //{
        //    StringBuilder strHtml = new StringBuilder();
        //    strHtml.Append("<table>");
        //    strHtml.Append("<tr>");
        //    strHtml.Append("<td>日期</td>");
        //    strHtml.Append("<td>假期类别</td>");
        //    strHtml.Append("<td>假期名称</td>");
        //    strHtml.Append("<td>加班补贴倍数</td>");
        //    strHtml.Append("<td>操作员</td>");
        //    strHtml.Append("<td>操作时间</td>");
        //    strHtml.Append("</tr>");
        //    foreach (App_Order row in list)
        //    {
        //        strHtml.Append("<tr>");
        //        strHtml.Append("<td>" + row.Holiday + "</td>");
        //        strHtml.Append("<td>" + row.Holiday_Type + "</td>");
        //        strHtml.Append("<td>" + row.Name + "</td>");
        //        strHtml.Append("<td>" + row.Subsidy + "</td>");
        //        strHtml.Append("<td>" + row.Operator + "</td>");
        //        strHtml.Append("<td>" + row.Create_Time.ToString("yyyy-MM-dd HH:mm:ss") + "</td>");
        //        strHtml.Append("</tr>");
        //    }
        //    strHtml.Append("</table>");
        //    return strHtml.ToString();
        //}

        ///// <summary>
        ///// 删除订单(多)
        ///// </summary>
        ///// <param name="rids">订单行编号(Row_ID)</param>
        ///// <returns></returns>
        //public static bool DeleteHolidaysByIds(string rids)
        //{
        //    string deleteSql = string.Format("DELETE FROM App_Order WHERE Row_ID in ({0})", rids);
        //    int updateRows = new DataHelper().ExecuteNonQuery(deleteSql);
        //    return (updateRows > 0);
        //}

        ///// <summary>
        ///// 新增订单
        ///// </summary>
        ///// <param name="hday"></param>
        ///// <returns></returns>
        //public static int AddHoliday(App_Order hday)
        //{
        //    //Subsidy由Reserve1转存
        //    string AddSql = "INSERT INTO App_Order (Holiday,Holiday_Type,Name,Subsidy,Create_Time,Operator) VALUES";
        //    AddSql += string.Format(" ('{0}',{1},'{2}',{3},GETDATE(),'{4}')", hday.Holiday, hday.Holiday_Type, hday.Name, hday.Reserve1, hday.Operator);
        //    return new DataHelper().ExecuteNonQuery(AddSql);
        //}

    }
}