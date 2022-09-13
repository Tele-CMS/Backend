using HC.Common;
using HC.Common.HC.Common;
using HC.Common.Options;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Service.IServices.Organizations;
using HC.Patient.Service.Token.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly ILogger _logger;
        private readonly HCMasterContext _masterContext;
        private string DomainName = "4Xdn04rbUB78Gd2UDWMiUrw+LCJHi8S4yTHeyN54z8JaxQ0etkkhME8hOoImr1lt"; //its Merging db        
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IOrganizationService _organizationService;
        private readonly ITokenService _tokenService;

        public SuperAdminController(ILoggerFactory loggerFactory, HCMasterContext masterContext, 
            IOptions<JwtIssuerOptions> jwtOptions, IOrganizationService organizationService, ITokenService tokenService)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
            _logger = loggerFactory.CreateLogger<SuperAdminController>();
            _masterContext = masterContext;
            _organizationService = organizationService;
            _tokenService = tokenService;
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody]ApplicationUser applicationUser)
        {
            //check user exit in database or not
            var dbUser = GetUserByUserName(applicationUser.UserName);

            if (dbUser != null) //if user exist in database
            {
                // token model just IP
                TokenModel token = GetIPFromRequst();

                //Check Credentials
                var identity = GetSuperAdminClaimsIdentity(applicationUser, dbUser);

                //if credentials are wrong
                if (identity == null)
                {
                    _logger.LogInformation($"Invalid username ({applicationUser.UserName}) or password ({applicationUser.Password})");
                    Response.StatusCode = (int)HttpStatusCodes.Unauthorized;//(Invalid credentials)
                    
                    return Json(new
                    {
                        data = new object(),
                        Message = StatusMessage.InvalidUserOrPassword,
                        StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                    });
                }

                StringValues Host = string.Empty; HttpContext.Request.Headers.TryGetValue("BusinessToken", out Host);
                if (!string.IsNullOrEmpty(Host))
                {
                    DomainName = CommonMethods.Decrypt(Host.ToString());
                }

                var claims = new[]
               {
                    new Claim("UserID", dbUser.Id.ToString()),
                    new Claim("RoleID", 0.ToString()),                      // not required please don't chamge
                    new Claim("UserName", dbUser.UserName.ToString()),
                    new Claim("OrganizationID", 0.ToString()),              // not required please don't chamge
                    new Claim("StaffID", 0.ToString()),                     // not required please don't chamge
                    new Claim("LocationID", 0.ToString()),                  // not required please don't chamge
                    new Claim("DomainName",DomainName),                     // Domain name always add in token
                    new Claim(JwtRegisteredClaimNames.Sub, applicationUser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
                    new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                    identity.FindFirst("HealthCare")
            };

                // Create the JWT security token and encode it.
                var jwt = new JwtSecurityToken(
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    claims: claims,
                    notBefore: _jwtOptions.NotBefore,
                    expires: _jwtOptions.Expiration,
                    signingCredentials: _jwtOptions.SigningCredentials);
                
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);                
                dbUser.Password = null;                                
                var response = Json(new JsonModel
                {
                    access_token = encodedJwt,
                    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                    data = dbUser,
                });
                return response;
            }
            else
            {
                return Json(new
                {
                    data = new object(),
                    Message = StatusMessage.InvalidUserOrPassword,
                    StatusCode = (int)HttpStatusCodes.Unauthorized//(Invalid credentials)
                });
            }

        }

        [HttpPost("VerifyNPI")]
        public IActionResult VerifyNPI([FromBody]ApplicationUser applicationUser)
        {
            var bussinessName = CommonMethods.Decrypt(HttpContext.Request.Headers["businessToken"].ToString());
            DomainToken domainToken = new DomainToken
            {
                BusinessToken = bussinessName
            };
            DomainToken tokenData = _tokenService.GetDomain(domainToken);

            TokenModel token = new TokenModel
            {
                Request = HttpContext,
                OrganizationID = tokenData.OrganizationId
            };
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = HttpContext;

            return Json(_organizationService.ExecuteFunctions(() => _organizationService.VerifyNPI(applicationUser, token, httpContextAccessor)));
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        private SuperUser GetUserByUserName(string userName)
        {
            try
            {
                return _masterContext.SuperUser.Where(m => m.UserName.ToUpper() == userName.ToUpper()).FirstOrDefault();
            }
            catch (Exception )
            {
                return null;
            }
        }

        /// <summary>
        /// IMAGINE BIG RED WARNING SIGNS HERE!
        /// You'd want to retrieve claims through your claims provider
        /// in whatever way suits you, the below is purely for demo purposes!
        /// </summary>
        private static ClaimsIdentity GetSuperAdminClaimsIdentity(ApplicationUser user, SuperUser dbUser)
        {
            if (dbUser != null && (user.UserName.ToUpper() == dbUser.UserName.ToUpper() && user.Password == CommonMethods.Decrypt(dbUser.Password)))
            {
                return new ClaimsIdentity(new GenericIdentity(user.UserName, "Token"),
                  new[]
                  {
                   new Claim("HealthCare", "IAmAuthorized")
                  });
            }
            else
            {
                return null;
            }
        }

        private TokenModel GetIPFromRequst()
        {
            StringValues ipAddress;
            TokenModel token = new TokenModel();
            HttpContext.Request.Headers.TryGetValue("IPAddress", out ipAddress);
            if (!string.IsNullOrEmpty(ipAddress)) { token.IPAddress = ipAddress; } else { token.IPAddress = "203.129.220.76"; }
            return token;
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}