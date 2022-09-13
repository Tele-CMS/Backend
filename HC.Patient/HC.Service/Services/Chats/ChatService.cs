using HC.Common;
using HC.Common.HC.Common;
using HC.Model;
using HC.Patient.Entity;
using HC.Patient.Model.Chat;
using HC.Patient.Repositories.IRepositories.Chats;
using HC.Patient.Service.IServices.Chats;
using HC.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HC.Patient.Service.IServices;
using HC.Patient.Repositories.IRepositories.User;
using HC.Patient.Service.IServices.User;
using static HC.Common.Enums.CommonEnum;
using HC.Patient.Model.Users;
using HC.Patient.Model.Patient;
using HC.Patient.Service.IServices.Patient;
using HC.Patient.Model;
using HC.Patient.Repositories.IRepositories.Staff;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices.MasterData;
using System.IO;
using Microsoft.AspNetCore.Http;
using HC.Common.Enums;
using HC.Patient.Model.Common;
using HC.Patient.Service.Services.Notification;
using HC.Patient.Repositories.IRepositories.Patient;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Data;

namespace HC.Patient.Service.Services.Chats
{
    public class ChatService : BaseService, IChatService
    {
        private JsonModel response;
        private readonly IChatRepository _chatRepository;
        private readonly IChatConnectedUserRepository _chatConnectedUserRepository;
        private readonly IChatRoomService _chatRoomService;
        private readonly IChatRoomUserService _chatRoomUserService;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleService _userRoleService;
        private readonly IStaffService _staffService;
        private readonly IPatientService _patientService;
        private readonly IStaffRepository _staffRepository;
        private readonly ILocationService _locationService;
        private readonly IPatientRepository _patientRepository;
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly HCOrganizationContext _context;

        public ChatService(IChatRepository chatRepository,
            IChatConnectedUserRepository chatConnectedUserRepository,
            IChatRoomService chatRoomService,
            IChatRoomUserService chatRoomUserService,
            IUserRepository userRepository,
            IUserRoleService userRoleService,
            IStaffService staffService,
            IPatientService patientService,
            IStaffRepository staffRepository,
            ILocationService locationService,
            IPatientRepository patientRepository,
            IChatRoomRepository chatRoomRepository,
            HCOrganizationContext context)
        {
            response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
            _chatRepository = chatRepository;
            _chatConnectedUserRepository = chatConnectedUserRepository;
            _chatRoomService = chatRoomService;
            _chatRoomUserService = chatRoomUserService;
            _userRepository = userRepository;
            _userRoleService = userRoleService;
            _staffService = staffService;
            _patientService = patientService;
            _staffRepository = staffRepository;
            _locationService = locationService;
            _patientRepository = patientRepository;
            _chatRoomRepository = chatRoomRepository;
            _context = context;
        }

