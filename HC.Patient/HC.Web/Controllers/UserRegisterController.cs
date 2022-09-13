using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Model;
using HC.Patient.Service.IServices;
using HC.Patient.Service.MasterData.Interfaces;
using HC.Patient.Service.Token.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Web.Controllers
{
    [Produces("application/json")]
    [Route("UserRegister")]
    public class UserRegisterController : Controller
    {
        private readonly IUserInvitationReadService _userInvitationReadService;
        private readonly IUserRegistrationWriteService _userRegistrationWriteService;
        private readonly ITokenService _tokenService;
        private readonly IMasterDataService _masterDataService;
        public UserRegisterController(ITokenService tokenService,
            IUserInvitationReadService userInvitationReadService,
            IUserRegistrationWriteService userRegistrationWriteService,
            IMasterDataService masterDataService)
        {
            _userInvitationReadService = userInvitationReadService;
            _userRegistrationWriteService = userRegistrationWriteService;
            _tokenService = tokenService;
            _masterDataService = masterDataService;
        }

        [HttpGet]
        [Route("CheckTokenAccessibility")]
        public JsonResult CheckTokenAccessibility(string id)//, string BusinessName = "sdm")
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

            return Json(_userInvitationReadService.ExecuteFunctions(() => _userInvitationReadService.CheckTokenAccessibilty(token, id)));
        }
        [HttpPost]
        [Route("RegisterWithToken")]
        public JsonResult RegisterNewUser([FromBody]RegisterUserModel registerUserModel)
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
            registerUserModel.OrganizationId = tokenData.OrganizationId;

            return Json(_userRegistrationWriteService.ExecuteFunctions(() => _userRegistrationWriteService.RegisterNewUser(registerUserModel, token, HttpContext.Request)));
        }

        [HttpPost]
        [Route("Register")]
        public JsonResult Register([FromBody]RegisterUserModel registerUserModel)
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
            registerUserModel.OrganizationId = tokenData.OrganizationId;

            return Json(_userRegistrationWriteService.ExecuteFunctions(() => _userRegistrationWriteService.RegisterNewUserWithoutToken(registerUserModel, token, HttpContext.Request)));
        }

        [HttpPost]
        [Route("MasterDataByName")]
        public JsonResult MasterDataByName([FromBody]JObject masterDataNames)
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
            List<string> masterDataNamesList = new List<string>(Convert.ToString(masterDataNames["masterdata"]).Split(','));
            return Json(_masterDataService.ExecuteFunctions(() => _masterDataService.GetMasterDataByName(masterDataNamesList, token)));
        }

        [HttpGet]
        [Route("CheckUsernameExistance")]
        public JsonResult CheckUsernameExistance( string username)
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
            return Json(_userInvitationReadService.ExecuteFunctions(() => _userInvitationReadService.CheckUsernameExistance(token, username)));
        }

        [HttpPost]
        [Route("RejectInvitation")]
        public JsonResult RejectInvitation([FromBody]RejectInvitationModel rejectInvitationModel)
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

            return Json(_userRegistrationWriteService.ExecuteFunctions(() => _userRegistrationWriteService.RejectInvitation(rejectInvitationModel, token)));
        }
    }
}