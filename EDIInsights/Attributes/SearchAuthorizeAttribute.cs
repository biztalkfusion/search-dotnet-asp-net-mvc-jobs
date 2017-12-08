using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace BizTalkFusion.Solutions.Integration.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class SearchAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthenticated = base.AuthorizeCore(httpContext);
            if (isAuthenticated)
            {
                var context = new Common.CommonUtilities();

                var userProfile = context.GetUserProfile(httpContext.Request);

                if (userProfile == null)
                {
                    return false;
                }

                var identity = new Common.CommonUtilities().SetIdentity(userProfile);
                IPrincipal Identity = (IPrincipal)identity;

                httpContext.User = Identity;

            }
            return isAuthenticated;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        redirectTo = "/"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}