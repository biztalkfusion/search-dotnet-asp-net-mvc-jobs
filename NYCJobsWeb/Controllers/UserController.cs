using NYCJobsWeb.Attributes;
using NYCJobsWeb.Client;
using NYCJobsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NYCJobsWeb.Controllers
{
    [SearchAuthorize]
    public class UserController : BaseController
    {
        private readonly UserClient _userClient;

        public UserController()
        {
            _userClient = new UserClient();
        }
        // GET: User
        public ActionResult Index()
        {
            var user = new User();
            user.Folders = GetFolderList();
            user.Roles = GetRolesList();
            return View(user);
        }

        [HttpPost]
        public ActionResult Index(User userDetails)
        {
            if (userDetails != null)
            {
                var users = new Result();
                if (userDetails.Id == 0)
                {
                    users = _userClient.SaveUserDetails(userDetails);
                }
                if (users != null)
                {
                    return Json(users, JsonRequestBehavior.AllowGet);
                }
            }
                return View(userDetails);
        }

        public IList<SelectListItem> GetFolderList()
        {
            var folderList = new List<SelectListItem>();
            folderList.Add(new SelectListItem { Text = "003CLLQ", Value = "1" });
            folderList.Add(new SelectListItem { Text = "003DLSS", Value = "2" });
            folderList.Add(new SelectListItem { Text = "003EXLA", Value = "3" });
            folderList.Add(new SelectListItem { Text = "008SHAF", Value = "4" });            
            return folderList;
        }
        public IList<SelectListItem> GetRolesList()
        {
            var roleList = new List<SelectListItem>();
            roleList.Add(new SelectListItem { Text = "Admin", Value = "1" });
            roleList.Add(new SelectListItem { Text = "User", Value = "2" });
            return roleList;
        }
        public JsonResult GetUserList()
        {
            var totals = new long[1];
            var userList = _userClient.GetUserList(totals, UserId);            
            return Json(userList, JsonRequestBehavior.AllowGet);
        }
    }
}