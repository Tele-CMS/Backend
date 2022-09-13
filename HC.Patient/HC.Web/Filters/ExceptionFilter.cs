using HC.Common.HC.Common;
using HC.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {   
            //            if (context.Exception is ApiException)
            //            {
            //                // handle explicit 'known' API errors
            //                var ex = context.Exception as ApiException;
            //                context.Exception = null;
            //                apiError = new ApiError(ex.Message);
            //                apiError.errors = ex.Errors;

            //                context.HttpContext.Response.StatusCode = ex.StatusCode;
            //            }
            //            else if (context.Exception is UnauthorizedAccessException)
            //            {
            //                apiError = new ApiError("Unauthorized Access");
            //                context.HttpContext.Response.StatusCode = 401;

            //                // handle logging here
            //            }
            //            else
            //            {
            //                // Unhandled errors
            //#if !DEBUG
            //                            var msg = "An unhandled error occurred.";                
            //                            string stack = null;
            //#else
            //                var msg = context.Exception.GetBaseException().Message;
            //                string stack = context.Exception.StackTrace;
            //#endif

            //                apiError = new ApiError(msg);
            //                apiError.detail = stack;

            //                context.HttpContext.Response.StatusCode = 500;

            //                // handle logging here
            //            }

            //            // always return a JSON result
            //            context.Result = new JsonResult(apiError);

            //            base.OnException(context);            
        }
    }
}
