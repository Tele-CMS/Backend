using HC.Common;
using HC.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HC.Patient.Web.Filters
{
    public class ActionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result != null)
            {
                TokenModel tokenModel = CommonMethods.GetTokenDataModel(context.HttpContext);
                if ((Object.ReferenceEquals((context.Result.GetType().Name), typeof(JsonModel).Name)))
                {
                    ((JsonModel)context.Result).access_token = tokenModel.AccessToken;
                }
                else if ((Object.ReferenceEquals((context.Result.GetType().Name), typeof(JsonResult).Name)))
                {   
                    if (Object.ReferenceEquals((((JsonResult)context.Result).Value).GetType().Name, typeof(JsonModel).Name))
                    {
                        ((JsonModel)(((JsonResult)context.Result).Value)).access_token = tokenModel.AccessToken;
                    }
                }
                else if ((Object.ReferenceEquals((context.Result.GetType().Name), typeof(ObjectResult).Name)))
                {
                    if (Object.ReferenceEquals((((ObjectResult)context.Result).Value).GetType().Name, typeof(JsonModel).Name))
                    {
                        ((JsonModel)(((ObjectResult)context.Result).Value)).access_token = tokenModel.AccessToken;
                    }
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
