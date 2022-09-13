using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HC.Common;
using HC.Model;
using HC.Patient.Service.Token.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HC.Patient.Web.Controllers
{
    [Authorize(Policy = "AuthorizedUser")]
    [Produces("application/json")]
    public class BaseController : Controller
    {
        
        [NonAction]
        public TokenModel GetToken(HttpContext httpContext)
        {
            return CommonMethods.GetTokenDataModel(httpContext);
        }
        [NonAction]
        public TokenModel GetBussinessToken(HttpContext httpContext,ITokenService tokenService)
        {
            StringValues timezone;
            StringValues ipAddress;
            //StringValues locationID;
            StringValues offSet;
            string bussinessToken = httpContext.Request.Headers["businessToken"].ToString();
            var bussinessName = CommonMethods.Decrypt(bussinessToken);
            httpContext.Request.Headers.TryGetValue("Timezone", out timezone);
            httpContext.Request.Headers.TryGetValue("IPAddress", out ipAddress);
            //httpContext.Request.Headers.TryGetValue("LocationID", out locationID);
            httpContext.Request.Headers.TryGetValue("Offset", out offSet);
            DomainToken domainToken = new DomainToken
            {
                BusinessToken = bussinessName
            };
            DomainToken tokenData = tokenService.GetDomain(domainToken);

            TokenModel token = new TokenModel
            {
                Request = httpContext,
                OrganizationID = tokenData.OrganizationId,
                Timezone = timezone,
                IPAddress = ipAddress,
                OffSet = Convert.ToInt32(offSet),
            };
            return token;
        }
    }
}