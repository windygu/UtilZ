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
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = (ControllerBase)context.Controller;
            if (controller is AuthenticationController)
            {
                //登录，不作验证
            }
            else
            {
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
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
