using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using college_assignment_mvc_project.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace college_assignment_mvc_project.Controllers
{
    public static class AuthorizationMiddleware
    {
        public static bool IsAdminAuthorized(ISession Session)
        {
            return Session.GetString("Role") == "ADMIN";
        }

        public static bool IsUserAuthorized(ISession Session)
        {
            return Session.GetString("Role") == "USER";
        }

        public static bool IsGuideAuthorized(ISession Session)
        {
            return Session.GetString("Role") == "GUIDE";
        }

        public static bool IsUserLoggedIn(ISession Session)
        {
            var is_logged_in = Session.GetString("IsUserLoggedIn") != "UserNotConnected";
           // if (!is_logged_in)
                //TempData["msg"] = "<script>alert('Must log in to see this page');</script>";
            return is_logged_in;
        }

        public static bool IsItGuideOrAdmin(ISession Session)
        {
            return IsGuideAuthorized(Session) || IsAdminAuthorized(Session);
        }
    }   
}