using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Service.IServices;
using HC.Patient.Service.IServices.MasterData;
using HC.Service;
using System.Net;
using AutoMapper;
using HC.Patient.Repositories.IRepositories.Staff;

namespace HC.Patient.Service.Services
{
    public class UserInvitationReadService : BaseService, IUserInvitationReadService
    {
        private readonly IUserInvitationReadRepository _userInvitationReadRepository;
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;
        private JsonModel response;
        private readonly IStaffRepository _staffRepository;
        public UserInvitationReadService(IUserInvitationReadRepository userInvitationReadRepository,
            ILocationService locationService,
            IMapper mapper,
            IStaffRepository staffRepository)
        {
            _userInvitationReadRepository = userInvitationReadRepository;
            _locationService = locationService;
            _mapper = mapper;
            _staffRepository = staffRepository;
        }

        public JsonModel GetUserInvitationList(TokenModel tokenModel, UserInvitationFilterModel userInvitationFilterModel)
        {
            response = new JsonModel();
            var locationModal = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);
            var userInvitations = _userInvitationReadRepository.GetUserInvitationList(tokenModel, locationModal, userInvitationFilterModel);
            if (userInvitations != null && userInvitations.Count > 0)
            {
                response.data = userInvitations;
                response.Message = StatusMessage.FetchMessage;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.meta = new Meta(userInvitations, userInvitationFilterModel);
            }
            else
            {
                response.data = new object();
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return response;
        }
        public JsonModel GetUserInvitation(int invitationId, TokenModel tokenModel)
        {
            response = new JsonModel();
            var userInvitation = _userInvitationReadRepository.GetUserInvitationByIdAndOrganizationId(invitationId, tokenModel);
            if (userInvitation != null)
            {
                var invitaionResponse = _mapper.Map<UserInvitationResponseModel>(userInvitation);
                response.data = invitaionResponse;
                response.Message = StatusMessage.FetchMessage;
                response.StatusCode = (int)HttpStatusCode.OK;
            }
            else
            {
                response.data = new object();
                response.Message = StatusMessage.NotFound;
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            return response;
        }

        public JsonModel CheckTokenAccessibilty(TokenModel tokenModel, string invitationId)
        {
            int.TryParse(Common.CommonMethods.Decrypt(invitationId.Replace(" ", "+")), out int Id);

            if (Id <= 0)
                return new JsonModel(new object(), StatusMessage.NotFound, (int)HttpStatusCode.NotFound);

            //check whether token is still valid or not
            UserInvitation userInvitation = _userInvitationReadRepository.GetUserInvitationByIdAndOrganizationId(Id, tokenModel);

            if (userInvitation == null || (userInvitation.InvitationStatus == (int)Common.Enums.CommonEnum.UserInvitationStatus.Rejected || userInvitation.InvitationStatus == (int)Common.Enums.CommonEnum.UserInvitationStatus.Accepted))
                return new JsonModel(new object(), StatusMessage.InvitaionTokenNotValid, (int)HttpStatusCode.BadRequest);

            //check whether user already register with same email id or not
            Staffs staffs = _staffRepository.GetStaffProfileDataByEmailAndOrgId(userInvitation.Email, tokenModel);

            if (staffs != null)
                return new JsonModel(new object(), StatusMessage.InvitaionTokenAlreadyUsed, (int)HttpStatusCode.Created);

            UserInvitationRegistrationModel userInvitationRegistrationModel = _mapper.Map<UserInvitationRegistrationModel>(userInvitation);

            return new JsonModel(userInvitationRegistrationModel, StatusMessage.InvitaionTokenValid, (int)HttpStatusCode.OK);
        }

        public JsonModel CheckUsernameExistance(TokenModel tokenModel, string username)
        {
            Entity.User user = _userInvitationReadRepository.CheckUserNameExistance(username, tokenModel);

            if (user != null)
                return new JsonModel(new object(), StatusMessage.UsernameTaken, (int)HttpStatusCode.Found);
            return new JsonModel(new object(), StatusMessage.UsernameAvailable, (int)HttpStatusCode.OK);

        }
    }
}
