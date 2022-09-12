using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SAPI.Common
{
    /// <summary>
    /// 数据库访问辅助类
    /// </summary>
    public class DataHelper : IDisposable
    {
        private readonly string SNAME = "DAL";
        private Log4Info info;
        private SqlConnection sqlCon;
        private SqlDataAdapter sqlAdp;
        private SqlCommand sqlCmd;
        private SqlTransaction objTrans;
        private static string SqlConStr = ServerCache.DBConnectStr;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (sqlCmd.Parameters != null)
                {
                    sqlCmd.Parameters.Clear();
                }
                if (sqlCmd != null)
                {
                    sqlCmd.Dispose();
                }
            }
            finally
            {
                if (sqlCon != null)
                {
                    sqlCon.Dispose();
                }
            }
        }

        /// <summary>
        /// 创建数据库连接（默认系统配置连接字符串）
        /// </summary>
        public DataHelper()
        {
            sqlCon = new SqlConnection(SqlConStr);
    //        sqlConStr = configuration.GetConnectionString("ScheduleEntities");
        //    sqlCon = new SqlConnection(sqlConStr);
            sqlCmd = sqlCon.CreateCommand();
        }

        /// <summary>
        /// 创建数据库连接（用户指定连接字符串）
        /// </summary>
        /// <param name="sql"></param>
        public DataHelper(string sql)
        {
            sqlCon = new SqlConnection(sql);
            sqlCmd = sqlCon.CreateCommand();
        }

        private SqlCommand SetCommandParams(SqlCommand cmd, string sql, CommandType cType)
        {
            return SetCommandParams(cmd, sql, null, cType);
        }

        private SqlCommand SetCommandParams(SqlCommand cmd, string sql, SqlParameter[] sParams)
        {
            return SetCommandParams(cmd, sql, sParams, CommandType.Text);
        }

        /// <summary>
        /// 设置命令的参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sql"></param>
        /// <param name="sParams"></param>
        /// <param name="cType"></param>
        /// <returns></returns>
        private SqlCommand SetCommandParams(SqlCommand cmd, string sql, SqlParameter[] sParams, CommandType cType)
        {
            cmd.CommandType = cType;
            cmd.CommandText = sql;
            //如果有参数为命令对象设置参数
            if (sParams != null)
            {
                if (cmd.Parameters != null)
                {
                    cmd.Parameters.Clear();
                }
                cmd.Parameters.AddRange(sParams);
            }
            return cmd;
        }

        /// <summary>
        /// 获取一个数字类型为String数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            try
            {
                sqlCmd.Connection = sqlCon;
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = sql;
                sqlCon.Open();
                object i = sqlCmd.ExecuteScalar();
                sqlCon.Close();
                return i;
            }
            catch (Exception ex)
            {
                info = new Log4Info(sql, SNAME, "", "", "", "", "", "");
                //         // Logger.Error(info, ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 执行sql命令返回单个值
        /// </summary>
        /// <param name="sql">Sql查询语句|SP名称</param>
        /// <param name="cType">参数的数据类型</param>
        /// <param name="nParams">使用的参数</param>
        /// <param name="vParams">参数的值</param>
        /// <returns>System.Int64</returns>
        public object ExecuteScalar(string sql, CommandType cType, string[] nParams, string[] vParams)
        {
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = cType;
            sqlCmd.CommandText = sql;
            sqlCmd.Parameters.Clear();
            for (int j = 0; j < nParams.Length; j++)
            {
                sqlCmd.Parameters.Add(new SqlParameter(nParams[j], vParams[j].ToString()));
            }
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
            sqlCon.Open();
            object obj = sqlCmd.ExecuteScalar();
            sqlCon.Close();
            return obj;
        }

        /// <summary>
        /// 执行sql语句返回单个值
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="sParams">语句中所要的参数</param>
        /// <param name="cType">语句类型</param>
        /// <returns>首行首列的值</returns>
        public object ExecuteScalar(string sql, SqlParameter[] sParams, CommandType cType)
        {
            object result = null;
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
                sqlCon.Dispose();
            }
            sqlCon.Open();
            sqlCmd = SetCommandParams(sqlCmd, sql, sParams, cType);
            try
            {
                result = sqlCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                sqlCon.Close();
                sqlCon.Dispose();
                info = new Log4Info(sql, SNAME, "", "", "", "", "", "");
                // Logger.Error(info, ex);
            }
            finally
            {
                if (sqlCmd.Parameters != null)
                {
                    sqlCmd.Parameters.Clear();
                }
                if (sqlCon.State != ConnectionState.Closed)
                {
                    sqlCon.Close();
                    sqlCon.Dispose();
                }
            }
            return result;
        }

        /// <summary>
        /// 使用存储过程设置数据（带返回值）
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="sParams">参数</param>
        /// <param name="useTran">是否使用事务</param>
        /// <param name="ErrorStr">错误信息</param>
        /// <returns>System.Int64</returns>
        public object ExecuteScalarWithTrans(string spName, SqlParameter[] sParams, bool useTran, ref string ErrorStr)
        {
            object back;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = spName;
            sqlCmd.Parameters.Clear();
            foreach (SqlParameter param in sParams)
            {
                sqlCmd.Parameters.Add(param);
            }
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
            sqlCon.Open();
            if (useTran)
            {
                objTrans = sqlCon.BeginTransaction();
                sqlCmd.Transaction = objTrans;
            }
            try
            {
                back = sqlCmd.ExecuteScalar();
                if (useTran)
                {
                    objTrans.Commit();
                }
                return back;
            }
            catch (Exception ex)
            {
                info = new Log4Info(sqlCmd.CommandText, SNAME, "", "", "", "", "", "");
                // Logger.Error(info, ex);
                if (useTran)
                {
                    objTrans.Rollback();
                }
                ErrorStr = ex.Message;
                return -1;
            }
            finally
            {
                sqlCon.Close();
            }
        }

        /// <summary>
        /// 获取存储过程执行行数
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="colName">存储过程参数</param>
        /// <param name="dType">数据类型</param>
        /// <param name="dLength">数据类型大小</param>
        /// <param name="vParams">参数值</param>
        /// <returns>DataSet</returns>
        public int ExecuteNonQuery(string spName, ArrayList colName, ArrayList dType, ArrayList dLength, ArrayList vParams)
        {
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = spName;
            sqlCmd.Parameters.Clear();
            for (int i = 0; i < colName.Count; i++)
            {
                if (int.Parse(dLength[i].ToString()) <= 0)
                {
                    sqlCmd.Parameters.Add(colName[i].ToString(), (SqlDbType)dType[i]);
                }
                else
                {
                    sqlCmd.Parameters.Add(colName[i].ToString(), (SqlDbType)dType[i], int.Parse(dLength[i].ToString()));
                }
                sqlCmd.Parameters[i].Value = vParams[i].ToString();
            }
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
            sqlCon.Open();
            int eCount = sqlCmd.ExecuteNonQuery();
            sqlCon.Close();
            return eCount;
        }

        /// <summary>
        /// 获取记录集第一行第一列的值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="nParams"></param>
        /// <param name="vParams"></param>
        /// <returns></returns>
        public string GetStringFromTopCellBySqlString(string sql, string[] nParams, string[] vParams)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = GetDataSet(sql, CommandType.Text, nParams, vParams);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].CovertString();
                }
            }
            catch (Exception ex)
            {
                info = new Log4Info(sql, SNAME, "", "", "", "", "", "");
                // Logger.Error(info, ex);
                return string.Empty;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
            }
            return string.Empty;
        }

        /// <summary>
        /// 使用存储过程设置数据（带返回值）
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="nParams">存储过程参数</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="DataLength">数据类型大小</param>
        /// <param name="vParams">参数值</param>
        /// <param name="backParam">返回值参数</param>
        /// <param name="useSP">是否使用事务</param>
        /// <returns>System.Int64</returns>
        public object GetBackValueBySPWithTrans(string spName, ArrayList nParams, ArrayList DataType, ArrayList DataLength, ArrayList vParams, SqlParameter backParam, bool useSP)
        {
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = spName;
            sqlCmd.Parameters.Clear();
            for (int i = 0; i < nParams.Count; i++)
            {
                if (int.Parse(DataLength[i].ToString()) <= 0)
                {
                    sqlCmd.Parameters.Add(nParams[i].ToString(), (SqlDbType)DataType[i]);
                }
                else
                {
                    sqlCmd.Parameters.Add(nParams[i].ToString(), (SqlDbType)DataType[i], int.Parse(DataLength[i].ToString()));
                }
                if (vParams[i] == null)
                {
                    sqlCmd.Parameters[i].Value = "";
                }
                else
                {
                    sqlCmd.Parameters[i].Value = vParams[i].ToString();
                }
            }
            //如果返回值参数不为空
            if (backParam != null)
            {
                sqlCmd.Parameters.Add(backParam);
            }
            //sqlCmd.Parameters.Add("@value", SqlDbType.BigInt, 8);
            //      sqlCmd.Parameters[nParams.Count].Direction = ParameterDirection.Output;
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
            sqlCon.Open();
            if (useSP)
            {
                objTrans = sqlCon.BeginTransaction();
                sqlCmd.Transaction = objTrans;
            }
            try
            {
                sqlCmd.ExecuteScalar();
                if (useSP)
                {
                    objTrans.Commit();
                }
                return sqlCmd.Parameters[nParams.Count].Value;
            }
            catch (Exception ex)
            {
                info = new Log4Info(sqlCmd.CommandText, SNAME, "", "", "", "", "", "");
                // Logger.Error(info, ex);
                if (useSP)
                {
                    objTrans.Rollback();
                }
                return -1;
            }
            finally
            {
                sqlCon.Close();
            }
        }

        /// <summary>
        /// 调用存储过程获得返回值
        /// </summary>
        /// <param name="spName">过程名称</param>
        /// <param name="nParams">参数名</param>
        /// <param name="dType">参数类型</param>
        /// <param name="dLength">参数长度</param>
        /// <param name="vParams">参数值</param>
        /// <param name="backParam">返回值参数</param>
        /// <returns></returns>
        public object GetBackValueBySP(string spName, ArrayList nParams, ArrayList dType, ArrayList dLength, ArrayList vParams, SqlParameter backParam)
        {
            return GetBackValueBySPWithTrans(spName, nParams, dType, dLength, vParams, backParam, false);
        }

        ///// <summary>
        ///// 修改数据库信息返回Int32
        ///// </summary>
        ///// <param name="sql">Sql查询语句</param>
        ///// <returns>System.Int32</returns>
        //public int ExecuteNonQueryBySql(string sql)
        //{
        //    return ExecuteNonQueryWithTrans(sql, false, out string errMsg);
        //}

        /// <summary>
        /// 执行sql语句（增、删、改语句）返回所影响的行数
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <returns>所影响的行数</returns>
        public int ExecuteNonQuery(string sql)
        {
            return ExecuteNonQueryUseParams(sql, null, CommandType.Text);
        }

        /// <summary>
        /// 修改数据库信息返回Int32同时获取错误信息
        /// </summary>
        /// <param name="sqlString">Sql查询语句</param>
        /// <param name="useTran">是否使用SP</param>
        ///<param name="errMsg">错误消息</param>
        /// <returns>System.Int32</returns>
        public int ExecuteNonQueryWithTrans(string sqlString, bool useTran, out string errMsg)
        {
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = sqlString;
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
            sqlCon.Open();
            if (useTran)
            {
                objTrans = sqlCon.BeginTransaction(); //开始一个事务
                sqlCmd.Transaction = objTrans;
            }
            try
            {
                int i = sqlCmd.ExecuteNonQuery();
                if (useTran)
                {
                    objTrans.Commit();
                }
                errMsg = "";
                return i;
            }
            catch (Exception ex)
            {
                if (useTran)
                {
                    objTrans.Rollback();
                }
                info = new Log4Info(sqlString, SNAME, "", "", "", "", "", "");
                errMsg = ex.Message;
                // Logger.Error(info, ex);
                return -1;
            }
            finally
            {
                sqlCon.Close();
            }
        }

        /// <summary>
        /// 获取存储过程事务影响行数
        /// </summary>
        /// <param name="spName">Sql存储过程</param>
        /// <param name="nParams">使用的参数</param>
        /// <param name="vParams">参数的值</param>
        /// <returns>System.Int32</returns>
        public bool ExecuteNonQueryFromSPTrans(string spName, string[] nParams, string[] vParams)
        {
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = spName;
            for (int j = 0; j < nParams.Length; j++)
            {
                sqlCmd.Parameters.Add(new SqlParameter(nParams[j], vParams[j]));
            }
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
            sqlCon.Open();
            objTrans = sqlCon.BeginTransaction();
            sqlCmd.Transaction = objTrans;
            try
            {
                sqlCmd.ExecuteNonQuery();
                objTrans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                info = new Log4Info(sqlCmd.CommandText, SNAME, "", "", "", "", "", "");
                // Logger.Error(info, ex);
                objTrans.Rollback();
                return false;
            }
            finally
            {
                sqlCon.Close();
            }
        }

        /// <summary>
        /// 获取存储过程事务影响行数
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="nParams">存储过程参数</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="DataLength">数据类型大小</param>
        /// <param name="vParams">参数值</param>
        /// <returns>System.Int32</returns>
        public int ExecuteNonQueryBySPTrans(string spName, ArrayList nParams, ArrayList DataType, ArrayList DataLength, ArrayList vParams)
        {
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandText = spName;
            sqlCmd.Parameters.Clear();
            for (int i = 0; i < nParams.Count; i++)
            {
                if (int.Parse(DataLength[i].ToString()) <= 0)
                {
                    sqlCmd.Parameters.Add(nParams[i].ToString(), (SqlDbType)DataType[i]);
                }
                else
                {
                    sqlCmd.Parameters.Add(nParams[i].ToString(), (SqlDbType)DataType[i], int.Parse(DataLength[i].ToString()));
                }

                if (vParams[i] == null)
                {
                    sqlCmd.Parameters[i].Value = "";
                }
                else
                {
                    sqlCmd.Parameters[i].Value = vParams[i].ToString();
                }
            }
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
            sqlCon.Open();
            objTrans = sqlCon.BeginTransaction();
            sqlCmd.Transaction = objTrans;

            try
            {
                int i = sqlCmd.ExecuteNonQuery();
                objTrans.Commit();
                return i;
            }
            catch (Exception ex)
            {
                info = new Log4Info(sqlCmd.CommandText, SNAME, "", "", "", "", "", "");
                // Logger.Error(info, ex);
                objTrans.Rollback();
                return -1;
            }
            finally
            {
                sqlCon.Close();
            }
        }

        /// <summary>
        /// 获取事务执行SqlString影响行数
        /// </summary>
        /// <param name="sqlString">Sql查询语句</param>
        /// <param name="nParams">使用的参数</param>
        /// <param name="vParams">参数的值</param>
        /// <param name="DataType">参数的数据类型</param>
        /// <returns>System.Int32</returns>
        public int ExecuteNonQueryBySqlStringTrans(string sqlString, string[] nParams, string[] vParams)
        {
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = sqlString;
            for (int j = 0; j < nParams.Length; j++)
            {
                sqlCmd.Parameters.Add(new SqlParameter(nParams[j], vParams[j].ToString()));
            }
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
            sqlCon.Open();
            objTrans = sqlCon.BeginTransaction();
            sqlCmd.Transaction = objTrans;
            try
            {
                int i = sqlCmd.ExecuteNonQuery();
                objTrans.Commit();
                return i;
            }
            catch (Exception ex)
            {
                info = new Log4Info(sqlString, SNAME, "", "", "", "", "", "");
                // Logger.Error(info, ex);
                objTrans.Rollback();
                return -1;
            }
            finally
            {
                sqlCon.Close();
            }
        }

        /// <summary>
        /// 执行sql语句（增、删、改语句）返回所影响的行数
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="paras">语句中所要的参数</param>
        /// <returns>所影响的行数</returns>
        public int ExecuteNonQuery(string sql, SqlParameter[] paras)
        {
            return ExecuteNonQueryUseParams(sql, paras, CommandType.Text);
        }

        /// <summary>
        /// 获取存储过程执行结果
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public bool ExecuteProcedure(string sql, SqlParameter[] paras)
        {
            return ExecuteNonQueryUseParams(sql, paras, CommandType.StoredProcedure) > 0;
        }

        /// <summary>
        /// 执行sql语句（增、删、改语句）返回所影响的行数
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="paras">语句中所要的参数</param>
        /// <param name="cType">语句类型</param>
        /// <returns>所影响的行数</returns>
        public int ExecuteNonQueryUseParams(string sql, SqlParameter[] paras, CommandType cType)
        {
            int result = 0;
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
                sqlCon.Dispose();
            }
            sqlCon.Open();
            sqlCmd = SetCommandParams(sqlCmd, sql, paras, cType);
            try
            {
                result = sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sqlCon.Close();
                sqlCon.Dispose();
                info = new Log4Info(sql, SNAME, "", "", "", "", "", "");
                // Logger.Error(info, ex);
            }
            finally
            {
                if (sqlCmd.Parameters != null)
                {
                    sqlCmd.Parameters.Clear();
                }
                if (sqlCon.State != ConnectionState.Closed)
                {
                    sqlCon.Close();
                    sqlCon.Dispose();
                }
            }
            return result;
        }

        /// <summary>
        /// 获取一个记录集
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tbName"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql, string tbName)
        {
            try
            {
                sqlAdp = new SqlDataAdapter(sql, sqlCon);
                DataSet ds = new DataSet();
                sqlAdp.Fill(ds, tbName);
                return ds;
            }
            catch (Exception ex)
            {
                info = new Log4Info(sql, SNAME, "", "", "", "", "", "");
                //// Logger.Error(info, ex);
                return null;
            }
            finally
            {
                if (sqlCon != null)
                    sqlCon.Close();
            }
        }

        /// <summary>
        /// 获取一个记录集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql)
        {
            return GetDataSet(sql, "sheet1");
        }

        /// <summary>
        /// 使用存储过程获取数据（获取一个记录集）
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="nParams">存储过程参数</param>
        /// <param name="DataType">数据类型</param>
        /// <param name="DataLength">数据类型大小</param>
        /// <param name="ColValue">参数值</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string spName, ArrayList nParams, ArrayList DataType, ArrayList DataLength, ArrayList ColValue)
        {
            sqlAdp = new SqlDataAdapter(spName, sqlCon);
            sqlAdp.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlAdp.SelectCommand.Parameters.Clear();
            for (int i = 0; i < nParams.Count; i++)
            {
                if (int.Parse(DataLength[i].ToString()) <= 0)
                {
                    sqlAdp.SelectCommand.Parameters.Add(nParams[i].ToString(), (SqlDbType)DataType[i]);
                }
                else
                {
                    sqlAdp.SelectCommand.Parameters.Add(nParams[i].ToString(), (SqlDbType)DataType[i], int.Parse(DataLength[i].ToString()));
                }
                if (ColValue[i] != null)
                {
                    sqlAdp.SelectCommand.Parameters[i].Value = ColValue[i].ToString();
                }
                else
                {
                    sqlAdp.SelectCommand.Parameters[i].Value = "";
                }

            }
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
            sqlCon.Open();
            DataSet ds = new DataSet();
            sqlAdp.Fill(ds, "Table");
            sqlCon.Close();
            return ds;
        }

        /// <summary>
        /// 使用存储过程获取数据（获取一个记录集）
        /// </summary>
        /// <param name="sql">Sql查询语句</param>
        /// <param name="cType">命令类型</param>
        /// <param name="nParams">使用的参数</param>
        /// <param name="vParams">参数的值</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string sql, CommandType cType, string[] nParams, string[] vParams)
        {
            sqlCmd.Connection = sqlCon;
            sqlCmd.CommandType = cType;
            sqlCmd.CommandText = sql;
            sqlCmd.Parameters.Clear();
            for (int j = 0; j < nParams.Length; j++)
            {
                sqlCmd.Parameters.Add(new SqlParameter(nParams[j], vParams[j].ToString()));
            }
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
            sqlCon.Open();
            DataSet ds = new DataSet();
            try
            {
                sqlAdp = new SqlDataAdapter(sqlCmd);
                sqlAdp.Fill(ds, "tbName");
            }
            finally
            {
                sqlCon.Close();
            }
            return ds;
        }

        /// <summary>
        /// 使用存储过程获取数据（获取一个记录集）
        /// </summary>
        /// <param name="sql">Sql查询语句</param>
        /// <param name="sParams">使用的参数</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSet(string sql, Dictionary<string, object> sParams)
        {
            sqlCmd = SetCommandParams(sqlCmd, sql, CommandType.StoredProcedure);
            sqlCmd.Parameters.Clear();
            foreach (string KeyString in sParams.Keys)
            {
                sqlCmd.Parameters.Add(new SqlParameter(KeyString, sParams[KeyString]));
            }
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
            DataSet ds = new DataSet();
            sqlCon.Open();
            try
            {
                sqlAdp = new SqlDataAdapter(sqlCmd);
                sqlAdp.Fill(ds, "Table");
            }
            catch (Exception ex)
            {
                info = new Log4Info(sql, SNAME, "", "", "", "", "", "");
                // Logger.Error(info, ex);
            }
            finally
            {
                sqlCon.Close();
            }
            return ds;
        }

        /// <summary>
        /// 执行sql语句返回数据集
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <returns>数据集</returns>
        public DataTable ExecuteTable(string sql)
        {
            return ExecuteTable(sql, null, CommandType.Text);
        }

        /// <summary>
        /// 执行sql语句返回数据集
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="sParams">语句中所要的参数</param>
        /// <returns>数据集</returns>
        public DataTable ExecuteTable(string sql, SqlParameter[] sParams)
        {
            return ExecuteTable(sql, sParams, CommandType.Text);
        }

        /// <summary>
        /// 执行sql语句返回数据集
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="sParams">语句中所要的参数</param>
        /// <param name="cType">语句类型</param>
        /// <returns>数据集</returns>
        public DataTable ExecuteTable(string sql, SqlParameter[] sParams, CommandType cType)
        {
            var ds = ExecuteDateSet(sql, sParams, cType, false);
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            else return null;
        }

        /// <summary>
        /// 执行sql语句事务返回数据集
        /// </summary>
        /// <param name="sql">要执行的sql语句</param>
        /// <param name="sParams">语句中所要的参数</param>
        /// <param name="cType">语句类型</param>
        /// <param name="useTrans">是否使用事务</param>
        /// <returns>数据集</returns>
        public DataSet ExecuteDateSet(string sql, SqlParameter[] sParams, CommandType cType, bool useTrans)
        {
            DataSet ds = new DataSet();
            if (sqlCon.State != ConnectionState.Closed)
            {
                sqlCon.Close();
            }
            sqlCon.Open();
            sqlCmd = SetCommandParams(sqlCmd, sql, sParams, cType);
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
            if (useTrans)
            {
                objTrans = sqlCon.BeginTransaction();
                sqlCmd.Transaction = objTrans;
            }
            try
            {
                da.Fill(ds);
                if (useTrans)
                {
                    objTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (useTrans)
                {
                    objTrans.Rollback();
                }
                sqlCon.Close();
                sqlCon.Dispose();
                info = new Log4Info(sql, SNAME, "", "", "", "", "", "");
                // Logger.Error(info, ex);
            }
            finally
            {
                if (sqlCon.State != ConnectionState.Closed)
                {
                    sqlCon.Close();
                    sqlCon.Dispose();
                }
            }
            return ds;
        }

    }
}