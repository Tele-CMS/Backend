using AutoMapper;
using HC.Common;
using HC.Common.HC.Common;
using HC.Common.Services;
using HC.Model;
using HC.Patient.Model;
using HC.Patient.Model.Users;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Repositories.IRepositories.Locations;
using HC.Patient.Repositories.IRepositories.Organizations;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Service.IServices.User;
using HC.Patient.Service.Token.Interfaces;
using HC.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using static HC.Common.Enums.CommonEnum;

namespace HC.Patient.Service.Services
{
    public class UserRegistrationWriteService : BaseService, IUserRegistrationWriteService
    {
        private readonly IUserInvitationReadRepository _userInvitationReadRepository;
        private readonly IUserInvitationWriteRepository _userInvitationWriteRepository;
        private readonly IMapper _mapper;
        private JsonModel _response;
        private readonly IUserRegisterWriteRepository _userRegisterWriteRepository;
        public readonly IUserInvitationReadService _userInvitationReadService;
        private readonly ILocationRepository _locationRepository;
        private readonly IUserRoleService _userRoleService;
        private readonly IPatientService _patientService;
        private readonly IEmailService _emailSender;
        private readonly IHostingEnvironment _env;
        private readonly ITokenService _tokenService;
        private readonly IOrganizationSMTPRepository _organizationSMTPRepository;
        private readonly IEmailWriteService _emailWriteService;
        private readonly IStaffService _staffService;
        public UserRegistrationWriteService(IUserInvitationWriteRepository userInvitationWriteRepository,
            IUserInvitationReadRepository userInvitationReadRepository,
            IMapper mapper,
            IUserRegisterWriteRepository userRegisterWriteRepository,
            IUserInvitationReadService userInvitationReadService,
            ILocationRepository locationRepository,
            IUserRoleService userRoleService,
            IPatientService patientService,
            IEmailService emailSender,
            IHostingEnvironment env,
            ITokenService tokenService,
            IOrganizationSMTPRepository organizationSMTPRepository,
            IEmailWriteService emailWriteService,
           IStaffService staffService
            )
        {
            _userInvitationReadRepository = userInvitationReadRepository;
            _mapper = mapper;
            _userRegisterWriteRepository = userRegisterWriteRepository;
            _userInvitationReadService = userInvitationReadService;
            _locationRepository = locationRepository;
            _userInvitationWriteRepository = userInvitationWriteRepository;
            _userRoleService = userRoleService;
            _patientService = patientService;
            _emailSender = emailSender;
            _env = env;
            _tokenService = tokenService;
            _organizationSMTPRepository = organizationSMTPRepository;
            _emailWriteService = emailWriteService;
            _staffService = staffService;
        }
        public JsonModel RegisterNewUser(RegisterUserModel registerUser, TokenModel tokenModel, HttpRequest Request)
        {
            try
            {
                if (_userInvitationReadService.CheckTokenAccessibilty(tokenModel, registerUser.InvitationId).StatusCode != (int)HttpStatusCode.OK)
                    return new JsonModel(new object(), StatusMessage.InvitaionTokenNotValid, (int)HttpStatusCode.NotFound);

                int.TryParse(CommonMethods.Decrypt(registerUser.InvitationId), out int invitationId);
                int result = 0;
                if (registerUser != null)
                {
                    var invitationDetail = _userInvitationReadRepository.GetUserInvitationByIdAndOrganizationId(invitationId, tokenModel);
                    registerUser.RoleId = invitationDetail.RoleId;
                    registerUser.LocationId = invitationDetail.LocationId;
                    //map user entity from RegisterUserModel
                    var user = _mapper.Map<Entity.User>(registerUser);
                    //map staff entity from RegisterUserModel
                    var staff = _mapper.Map<Entity.Staffs>(registerUser);
                    //map location entity from RegisterUserModel
                    var location = _mapper.Map<Entity.StaffLocation>(registerUser);

                    result = _userRegisterWriteRepository.SaveNewUser(user, staff, location, invitationDetail);
                }
                if (result > 0)
                {
                    _response = new JsonModel(new object(), StatusMessage.UserRegistredSuccessfully, (int)HttpStatusCode.OK);
                }
                else
                    _response = new JsonModel(new object(), StatusMessage.UserRegistredNotSaved, (int)HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                throw e;
            }
            return _response;
        }

        public JsonModel RegisterNewUserWithoutToken(RegisterUserModel registerUser, TokenModel tokenModel, HttpRequest Request)
        {
            try
            {
                int result = 0;
                if (registerUser != null)
                {
                    string role = string.Empty;
                    JsonModel userRole = _userRoleService.GetRoleById(registerUser.RoleId, tokenModel);
                    if (userRole.StatusCode == (int)HttpStatusCodes.OK)
                    {
                        UserRoleModel userRoleModel = (UserRoleModel)userRole.data;
                        role = userRoleModel.UserType;
                    }
                    if (string.IsNullOrEmpty(role))
                        _response = new JsonModel(new object(), StatusMessage.SelectRole, (int)HttpStatusCode.NotFound);
                    if (role.ToUpper() == "STAFF" || role.ToUpper() == "PROVIDER")
                    {
                        var response = _staffService.GetStaffByEmail(registerUser.Email, tokenModel);
                        if (response.StatusCode == (int)HttpStatusCode.OK)
                            return new JsonModel(new object(), StatusMessage.EmailAddressExisted, (int)HttpStatusCode.Found);
                        //map user entity from RegisterUserModel
                        var user = _mapper.Map<Entity.User>(registerUser);
                        //map staff entity from RegisterUserModel
                        var staff = _mapper.Map<Entity.Staffs>(registerUser);
                        staff.PhoneNumber = registerUser.Phone;
                        //map location entity from RegisterUserModel
                        var location = _mapper.Map<Entity.StaffLocation>(registerUser);

                        result = _userRegisterWriteRepository.SaveNewUser(user, staff, location);
                    }
                    else//Save Client
                    {
                        var patientDemographicsModel = _mapper.Map<Model.Patient.PatientDemographicsModel>(registerUser);
                        //tokenModel.UserID = null;
                        patientDemographicsModel.IsPortalActivate = true;
                        patientDemographicsModel.IsActive = true;
                        patientDemographicsModel.NewPassword = CommonMethods.Encrypt(registerUser.Password);
                        return _patientService.CreateUpdatePatient(patientDemographicsModel, tokenModel);
                    }
                }
                if (result > 0)
                {
                    _response = new JsonModel(new object(), StatusMessage.UserRegistredSuccessfully, (int)HttpStatusCode.OK);
                }
                else
                    _response = new JsonModel(new object(), StatusMessage.UserRegistredNotSaved, (int)HttpStatusCode.InternalServerError);
            }
            catch (Exception e)
            {
                throw e;
            }
            return _response;
        }
        public JsonModel RejectInvitation(RejectInvitationModel rejectInvitationModel, TokenModel tokenModel)
        {
            try
            {
                var result = 0;
                if (_userInvitationReadService.CheckTokenAccessibilty(tokenModel, rejectInvitationModel.InvitationId).StatusCode != (int)HttpStatusCode.OK)
                    return new JsonModel(new object(), StatusMessage.InvitaionTokenNotValid, (int)HttpStatusCode.NotFound);

                int.TryParse(CommonMethods.Decrypt(rejectInvitationModel.InvitationId.Replace(" ", "+")), out int invitationId);
                if (rejectInvitationModel != null)
                {
                    var invitationDetail = _userInvitationReadRepository.GetUserInvitationByIdAndOrganizationId(invitationId, tokenModel);
                    //map InvitationRejectLog entity from RejectInvitationModel
                    var rejectInvitaion = _mapper.Map<Entity.InvitationRejectLog>(rejectInvitationModel);
                    if (invitationDetail.InvitationStatus == (int)Common.Enums.CommonEnum.UserInvitationStatus.Pending || invitationDetail.InvitationStatus == (int)Common.Enums.CommonEnum.UserInvitationStatus.ReInvited)
                    {
                        Entity.InvitationRejectLog invitationRejectLog = _userRegisterWriteRepository.RejectInvitation(rejectInvitaion);
                        if (invitationRejectLog != null)
                        {
                            //invitationDetail.UpdatedBy = tokenModel.UserID;
                            invitationDetail.UpdatedDate = DateTime.UtcNow;
                            invitationDetail.InvitationStatus = (int)Common.Enums.CommonEnum.UserInvitationStatus.Rejected;
                            result = _userInvitationWriteRepository.SaveUpdateUserInvitation(invitationDetail);
                            if (result > 0)
                                _response = new JsonModel(new object(), StatusMessage.InvitationRejectedSuccessfully, (int)HttpStatusCode.OK);
                            else
                                _response = new JsonModel(new object(), StatusMessage.InvitationNotRejected, (int)HttpStatusCode.InternalServerError);
                        }
                        else
                            _response = new JsonModel(new object(), StatusMessage.InvitationNotRejected, (int)HttpStatusCode.InternalServerError);

                    }
                    else
                        _response = new JsonModel(new object(), StatusMessage.AlreadyRejectedInvitation, (int)HttpStatusCode.Created);
                }
                else
                    _response = new JsonModel(new object(), StatusMessage.InvitationNotRejected, (int)HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                throw e;
            }
            return _response;
        }

    }
}
