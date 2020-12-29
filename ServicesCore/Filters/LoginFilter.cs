
using HitServicesCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace HitServicesCore.Filters
{

    public class LoginFilter : IActionFilter
    {
        private readonly LoginsUsers loginsUsers;
        public LoginFilter(LoginsUsers _loginsUsers)
        {
            this.loginsUsers = _loginsUsers;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

            if (loginsUsers.logins["isAdmin"] == true || loginsUsers.logins["isAdmin"] == false)
                return;
            if (loginsUsers.logins["isAdmin"] == null)
            {
                context.Result = new RedirectToRouteResult
           (new RouteValueDictionary(new
           {
               action = "Index",
               controller = "Login"
           }));
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //if (loginsUsers.logins["isAdmin"] == true || loginsUsers.logins["isAdmin"] == false)
            //    return;
            //if (loginsUsers.logins["isAdmin"] == null)
            //{
            //    context.Result = new RedirectToRouteResult
            //       (new RouteValueDictionary(new
            //       {
            //           action = "Index",
            //           controller = "Login"
            //       }));
            //}
        }
    }
}
