using Newtonsoft.Json;
using NYCJobsWeb.Common;
using NYCJobsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using static NYCJobsWeb.Common.Enums;

namespace NYCJobsWeb.Controllers
{
    public class BaseController : Controller
    {
        public bool IsAdmin => ControllerContext.HttpContext.User.IsInRole(EnumHelper.GetEnumDescription(RoleType.Admin));
        public bool IsUser => ControllerContext.HttpContext.User.IsInRole(EnumHelper.GetEnumDescription(RoleType.User));

        public int UserId
        {
            get
            {
                var identity = (ClaimsIdentity)ControllerContext.HttpContext.User.Identity;
                return Convert.ToInt32(identity.FindFirst("Id").Value);
            }
        }

        public string UserName
        {
            get
            {
                var identity = (ClaimsIdentity)ControllerContext.HttpContext.User.Identity;
                return identity.FindFirst("UserName").Value;
            }
        }
        public long RoleId
        {
            get
            {
                var identity = (ClaimsIdentity)ControllerContext.HttpContext.User.Identity;
                return Convert.ToInt64(identity.FindFirst("RoleId")?.Value);
            }
        }

        public string RoleName
        {
            get
            {
                var identity = (ClaimsIdentity)ControllerContext.HttpContext.User.Identity;
                return identity.FindFirst("RoleName").Value;
            }
        }

        protected HttpCookie CreateAuthCookie(Login userInfoDto)
        {
            var authTicket = new FormsAuthenticationTicket(
              1,
              userInfoDto.UserName,
              DateTime.Now,
              DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes),
              true,
              JsonConvert.SerializeObject(userInfoDto), FormsAuthentication.FormsCookiePath
              );

            FormsAuthentication.SetAuthCookie(userInfoDto.UserName, false);
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(userInfoDto.UserName, false);
            authCookie.Name = FormsAuthentication.FormsCookieName;
            authCookie.Value = FormsAuthentication.Encrypt(authTicket);
            //authCookie.HttpOnly = true;
            authCookie.Expires = DateTime.Now.AddMinutes(FormsAuthentication.Timeout.TotalMinutes);

            return authCookie;
        }
    }
}