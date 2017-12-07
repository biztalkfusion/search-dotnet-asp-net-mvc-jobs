using NYCJobsWeb.Data.Context;
using NYCJobsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NYCJobsWeb.Client
{
    public class AccountClient
    {
        private readonly ISearchContext _searchContext;

        public AccountClient()
        {
            _searchContext = new SearchContext();
        }
        public Login GetUserDetails(Login loginModel)
        {
            try
            {
                loginModel.UserName = loginModel.UserName.Trim();
                var passwordEn = Common.SecurityUtilities.EncryptUrl(loginModel.Password, Common.Settings.EncSecretkey);
                var userdetails = new Login();
                var user = _searchContext.Users.FirstOrDefault(_ => _.UserName == loginModel.UserName && _.Password == passwordEn);
                if (user != null && user.Id > 0)
                {
                    var userRole = _searchContext.UserRoles.FirstOrDefault(_ => _.UserId == user.Id);
                    userdetails.RoleId = userRole?.RoleId ?? 0;
                    userdetails.UserName = user.UserName;
                    userdetails.Id = user.Id;
                    userdetails.RoleName = userRole.Role.Name;
                }
                return userdetails;
            }
            catch (Exception exception)
            {
            }
            return loginModel;
        }
    }
}