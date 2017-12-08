using Newtonsoft.Json;
using BizTalkFusion.Solutions.Integration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using static BizTalkFusion.Solutions.Integration.Common.Enums;

namespace BizTalkFusion.Solutions.Integration.Common
{
    public class CommonUtilities
    {
        public GenericPrincipal SetIdentity(Login userInfo)
        {
            var roleName = "";
            var identity = new ClaimsIdentity();
            identity.AddClaim(new Claim("Id", userInfo.Id.ToString()));
            identity.AddClaim(new Claim("UserName", userInfo.UserName.ToString()));
            // identity.AddClaim(new Claim(Settings.EgnyteToken, userInfoDto.EgnyteToken));
            identity.AddClaim(new Claim("RoleId", userInfo.RoleId.ToString()));

            if (userInfo.RoleId == (int)RoleType.Admin)
            {
                roleName = EnumHelper.GetEnumDescription(RoleType.Admin);
            }
            else
            {
                roleName = EnumHelper.GetEnumDescription(RoleType.User);
            }
            identity.AddClaim(new Claim("RoleName", roleName));
            string[] Roles = { roleName };

            GenericPrincipal MyPrincipal = new GenericPrincipal(identity, Roles);

            return MyPrincipal;
        }

        public Login GetUserProfile(HttpRequestBase httpContext)
        {
            Login userProfileDto = null;
            var cookieName = FormsAuthentication.FormsCookieName;
            if (httpContext.IsAuthenticated && httpContext.Cookies?[cookieName] != null)
            {
                var authCookie = httpContext.Cookies[cookieName];
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                if (authTicket != null)
                {
                    userProfileDto = JsonConvert.DeserializeObject<Login>(authTicket.UserData);
                }
            }
            return userProfileDto;
        }
    }
}