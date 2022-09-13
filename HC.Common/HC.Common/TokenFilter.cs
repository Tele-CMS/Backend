using Microsoft.AspNetCore.Mvc.Filters;

namespace HC.Common
{
    public class TokenFilterAttribute : ActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext actionContext)
        //{
        //    StringValues authorizationToken;
        //    var authHeader = actionContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
        //    var authToken = authorizationToken.ToString().Replace("Bearer", "").Trim();

        //    var encryptData = CommonMethods.GetDataFromToken(authToken);
        //    int userID = 0;
        //    if (encryptData != null && encryptData.Claims != null)
        //    {
        //        if (encryptData.Claims.Count > 0)
        //        {
        //            userID = Convert.ToInt32(encryptData.Claims[0].Value);
        //            var actionParams = actionContext.ActionArguments;
        //            try
        //            {
        //                if (((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)actionContext.ActionDescriptor).ActionName == "PostAsync")
        //                {
        //                    ((dynamic)actionParams["entity"]).CreatedBy = userID;
        //                    ((dynamic)actionParams["entity"]).CreatedDate = DateTime.UtcNow;

        //                }
        //                else if (((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)actionContext.ActionDescriptor).ActionName == "PatchAsync")
        //                {
        //                    ((dynamic)actionParams["entity"]).UpdatedBy = userID;
        //                    ((dynamic)actionParams["entity"]).UpdatedDate = DateTime.UtcNow;
        //                }
        //                else if (((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)actionContext.ActionDescriptor).ActionName == "DeleteAsync")
        //                {
        //                    ((dynamic)actionParams["entity"]).DeletedBy = userID;
        //                    ((dynamic)actionParams["entity"]).DeletedDate = DateTime.UtcNow;
        //                }
        //            }
        //            catch (Exception)
        //            {

   
        //            }
                    
        //            base.OnActionExecuting(actionContext);
        //        }
        //    }
        //}
    }
}
