using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Filters
{
    public class AuthorizeHandler : AuthorizationHandler<AuthorizationFilter>
    {
        private readonly IHttpContextAccessor contextAccessor;        
        JsonModel response = new JsonModel(new object(), StatusMessage.TokenExpired, (int)HttpStatusCodes.Unauthorized);
        public AuthorizeHandler(IHttpContextAccessor contextAccessor)
        {        
            this.contextAccessor = contextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationFilter requirement)
        {
#if !DEBUG
            StringValues authorizationToken;
            StringValues businessToken;
            var authHeader = contextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out authorizationToken);
            var businessTokenHeader = contextAccessor.HttpContext.Request.Headers.TryGetValue("BusinessToken", out businessToken);
            var authToken = authorizationToken.ToString().Replace("Bearer", "").Trim();

            var encryptData = CommonMethods.GetDataFromToken(authToken);
            if ((encryptData != null && encryptData.Claims != null) || (businessTokenHeader == true))
            {
                //if (encryptData.ValidTo < DateTime.UtcNow) //check if login user's token expire then its unauthorized request
                //{
                //    context.Fail();
                //    throw new AuthenticationException(StatusMessage.TokenExpired);
                //}
                context.Succeed(requirement);
            }
            else
                context.Fail();
#else
            context.Succeed(requirement);
#endif
            return Task.CompletedTask;
        }
    }
}
