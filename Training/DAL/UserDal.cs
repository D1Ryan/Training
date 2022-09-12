using log4net;
using SAPI.Common;
using SAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SAPI.DAL
{
    public class UserDal
    {
        private static ILog dLogger = LogManager.GetLogger("User");

        /// <summary>
        /// 将字串转换成用户列表
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public static List<UserBrief> GetUsersByString(string users)
        {
            List<UserBrief> brief = new List<UserBrief>();
            string[] allusers = users.Split(",".ToCharArray());
            foreach (string aUser in allusers)
            {
                try
                {
                    string[] uNames = aUser.Split("-".ToCharArray());
                    brief.Add(new UserBrief() { User_Id = Convert.ToInt32(uNames[0]), User_Name = uNames[1] });
                }
                catch
                {
                    continue;
                }
            }
            return brief;
        }

        ///// <summary>
        ///// 查询行数
        ///// </summary>
        ///// <param name="cuid">当前用户编号</param>
        ///// <param name="loginName">账号</param>
        ///// <param name="userName">姓名</param>
        ///// <param name="iCode">用户编码</param>
        ///// <param name="isGroup">是否用户组</param>
        ///// <param name="isEnable">有效状态(0,1)</param>
        ///// <returns>记录数据</returns>
        //public static int GetUserCount(long cuid, string loginName, string userName, string iCode, string isGroup, string isEnable)
        //{
        //    string sql = @"SELECT COUNT(User_ID) FROM User";
        //    List<SqlParameter> sqlParaList = new List<SqlParameter>();
        //    string sqlwhere = " WHERE User_ID<>" + cuid.ToString();
        //    //如果当前登录用户非超级操作员（Inner_Code=001）时，只返回当前用户创建的用户列表
        //    if (iCode != "001")
        //    {
        //        sqlwhere += string.Format(" AND Upper_Code = '{0}'", iCode);
        //    }
        //    //账号
        //    if (!string.IsNullOrWhiteSpace(loginName))
        //    {
        //        sqlwhere += " AND Login_Name like @Login_Name";
        //        sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@Login_Name", SqlDbType.NVarChar, SqlFieldHelper.SetString("%" + loginName + "%")));
        //    }
        //    //姓名
        //    if (!string.IsNullOrWhiteSpace(userName))
        //    {
        //        sqlwhere += " AND User_Name like @User_Name";
        //        sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@User_Name", SqlDbType.NVarChar, SqlFieldHelper.SetString("%" + userName + "%")));
        //    }
        //    //是否用户组
        //    if ((!string.IsNullOrWhiteSpace(isGroup)) && (isGroup != "-1"))
        //    {
        //        sqlwhere += " AND Is_Group = @Is_Group";
        //        sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@Is_Group", SqlDbType.Bit, SqlFieldHelper.SetBit(Convert.ToBoolean(isGroup))));
        //    }
        //    //账户状态
        //    if ((!string.IsNullOrWhiteSpace(isEnable)) && (isEnable != "-1"))
        //    {
        //        sqlwhere += " AND Is_Enabled = @Is_Enabled";
        //        sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@Is_Enabled", SqlDbType.Bit, SqlFieldHelper.SetBit(Convert.ToBoolean(isEnable))));
        //    }
        //    return Convert.ToInt32(new DataHelper().ExecuteScalar(sql + sqlwhere, sqlParaList.ToArray(), CommandType.Text));
        //}

        ///// <summary>
        ///// 查询返回列表
        ///// </summary>
        ///// <param name="cuid">当前用户编号</param>
        ///// <param name="loginName">账号</param>
        ///// <param name="userName">姓名</param>
        ///// <param name="iCode">用户编码</param>
        ///// <param name="isGroup">是否用户组</param>
        ///// <param name="isEnable">账户状态</param>
        ///// <param name="since"></param>
        ///// <param name="size"></param>
        ///// <returns></returns>
        //public static List<User> GetUserList(long cuid, string loginName, string userName, string iCode, string isGroup, string isEnable, int since, int size)
        //{
        //    List<User> list = new List<User>();
        //    string sql = @"SELECT User_ID,Is_Group,Login_Name,User_Name,Is_Enabled,Create_Time,Update_Time,Operator FROM User";
        //    List<SqlParameter> sqlParaList = new List<SqlParameter>();
        //    string sqlwhere = " WHERE User_ID<>" + cuid.ToString();
        //    //如果当前登录用户非初始操作员（Inner_Code=001）时，只返回当前用户创建的用户列表
        //    if (iCode != "001")
        //    {
        //        sqlwhere += string.Format(" AND Upper_Code = '{0}'", iCode);
        //    }
        //    //账号
        //    if (!string.IsNullOrWhiteSpace(loginName))
        //    {
        //        sqlwhere += " AND Login_Name like @Login_Name";
        //        sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@Login_Name", SqlDbType.NVarChar, SqlFieldHelper.SetString("%" + loginName + "%")));
        //    }
        //    //姓名
        //    if (!string.IsNullOrWhiteSpace(userName))
        //    {
        //        sqlwhere += " AND User_Name like @User_Name";
        //        sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@User_Name", SqlDbType.NVarChar, SqlFieldHelper.SetString("%" + userName + "%")));
        //    }
        //    //是否用户组
        //    if ((!string.IsNullOrWhiteSpace(isGroup)) && (isGroup != "-1"))
        //    {
        //        sqlwhere += " AND Is_Group = @Is_Group";
        //        sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@Is_Group", SqlDbType.Bit, SqlFieldHelper.SetBit(Convert.ToBoolean(isGroup))));
        //    }
        //    //账户状态
        //    if ((!string.IsNullOrWhiteSpace(isEnable)) && (isEnable != "-1"))
        //    {
        //        sqlwhere += " AND Is_Enabled = @Is_Enabled";
        //        sqlParaList.Add(SqlParameterSelf.CreateSqlParameter("@Is_Enabled", SqlDbType.Bit, SqlFieldHelper.SetBit(Convert.ToBoolean(isEnable))));
        //    }
        //    //排序和分页
        //    string sqlOffset = " ORDER BY User_ID OFFSET (" + since + ") ROWS FETCH NEXT (" + size + ") ROWS ONLY";
        //    DataTable dt = new DataHelper().ExecuteTable(sql + sqlwhere + sqlOffset, sqlParaList.ToArray(), CommandType.Text);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        list = dt.List<User>();
        //    }
        //    return list;
        //}

        public static object GetUserManager(string userName)
        {
            string querymax = string.Format("SELECT TOP 1 CONVERT(VARCHAR(9),c.UserId)+'-'+c.ChineseName FROM [User] c,[User] f WHERE c.UserId=f.SuperiorId and f.ChineseName ='{0}'", userName);
            return new DataHelper().ExecuteScalar(querymax, null, CommandType.Text);
        }


        /// <summary>
        /// 获取管理员列表
        /// </summary>
        /// <returns></returns>
        public static List<UserBrief> GetUpperUserList()
        {
            List<UserBrief> list = new List<UserBrief>();
            string querymax = "SELECT UserId AS User_Id,TRIM(ChineseName) AS User_Name FROM [User] WHERE StatusId<>0 and len(RoleId)>1";
            DataTable dt = new DataHelper().ExecuteTable(querymax, null, CommandType.Text);
            if (dt != null && dt.Rows.Count > 0)
            {
                list = dt.List<UserBrief>();
            }
            return list;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="uid">用户编号</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <param name="loginName">操作员账号</param>
        /// <returns></returns>
        public static bool ModifyPassword(long uid, string oldPassword, string newPassword, string loginName)
        {
            string updateSql = string.Format("UPDATE User SET Password='{0}',Update_Time=GETDATE(),Operator='{1}' WHERE User_ID={2} AND Password='{3}' ", newPassword, loginName, uid, oldPassword);
            int updateRows = new DataHelper().ExecuteNonQuery(updateSql);
            Log4Info info = new Log4Info("修改密码，User_ID:" + uid.ToString(), "System", loginName, "", "", "", "", "");
            dLogger.Info(info);
            return (updateRows > 0);
        }

        /// <summary>
        /// 标记停用（单）
        /// </summary>
        /// <param name="uid">用户编号</param>
        /// <param name="loginName">操作员账号</param>
        /// <returns></returns>
        public static bool MarkUserDisable(long uid, string loginName)
        {
            string updateSql = string.Format("Update User SET Is_Enabled=0,Update_Time=GETDATE(),Operator='{0}' WHERE User_ID={1}", loginName, uid);
            int updateRows = new DataHelper().ExecuteNonQuery(updateSql);
            Log4Info info = new Log4Info("停用账号，User_ID:" + uid.ToString(), "System", loginName, "", "", "", "", "");
            dLogger.Info(info);
            return (updateRows > 0);
        }

        /// <summary>
        /// 删除（多）
        /// </summary>
        /// <param name="uid">（多）用户编号</param>
        /// <param name="loginName">操作员账号</param>
        /// <returns></returns>
        public static bool DeleteUsersByIds(string uid, string loginName)
        {
            string updateSql = string.Format("DELETE FROM User WHERE User_ID in ({0})", uid);
            int updateRows = new DataHelper().ExecuteNonQuery(updateSql);
            Log4Info info = new Log4Info("删除账号，User_ID:" + uid.ToString(), "System", loginName, "", "", "", "", "");
            dLogger.Info(info);
            return (updateRows > 0);
        }

        ///// <summary>
        ///// 获取指定用户已拥有权限的部门列表
        ///// </summary>
        ///// <param name="uid">用户编号</param>
        ///// <param name="suid">超管的用户编码</param>
        ///// <param name="db">数库库对象</param>
        ///// <param name="istree">排序优先上级编码(tree时为true,非tree为false)</param>
        ///// <returns></returns>
        //public static List<UserDepts> GetUserDepts(long uid, long suid, POADB db, bool istree)
        //{
        //    //获取当前操作员拥有权限的部门列表,需判断是否为超级操作员(User_ID=1)，
        //    //Y:超级操作员默认拥有所有部门权限，super user have all dept auth,
        //    //N:非超级操作员要串表PubDepts.Dept_ID=PubDeptAuths.Dept_ID查当前用户的部门权限User_ID
        //    //获取超级操作员账号：
        //    List<UserDepts> luDepts;
        //    if (uid == suid)
        //    {
        //        if (istree)
        //        {
        //            luDepts = db.PubDepts.Where(a => a.Is_Enabled == true)
        //                      .Select(s => new UserDepts { Inner_Code = s.Inner_Code, Upper_Code = s.Upper_Code, Dept_ID = s.Dept_ID, Dept_Name = s.Dept_Name, Is_Enabled = true })
        //                      .OrderBy(o => o.Upper_Code).ThenBy(t => t.Inner_Code).ToList();
        //        }
        //        else
        //        {
        //            luDepts = db.PubDepts.Where(a => a.Is_Enabled == true)
        //                      .Select(s => new UserDepts { Inner_Code = s.Inner_Code, Upper_Code = s.Upper_Code, Dept_ID = s.Dept_ID, Dept_Name = s.Dept_Name, Is_Enabled = true })
        //                      .OrderBy(o => o.Inner_Code).ToList();
        //        }
        //    }
        //    else
        //    {
        //        if (istree)
        //        {
        //            luDepts = db.PubDepts.Join(db.PubDeptAuths, d => d.Dept_ID, a => a.Dept_ID,
        //                     (d, a) => new UserDepts { User_ID = a.User_ID, Inner_Code = d.Inner_Code, Upper_Code = d.Upper_Code, Dept_ID = d.Dept_ID, Dept_Name = d.Dept_Name, Is_Enabled = d.Is_Enabled })
        //                     .Where(d => d.Is_Enabled == true && d.User_ID == uid).OrderBy(o => o.Upper_Code).ThenBy(t => t.Inner_Code).ToList();
        //        }
        //        else
        //        {
        //            luDepts = db.PubDepts.Join(db.PubDeptAuths, d => d.Dept_ID, a => a.Dept_ID,
        //                      (d, a) => new UserDepts { User_ID = a.User_ID, Inner_Code = d.Inner_Code, Upper_Code = d.Upper_Code, Dept_ID = d.Dept_ID, Dept_Name = d.Dept_Name, Is_Enabled = d.Is_Enabled })
        //                      .Where(d => d.Is_Enabled == true && d.User_ID == uid).OrderBy(o => o.Inner_Code).ToList();
        //        }
        //    }
        //    return luDepts;
        //}

        ///// <summary>
        ///// 设置用户部门权限
        ///// </summary>
        ///// <param name="uid">用户编号</param>
        ///// <param name="gids">用户群组编号</param>
        ///// <param name="loginName">操作员账号</param>
        ///// <returns></returns>
        //public static bool SetUserDepts(string uid, string gids, string loginName)
        //{
        //    //先移除已存在的群组权限
        //    string removeOld = string.Format("DELETE FROM PubDeptAuth WHERE User_ID ={0}", uid);
        //    int updateRows = new DataHelper().ExecuteNonQuery(removeOld);///删除已有不判断
        //                //如果有新的用户群组编号
        //    if (!string.IsNullOrEmpty(gids))
        //    {
        //        //后插入新的群组权限
        //        string insertNew = string.Format("INSERT INTO PubDeptAuth SELECT {0},Dept_ID,GETDATE(),'{1}' FROM PubDept WHERE Dept_ID in ({2})", uid, loginName, gids);
        //        updateRows = new DataHelper().ExecuteNonQuery(insertNew);
        //        string msg = string.Format("设置部门权限，User_ID:{0},Dept_ID:{1}", uid, gids);
        //        Log4Info info = new Log4Info(msg, "System", loginName, "", "", "", "", "");
        //        dLogger.Info(info);
        //    }
        //    else
        //    {
        //        updateRows = 1;
        //    }
        //    return (updateRows > 0);
        //}

        ///// <summary>
        ///// 更新部门权限的部门编号
        ///// </summary>
        ///// <param name="newDeptId">新部门编号</param>
        ///// <param name="oldDeptId">旧部门编号</param>
        ///// <returns></returns>
        //public static bool UpdateDeptAuthByDeptId(string newDeptId, string oldDeptId)
        //{
        //    if ((string.IsNullOrEmpty(oldDeptId)) | (string.IsNullOrEmpty(newDeptId)))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        string updateDeptId = string.Format("UPDATE PubDeptAuth SET Dept_ID='{0}' WHERE Dept_ID='{1}'", newDeptId, oldDeptId);
        //        return new DataHelper().ExecuteNonQuery(updateDeptId) > 0;
        //    }
        //}

        ///// <summary>
        ///// 获取指定用户已拥有系统权限列表（菜单）
        ///// </summary>
        ///// <param name="uID">用户编号</param>
        ///// <param name="suid">超管的用户编码</param>
        ///// <param name="db">数库库对象</param>
        ///// <returns></returns>
        //public static List<UserAuths> GetUserSystems(long uID, long suid, POADB db)
        //{
        //    //获取当前操作员拥有系统权限列表,需判断是否为超级操作员，
        //    //Y:超级操作员默认拥有所有系统权限，super user have all system auth,
        //    //N:非超级操作员要串表PubAuthItems，UserAuths查当前用户的系统权限User_ID
        //    //获取超级操作员账号：
        //    List<UserAuths> luAuths;
        //    if (uID == suid)
        //    {
        //        luAuths = db.PubAuthItems.Where(a => a.Is_Enabled == true && a.Is_Menu == true)
        //                 .Select(s => new UserAuths { Inner_Code = s.Inner_Code, Upper_Code = s.Upper_Code, Auth_ID = s.Auth_ID, Auth_Name = s.Auth_Name, Is_Enabled = true })
        //                 .OrderBy(o => o.Upper_Code).ThenBy(t => t.Inner_Code).ToList();
        //    }
        //    else
        //    {
        //        luAuths = db.PubAuthItems.Join(db.UserAuths, d => d.Auth_ID, a => a.Auth_ID,
        //                  (d, a) => new UserAuths { User_ID = a.User_ID, Inner_Code = d.Inner_Code, Upper_Code = d.Upper_Code, Auth_ID = d.Auth_ID, Auth_Name = d.Auth_Name, Is_Enabled = d.Is_Enabled, Is_Menu = d.Is_Menu })
        //                  .Where(d => d.Is_Enabled == true && d.Is_Menu == true && d.User_ID == uID)
        //                  .OrderBy(o => o.Upper_Code).ThenBy(t => t.Inner_Code).ToList();
        //    }
        //    return luAuths;
        //}

        ///// <summary>
        ///// 获取当前用户已拥有的群组继承的部门名称列表
        ///// </summary>
        ///// <param name="uid">用户编号</param>
        ///// <returns></returns>
        //public static List<AuthNames> GetUserGroupSystems(long uid)
        //{
        //    string queryDept = string.Format("SELECT Auth_Name FROM PubAuthItem WHERE Auth_ID in(SELECT Auth_ID FROM UserAuth WHERE User_ID in(SELECT Group_ID AS User_ID FROM PubGroupAuth WHERE User_ID={0}))", uid);
        //    DataTable dt = new DataHelper().ExecuteTable(queryDept, null, CommandType.Text);
        //    List<AuthNames> list = new List<AuthNames>();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        list = dt.List<AuthNames>();
        //    }
        //    return list;
        //}

        ///// <summary>
        ///// 设置用户部门权限
        ///// </summary>
        ///// <param name="uid">用户编号</param>
        ///// <param name="gids">用户群组编号</param>
        ///// <param name="loginName">操作员账号</param>
        ///// <returns></returns>
        //public static bool SetUserSystems(string uid, string gids, string loginName)
        //{
        //    //先移除已存在的群组权限
        //    string removeOld = string.Format("DELETE FROM UserAuth WHERE User_ID ={0}", uid);
        //    //删除已有不判断操作结果
        //    int updateRows = new DataHelper().ExecuteNonQuery(removeOld);
        //    //如果有新的用户群组编号
        //    if (!string.IsNullOrEmpty(gids))
        //    {
        //        //后插入新的群组权限
        //        string insertNew = string.Format("INSERT INTO UserAuth SELECT {0},Auth_ID,GETDATE(),'{1}' FROM PubAuthItem WHERE Auth_ID in ({2})", uid, loginName, gids);
        //        updateRows = new DataHelper().ExecuteNonQuery(insertNew);
        //        string msg = string.Format("设置系统权限，User_ID:{0},Auth_ID:{1}", uid, gids);
        //        Log4Info info = new Log4Info(msg, "System", loginName, "", "", "", "", "");
        //        dLogger.Info(info);
        //    }
        //    else
        //    {
        //        updateRows = 1;
        //    }
        //    return (updateRows > 0);
        //}

        ///// <summary>
        ///// 获取指定用户已拥有权限的部门列表
        ///// </summary>
        ///// <param name="uid">用户编号</param>
        ///// <param name="suid">超管的用户编码</param>
        ///// <param name="db">数库库对象</param>
        ///// <returns></returns>
        //public static List<UserBrief> GetUserGroups(long uid, long suid)
        //{
        //    string queryGroup = "SELECT User_ID,Login_Name FROM User WHERE Is_Group = 1 AND Is_Enabled = 1";
        //    if (uid != suid)
        //    {
        //        queryGroup += string.Format(" AND User_ID in(SELECT Group_ID AS User_ID FROM PubGroupAuth WHERE User_ID={0})", uid);
        //    }
        //    DataTable dt = new DataHelper().ExecuteTable(queryGroup, null, CommandType.Text);
        //    List<UserBrief> list = new List<UserBrief>();
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        list = dt.List<UserBrief>();
        //    }
        //    return list;
        //}

        ///// <summary>
        ///// 设置用户群组权限
        ///// </summary>
        ///// <param name="uid">用户编号</param>
        ///// <param name="gids">用户群组编号</param>
        ///// <param name="loginName">操作员账号</param>
        ///// <returns></returns>
        //public static bool SetUserGroups(string uid, string gids, string loginName)
        //{
        //    //先移除已存在的群组权限
        //    string removeOld = string.Format("DELETE FROM PubGroupAuth WHERE User_ID ={0}", uid);
        //    //删除已有权限不判断执行结果
        //    int updateRows = new DataHelper().ExecuteNonQuery(removeOld);
        //    //如果有新的用户群组编号
        //    if (!string.IsNullOrEmpty(gids))
        //    {
        //        //后插入新的群组权限
        //        string insertNew = string.Format("INSERT INTO PubGroupAuth SELECT {0},User_ID,GETDATE(),'{1}' FROM User WHERE User_ID in ({2})", uid, loginName, gids);
        //        updateRows = new DataHelper().ExecuteNonQuery(insertNew);
        //        string msg = string.Format("设置群组权限，User_ID:{0},GorupIDs:{1}", uid, gids);
        //        Log4Info info = new Log4Info(msg, "System", loginName, "", "", "", "", "");
        //        dLogger.Info(info);
        //    }
        //    else
        //    {
        //        updateRows = 1;
        //    }
        //    return (updateRows > 0);
        //}

        ///// <summary>
        ///// 查询用户的菜单项目
        ///// </summary>
        ///// <returns></returns>
        //public static List<UserMenu> GetMenuListByUserId(long userId)
        //{
        //    /*SELECT Auth_ID AS Menu_ID, Inner_Code, Auth_Name AS Menu_Name, Auth_Value AS Menu_Link FROM PubAuthItem WHERE Is_Enabled = 1 AND Is_Menu = 1 AND Auth_ID in 
        //     (SELECT Auth_ID FROM UserAuth WHERE User_ID IN (SELECT Group_ID FROM PubGroupAuth WHERE User_ID = 3 UNION SELECT 3)) ORDER BY Inner_Code*/
        //    List<UserMenu> menus = new List<UserMenu>();
        //    List<UserMenu> result = new List<UserMenu>();
        //    string sql = "SELECT Auth_ID AS Menu_ID, Inner_Code, Auth_Name AS Menu_Name, Auth_Value AS Menu_Link FROM PubAuthItem WHERE Is_Enabled = 1 AND Is_Menu = 1 AND Auth_ID IN";
        //    sql += string.Format(" (SELECT Auth_ID FROM UserAuth WHERE User_ID IN (SELECT Group_ID FROM PubGroupAuth WHERE User_ID = {0} UNION SELECT {1})) ORDER BY Inner_Code", userId, userId);
        //    DataTable dt = new DataHelper().ExecuteTable(sql);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        result = dt.List<UserMenu>();
        //        foreach (UserMenu menu in result)
        //        {
        //            //系统菜单Inner_Code每级3位长,如果是系统菜单，增加到列表；否则为子菜单，添加到系统菜单的列表中
        //            if (menu.Inner_Code.Trim().Length == 3)
        //            {
        //                menu.SubMenuList = new List<UserMenu>();
        //                menus.Add(menu);
        //            }
        //            else
        //            {
        //                var eSubMenu = menus.Find(row => row.Inner_Code == menu.Inner_Code.Substring(0, 3));
        //                if (eSubMenu != null)
        //                {
        //                    eSubMenu.SubMenuList.Add(menu);
        //                }

        //            }
        //        }
        //    }
        //    return menus;
        //}

        ///// <summary>
        ///// 获取用户拥有权限的页面地址列表
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public static List<AuthMenusUrl> GetAuthMenusUrlByUserId(long userId)
        //{
        //    List<AuthMenusUrl> menus = new List<AuthMenusUrl>();
        //    string sql = "SELECT Auth_Value AS MenuUrl FROM PubAuthItem WHERE Is_Enabled = 1 AND Auth_Value IS NOT NULL AND Is_Menu = 1 AND Auth_ID IN";
        //    sql += string.Format(" (SELECT Auth_ID FROM UserAuth WHERE User_ID IN (SELECT Group_ID FROM PubGroupAuth WHERE User_ID = {0} UNION SELECT {1})) ORDER BY Auth_Value", userId, userId);
        //    DataTable dt = new DataHelper().ExecuteTable(sql);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        menus = dt.List<AuthMenusUrl>();
        //    }
        //    return menus;
        //}
    }
}