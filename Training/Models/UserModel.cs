using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAPI.Models
{

    ////以业务为主的用户模式
    //public class UserModel
    //{
    //    public int UserID { get; set; }                 //用户编号
    //    public string UserName { get; set; }            //用户名
    //    public string Password { get; set; }            //密码
    //    public int Sex { get; set; }                    //性别，0男，1女
    //    public int Age { get; set; }                    //年龄
    //    public int Political { get; set; }              //政治面貌
    //    public int Height { get; set; }                 //身高
    //    public int Weight { get; set; }                 //体重
    //    public string Graduated { get; set; }           //毕业院校
    //    public string Professional { get; set; }        //专业
    //    public DateTime GraduatedDate { get; set; }         //毕业日期
    //    public string Address { get; set; }             //现住地址
    //    public string Phone { get; set; }               //联系电话
    //    public string ImagePath { get; set; }           //头相地址
    //    public string Other { get; set; }               //其他描述
    //}

    //以页面为主的用户登陆模型
    public class LoginModel
    {
        public string UserName { get; set; }            //用户名
        public string Password { get; set; }            //密码
    }
    ////以页面为主的用户注册模式
    //public class RegeditModel
    //{
    //    public string UserName { get; set; }            //用户名
    //    public string Password { get; set; }            //密码
    //    public int Sex { get; set; }                    //性别
    //    public int Age { get; set; }                    //年龄
    //}
}