        public async Task<JsonModel> ChatConnectedUser(ChatConnectedUserModel chatConnectedUserModel, TokenModel tokenModel)
        {
            ChatConnectedUser chatConnectedUser = _chatConnectedUserRepository.Get(a => a.UserId == chatConnectedUserModel.UserId);
            if (chatConnectedUser != null)
            {
                //return new JsonModel(null, StatusMessage.ChatConnectedAlreadyEstablished, (int)HttpStatusCode.Created);
                chatConnectedUser.ConnectionId = chatConnectedUserModel.ConnectionId;
                chatConnectedUser.UpdatedDate = DateTime.UtcNow;
                _chatConnectedUserRepository.Update(chatConnectedUser);
            }
            else
            {
                chatConnectedUser = new ChatConnectedUser
                {
                    ConnectionId = chatConnectedUserModel.ConnectionId,
                    UserId = chatConnectedUserModel.UserId,
                    CreatedDate = DateTime.UtcNow
                };
                _chatConnectedUserRepository.Create(chatConnectedUser);
            }

            await _chatConnectedUserRepository.SaveChangesAsync();
            return new JsonModel(null, StatusMessage.ChatConnectedEstablished, (int)HttpStatusCode.OK);
        }
        public JsonModel SaveChat(ChatModel chatModel, TokenModel tokenModel)
        {
            Chat chat = null;
            if (chatModel.Id == 0)
            {
                chat = new Chat();
                chatModel.Message = CommonMethods.Encrypt(chatModel.Message);

                AutoMapper.Mapper.Map(chatModel, chat);
                chat.OrganizationID = tokenModel.OrganizationID;
                chat.RoomId = chatModel.RoomId;
                chat.CreatedBy = chatModel.FromUserId;
                chat.CreatedDate = DateTime.UtcNow;
                chat.IsDeleted = false;
                chat.IsActive = true;
                chat.ChatDate = DateTime.UtcNow;
                chat.ToUserId = null;
                chat.FileName = chatModel.FileName;
                chat.FileType = chatModel.FileType;
                chat.MessageType = chatModel.MessageType;
                // _chatRepository.Create(chat);
                var result = _chatRepository.SaveChat(chat);  //_chatRepository.SaveChanges();
                if (result != null)
                    response = new JsonModel(null, StatusMessage.MessageSent, (int)HttpStatusCode.OK);
                else
                    response = new JsonModel(null, StatusMessage.MessageNotSent, (int)HttpStatusCode.InternalServerError);
            }
            return response;
        }
        public JsonModel GetChatHistory(ChatParmModel chatParmModel, TokenModel tokenModel)
        {
            List<ChatModel> chatModels = _chatRepository.GetChatHistory<ChatModel>(chatParmModel, tokenModel).ToList();

            if (chatModels != null && chatModels.Count() > 0)
            {
                int locationId = 0;
                Entity.User user = _userRepository.Get(a => a.Id == chatParmModel.FromUserId);

                if (user != null)
                {
                    string role = string.Empty;
                    JsonModel userRole = _userRoleService.GetRoleById(user.RoleID, tokenModel);
                    if (userRole.StatusCode == (int)HttpStatusCodes.OK)
                    {
                        UserRoleModel userRoleModel = (UserRoleModel)userRole.data;
                        role = userRoleModel.UserType;
                    }

                    //var userRole=role                    
                    if (role.ToUpper() == UserTypeEnum.CLIENT.ToString().ToUpper())
                    {
                        var response = _patientService.GetPatientIdByUserId(chatParmModel.FromUserId, tokenModel);
                        var patientId = 0;
                        if (response.StatusCode == (int)HttpStatusCodes.OK)
                            patientId = (int)response.data;
                        else
                            return new JsonModel(null, StatusMessage.AccountNotFound, (int)HttpStatusCodes.NotFound);
                        var patientResponse = _patientService.GetPatientById(patientId, tokenModel);
                        PatientDemographicsModel patient;
                        if (patientResponse.StatusCode == (int)HttpStatusCodes.OK)
                            patient = (PatientDemographicsModel)patientResponse.data;
                        else
                            patient = null;
                        if (patient == null)
                            return new JsonModel(null, StatusMessage.AccountNotFound, (int)HttpStatusCodes.NotFound);
                        //if (patient.IsActive == false)
                        //return new JsonModel(null, StatusMessage.AccountDeactivated, (int)HttpStatusCodes.Unauthorized);
                        locationId = patient.LocationID;
                    }
                    else if (role.ToUpper() == UserTypeEnum.PROVIDER.ToString().ToUpper() || role.ToUpper() == UserTypeEnum.STAFF.ToString().ToUpper() || role.ToUpper() == UserTypeEnum.ADMIN.ToString().ToUpper())
                    {
                        var staff = _staffService.GetStaffByUserId(user.Id, tokenModel);
                        if (staff == null)
                            return new JsonModel(null, StatusMessage.StaffInfoNotFound, (int)HttpStatusCodes.NotFound);
                        List<AssignedLocationsModel> staffLocations = _staffRepository.GetAssignedLocationsByStaffId(staff.Id, tokenModel).ToList();
                        int.TryParse(CommonMethods.Decrypt(staffLocations.Where(l => l.IsDefault == true).FirstOrDefault().LocationId.Replace(" ", "+")), out locationId);

                    }

                }
                if (locationId > 0)
                {
                    LocationModel locationModal = _locationService.GetLocationOffsets(locationId, tokenModel);
                    chatModels.ForEach(a =>
                    {
                        if (a.MessageType == (int)Common.Enums.CommonEnum.MessageType.Text)
                            a.Message = CommonMethods.Decrypt(a.Message);
                        else
                            a.Message = CommonMethods.CreateImageUrl(tokenModel.Request, "\\", CommonMethods.Decrypt(a.Message));
                        a.FileName = CommonMethods.Decrypt(a.FileName);
                        a.FileType = CommonMethods.Decrypt(a.FileType);
                        a.ChatDate = Common.CommonMethods.ConvertFromUtcTimeWithOffset(a.ChatDate, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, tokenModel);
                    });
                }
                else
                {
                    //LocationModel locationModal = _locationService.GetLocationOffsets(locationId, tokenModel);
                    chatModels.ForEach(a =>
                    {
                        a.Message = CommonMethods.Decrypt(a.Message);
                        a.FileName = CommonMethods.Decrypt(a.FileName);
                        a.FileType = CommonMethods.Decrypt(a.FileType);
                        a.ChatDate = Common.CommonMethods.ConvertFromUtcTimeWithOffset(a.ChatDate, tokenModel.OffSet, 0, tokenModel.Timezone, tokenModel);
                    });
                }
                response = new JsonModel(chatModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(chatModels, chatModels);
            }
            return response;
        }
        public string GetConnectionId(int UserID)
        {
            ChatConnectedUser _chatConnectedUser = _chatConnectedUserRepository.Get(a => a.UserId == UserID);
            if (_chatConnectedUser != null)
                return _chatConnectedUser.ConnectionId;

            return null;
        }
        public bool SignOutChat(int UserID)
        {
            bool isConnected = true;
            ChatConnectedUser chatConnectedUser = _chatConnectedUserRepository.Get(a => a.UserId == UserID);
            if (chatConnectedUser != null)
            {
                chatConnectedUser.ConnectionId = string.Empty;
                chatConnectedUser.UpdatedDate = DateTime.UtcNow;
                _chatConnectedUserRepository.Update(chatConnectedUser);
                _chatConnectedUserRepository.SaveChanges();
                isConnected = false;
            }
            return isConnected;
        }
        public JsonModel GetChatRoomId(int room, int userId, TokenModel tokenModel)
        {
            int roomId = 0;
            ChatRoomModel chatRoomModel = new ChatRoomModel()
            {
                RoomName = "App-" + room.ToString(),
                OranizationId = tokenModel.OrganizationID,
                UserId = userId
            };
            var roomResult = _chatRoomService.SaveChatRoomAsync(chatRoomModel, tokenModel);
            if (roomResult.StatusCode == (int)HttpStatusCode.Created || roomResult.StatusCode == (int)HttpStatusCode.OK)
            {
                var roomData = (ChatRoom)roomResult.data;
                ChatRoomUserModel chatRoomUserModel = new ChatRoomUserModel()
                {
                    RoomId = roomData.Id,
                    UserId = userId
                };
                var userInRoomResult = _chatRoomUserService.SaveChatRoomUser(chatRoomUserModel, tokenModel);
                if (userInRoomResult.StatusCode == (int)HttpStatusCode.Created || userInRoomResult.StatusCode == (int)HttpStatusCode.OK)
                {
                    var roomUserData = (ChatRoomUser)userInRoomResult.data;
                    roomId = roomUserData.RoomId;

                }
            }
            if (roomId > 0)
                response = new JsonModel(roomId, StatusMessage.ChatRoomCreated, (int)HttpStatusCode.OK);
            else
                response = new JsonModel(0, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
            return response;
        }
        public async Task<JsonModel> SendFileInChat(IFormFileCollection files, int roomId, int userId, TokenModel tokenModel)
        {
            if (files == null || files.Count == 0)
                return new JsonModel(0, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);

            var folderName = Path.Combine("Chat", "Files");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            List<ChatFileResponseModel> ChatFileResponseModels = new List<ChatFileResponseModel>();
            var fileIndex = 1;
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName);
                string fileName = DateTime.UtcNow.ToString("yyyyMMddTHHmmss");
                var uniqueFileName = $"{fileIndex}_{roomId}_{userId}_{fileName}{extension}";
                var dbPath = Path.Combine(folderName, uniqueFileName);

                using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
                {
                    ChatFileResponseModels.Add(
                     new ChatFileResponseModel()
                     {
                         FileName = file.FileName,
                         Message = dbPath
                     });
                    await file.CopyToAsync(fileStream);
                }
                fileIndex++;
            }
            if (ChatFileResponseModels.Count > 0)
            {
                var fileSaved = 0;
                foreach (var chatFile in ChatFileResponseModels)
                {
                    ChatModel chatModel = new ChatModel()
                    {
                        ChatDate = DateTime.UtcNow,
                        FromUserId = userId,
                        RoomId = roomId,
                        IsSeen = false,
                        Message = chatFile.Message,
                        FileName = CommonMethods.Encrypt(chatFile.FileName),
                        FileType = CommonMethods.Encrypt(Path.GetExtension(chatFile.Message)),
                        MessageType = (int)Common.Enums.CommonEnum.MessageType.File
                    };
                    var result = SaveChat(chatModel, tokenModel);
                    if (result.StatusCode == (int)HttpStatusCode.OK)
                        fileSaved++;
                }
                if (fileSaved > 0)
                {
                    ChatFileResponseModels.ForEach(x =>
                    {
                        x.Message = CommonMethods.CreateImageUrl(tokenModel.Request, "\\", x.Message);
                        x.FileType = Path.GetExtension(x.Message);
                        x.MessageType = (int)Common.Enums.CommonEnum.MessageType.File;
                    });
                    return new JsonModel(ChatFileResponseModels, string.Format(StatusMessage.ChatFileSaved, fileSaved, files.Count), (int)HttpStatusCode.OK);
                }
                else
                    return new JsonModel(ChatFileResponseModels, StatusMessage.ChatFileNotSaved, (int)HttpStatusCode.InternalServerError);
            }
            else
                return new JsonModel(ChatFileResponseModels, StatusMessage.ChatFileNotSaved, (int)HttpStatusCode.InternalServerError);
        }
        public void SendChatPushNotification(PushNotificationChatMessageModel pushNotificationChatMessage, string message, TokenModel token)
        {
            string image = string.Empty;

            Staffs staffs = _staffRepository.GetAll(x => x.UserID == pushNotificationChatMessage.ToUserId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
            if (staffs != null && staffs.FirstName != null)
            {
                image = string.IsNullOrEmpty(staffs.PhotoThumbnailPath) ? staffs.PhotoThumbnailPath : CommonMethods.CreateImageUrl(token.Request, ImagesPath.StaffThumbPhotos, staffs.PhotoThumbnailPath);
            }
            else
            {
                Patients patient = _patientRepository.GetAll(x => x.UserID == pushNotificationChatMessage.ToUserId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                image = string.IsNullOrEmpty(patient.PhotoThumbnailPath) ? patient.PhotoThumbnailPath : CommonMethods.CreateImageUrl(token.Request, ImagesPath.PatientThumbPhotos, patient.PhotoThumbnailPath);
            }

            pushNotificationChatMessage.Image = image;
            var chatRoom = _chatRoomRepository.GetChatRoomNameById(pushNotificationChatMessage.RoomId);
            var replaced = chatRoom.Replace("App-", "");
            pushNotificationChatMessage.AppointmentId = Convert.ToInt32(replaced);
            string deviceToken = _userRepository.GetByID(pushNotificationChatMessage.ToUserId).DeviceToken;
            if (!string.IsNullOrWhiteSpace(deviceToken))
            {
                PushMobileNotificationModel pushMobileNotification = new PushMobileNotificationModel();
                pushMobileNotification.DeviceToken = deviceToken;
                pushMobileNotification.Message = message;
                pushMobileNotification.NotificationPriority = PushNotificationPriority.High;
                pushMobileNotification.NotificationType = CommonEnum.NotificationActionType.ChatMessage.ToString();
                pushMobileNotification.Data = pushNotificationChatMessage;
                PushNotificationsService.SendPushNotificationForMobile(pushMobileNotification);
            }
        }

        public async Task<JsonModel> SaveCareChat(CareChatModel chatModel, TokenModel tokenModel)
        {
            CareChat chat = null;
            if (chatModel.Id == 0)
            {
                chat = new CareChat();
                chatModel.Message = CommonMethods.Encrypt(chatModel.Message);

                AutoMapper.Mapper.Map(chatModel, chat);
                chat.OrganizationID = tokenModel.OrganizationID;
                chat.CreatedBy = chatModel.FromUserId;
                chat.CreatedDate = DateTime.UtcNow;
                chat.IsDeleted = false;
                chat.IsActive = true;
                _context.CareChat.Add(chat);
                _context.SaveChanges();
              //  _chatRepository.Create(chat);
             //   await _chatRepository.SaveChangesAsync();
                AutoMapper.Mapper.Map(chat, chatModel);
                chatModel.Message = CommonMethods.Decrypt(chatModel.Message);
                response = new JsonModel(chatModel, "Message Saved", (int)HttpStatusCode.OK);
            }
            return response;
        }


        public JsonModel GetCareChatHistory(ChatParmModel chatParmModel, TokenModel tokenModel)
        {
            //List<Chat> chats = _chatRepository.GetAll(c => c.IsSeen == false
            //&& c.IsActive == true
            //&& c.IsDeleted == false
            //&& (
            //    (c.FromUserId == chatParmModel.FromUserId && c.ToUserId == chatParmModel.ToUserId)
            //    || (c.FromUserId == chatParmModel.ToUserId && c.ToUserId == chatParmModel.FromUserId))
            //).ToList();

            //if (chats != null && chats.Count > 0)
            //{
            //    chats.ForEach(x =>
            //    {
            //        x.IsSeen = true;
            //    });
            //    _chatRepository.Update(chats.ToArray());
            //    _chatRepository.SaveChanges();
            //}

            List<CareChatModel> chatModels = _chatRepository.GetCareChatHistory<CareChatModel>(chatParmModel, tokenModel).ToList();
            if (chatModels != null && chatModels.Count() > 0)
            {
                LocationModel locationModal = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);
                chatModels.ForEach((x =>
                {
                    if (x.ChatDate != null && tokenModel.LocationID > 0)
                    {
                        var dateTime = x.ChatDate;
                        x.ChatDateForWebApp = CommonMethods.ConvertFromUtcTimeWithOffset((DateTime)dateTime, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, tokenModel);
                    }
                    x.Message = (x.Message.Contains("are ") ? x.Message : CommonMethods.Decrypt(x.Message));
                }));


                response = new JsonModel(chatModels, StatusMessage.FetchMessage, (int)HttpStatusCode.OK);
                response.meta = new Meta(chatModels, chatParmModel);
            }
            return response;
        }
    }
}
