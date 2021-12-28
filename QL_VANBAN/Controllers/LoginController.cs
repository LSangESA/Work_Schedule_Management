using Model.DAO;
using QL_VANBAN.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QL_VANBAN.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string passWord)
        {
            if(ModelState.IsValid)
            {
                var result = new NhanVienDAO().Login(userName, passWord);

                if(result)
                {
                    var nhanVien = new NhanVienDAO().GetNhanVien(userName);
                    var user_session = new UserLogin();
                    user_session.UserName = nhanVien.USERNAME;
                    Session.Add(CommonConstants.USER_SESSION, user_session);
                    
                    ViewBag.SessionName = nhanVien.HO_TEN;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không thành công!");
                }
            }

            return View("Index");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}