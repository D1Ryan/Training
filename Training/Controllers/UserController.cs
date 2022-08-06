using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAPI.Controllers
{
    public class UserController : Controller
    {
        //// GET: User
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //登陆控制器
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]//登陆控制器
        public ActionResult Login(Models.LoginModel loginModel)
        {
            if (loginModel.UserName == "张三" && loginModel.Password == "123456")
                Response.Write("正确！");
            else
                Response.Write("不正确！");
            return View();
        }


    }
}