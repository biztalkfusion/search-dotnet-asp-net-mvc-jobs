using NYCJobsWeb.Client;
using NYCJobsWeb.Common;
using NYCJobsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NYCJobsWeb.Controllers
{
    public class AccountController : BaseController
    {
        private readonly AccountClient _accountClient;

        public AccountController()
        {
            _accountClient = new AccountClient();
        }
        // GET: Account
        public ActionResult Login(string returnUrl = "")
        {
            var loginModel = new Login();
            var authCookieExists = Request.Cookies.Cast<string>().Any(s => s.Contains(Settings.Aspxauth));
            if (!authCookieExists) return View(loginModel);

            if (string.IsNullOrEmpty(returnUrl) || returnUrl.Equals("/"))
                return RedirectToAction("Search", "Home");

            return Redirect(returnUrl);
            //return View(new Login());
        }

        // POST: /Account/Login
        [HttpPost]
        public ActionResult Login(Login loginModel)
        {
            var userdetails = new Login();
            ModelState["Password"].Errors.Clear();
            userdetails = _accountClient.GetUserDetails(loginModel);
            if (userdetails != null)
                userdetails.IsLoginFailed = true;

            if (userdetails != null && userdetails.Id > 0)
            {
                CreateCookie(new Login() { Id = userdetails.Id, UserName = userdetails.UserName, RoleId = userdetails.RoleId });
                return RedirectToAction("Search", "Home");
            }

            return View(userdetails);
        }

        private void CreateCookie(Login userInfo)
        {
            var authCookie = CreateAuthCookie(userInfo);
            Response.AppendCookie(authCookie);
            var identity = new Common.CommonUtilities().SetIdentity(userInfo);
            IPrincipal Identity = (IPrincipal)identity;
            ControllerContext.HttpContext.User = Identity;
        }

        /// <summary>
        /// LogOff
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            ClearData();
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Clear all session and forms authentication
        /// </summary>
        private void ClearData()
        {
            if (Session != null)
            {
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
            }

            if (Response.Cookies != null)
            {
                Response.Cookies.Clear();
                var myCookies = Request.Cookies.AllKeys;
                foreach (var cookie in myCookies)
                {
                    if (Response.Cookies[cookie] == null) continue;

                    Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Remove(cookie);
                }
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
            }

            if (FormsAuthentication.IsEnabled)
                FormsAuthentication.SignOut();
        }
    }
}