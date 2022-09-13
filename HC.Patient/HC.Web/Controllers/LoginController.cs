using HC.Common;
using HC.Common.Options;
using HC.Model;
using HC.Patient.Model;
using HC.Patient.Model.SecurityQuestion;
using HC.Patient.Service.IServices.Login;
using HC.Patient.Service.Token.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using HC.Patient.Service.IServices.User;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("Login")]
    public class LoginController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly ILoginService _loginService;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IUserService _usrService;
        public LoginController(ITokenService tokenService, ILoginService loginService, IOptions<JwtIssuerOptions> jwtOptions, IUserService usrService)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
            _tokenService = tokenService;
            _loginService = loginService;
            _usrService = usrService;
        }

        [HttpPost("login")]
        public JsonResult login([FromBody]ApplicationUser applicationUser)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_loginService.Login(applicationUser, _jwtOptions, token));            
        }

        [HttpPost("PatientLogin")]
        public JsonResult PatientLogin([FromBody]ApplicationUser applicationUser)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_loginService.PatientLogin(applicationUser, _jwtOptions, token));
        }
        [HttpPost("mobilelogin")]
        public JsonResult mobilelogin([FromBody] ApplicationUser applicationUser)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_loginService.MobileLogin(applicationUser, _jwtOptions, token));
        }
        [HttpPost("mobilelogout")]
        public JsonResult MobileLogout()
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_loginService.MobileLogout(token));
        }

        /// <summary>
        /// save question's answer for users
        /// </summary>
        /// <param name="securityQuestionListModel"></param>
        /// <returns></returns>
        [HttpPost("SaveUserSecurityQuestion")]
        public JsonResult SaveUserSecurityQuestion([FromBody]SecurityQuestionListModel securityQuestionListModel)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_loginService.SaveUserSecurityQuestion(securityQuestionListModel, _jwtOptions, token));
        }

        [HttpGet]
        [Route("SetUserOffline")]
        public JsonResult SetUserOffline()
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_usrService.Logout(token));
        }

        /// <summary>
        /// compare answer of the question if user try to login with new machine
        /// </summary>
        /// <param name="securityQuestionModel"></param>
        /// <returns></returns>
        [HttpPost("CheckQuestionAnswer")]
        public JsonResult CheckQuestionAnswer([FromBody]SecurityQuestionModel securityQuestionModel)
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_loginService.CheckQuestionAnswer(securityQuestionModel, _jwtOptions, token));
        }


        /// <summary>
        /// this will set user offline
        /// </summary>
        /// <returns></returns>
        [HttpPatch("Logout")]
        public JsonResult Logout()
        {
            TokenModel token = CommonMethods.GetTokenDataModel(HttpContext);
            return Json(_usrService.Logout(token));
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
    }
}