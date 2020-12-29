using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HitServicesCore.Filters;
using HitServicesCore.Helpers;
using HitServicesCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HitServicesCore.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginsUsers loginsUsers;
        public LoginController(ILogger<LoginController> logger, LoginsUsers _loginsUsers)
        {
            loginsUsers = _loginsUsers;
        }
        [ServiceFilter(typeof(LoginFilter))]
        public IActionResult Logout(bool logout)
        {
            if (logout == true)
                loginsUsers.logins["isAdmin"] = null;

            ViewBag.isValid = false;
            return View("Login");
        }

        public IActionResult Index(bool error)
        {
            ViewBag.Version = GetType().Assembly.GetName().Version.ToString();
            ViewBag.isValid = error;
            return View("Login");
        }
        public IActionResult Validation(string username, string password)
        {
            if(username==null || password ==null)
                return RedirectToAction("Index", "Login", new { error = true });
             bool isValid = false;

            if (username.Equals(loginsUsers.logins["Admin_Username"].ToString()) && password.Equals(loginsUsers.logins["Admin_Password"].ToString()))
            { loginsUsers.logins["isAdmin"] = true; isValid = true; }


            if (username.Equals(loginsUsers.logins["User_Username"].ToString()) && password.Equals(loginsUsers.logins["User_Password"].ToString()))
            { loginsUsers.logins["isAdmin"] = false; isValid = true;}

            if (isValid)
                    return RedirectToAction("Index", "Plugins");
                else
                return RedirectToAction("Index", "Login", new {error = true });
            
        }

        public class mainLoginsHelper
        {
            string user { get; set; }
            string pass { get; set; }
        }
    }
}
