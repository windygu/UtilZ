using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilZ.ParaService.WebApp.Models;

namespace UtilZ.ParaService.WebApp.Controllers._1_0
{
    public class ActionAuthenticationFilter : IActionFilter
    {
        /*
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!this.IsFilter(context))
            {
                return;
            }

            var controller = (ControllerBase)context.Controller;
            var token = controller.Request.Headers[WebAppConstant.AccessToken];
            var userInfo = AuthenticationController.GetUserInfo(token);
            if (userInfo == null)
            {
                context.Result = controller.Unauthorized();
                context.Canceled = true;
            }

            if (!context.ModelState.IsValid)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        sb.Append(error.ErrorMessage);
                        sb.Append("|");
                    }
                }

                if (sb.Length > 0)
                {
                    sb = sb.Remove(sb.Length - 1, 1);
                }

                context.Result = new ActionResult<string>(sb.ToString()).Result;
                context.Canceled = true;
            }
        }

        private bool IsFilter(ActionExecutedContext context)
        {
            var controller = (ControllerBase)context.Controller;
            if (controller is AuthenticationController)
            {
                //登录，不作验证
                return false;
            }

            if (controller is ParaValueController && string.Equals(context.ActionDescriptor.RouteValues["action"], "get", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }*/

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!context.ModelState.IsValid)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        sb.Append(error.ErrorMessage);
                        sb.Append("|");
                    }
                }

                if (sb.Length > 0)
                {
                    sb = sb.Remove(sb.Length - 1, 1);
                }

                context.Result = new ActionResult<string>(sb.ToString()).Result;
                context.Canceled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!this.IsFilter(context))
            {
                return;
            }

            var controller = (ControllerBase)context.Controller;
            var token = controller.Request.Headers[WebAppConstant.AccessToken];
            var userInfo = AuthenticationController.GetUserInfo(token);
            if (userInfo == null)
            {
                context.Result = controller.Unauthorized();
            }


            //var controller = (ControllerBase)context.Controller;
            //context.Result = controller.Unauthorized();
            //context.Result = new EmptyResult();
        }

        private bool IsFilter(ActionExecutingContext context)
        {
            var controller = (ControllerBase)context.Controller;
            if (controller is AuthenticationController)
            {
                //登录，不作验证
                return false;
            }

            if (controller is ParaValueController && string.Equals(context.ActionDescriptor.RouteValues["action"], "get", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
