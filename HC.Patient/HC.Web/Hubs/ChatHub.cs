using HC.Common;
using HC.Common.Options;
using HC.Model;
using HC.Patient.Model.Chat;
using HC.Patient.Model.Common;
using HC.Patient.Model.Message;
using HC.Patient.Service.IServices.Chats;
using HC.Patient.Service.IServices.Message;
using HC.Patient.Service.Token.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using HC.Patient.Service.IServices;
using HC.Patient.Repositories.IRepositories.Patient;
using System.Net;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using HC.Patient.Repositories.IRepositories;
using System.Linq;
using HC.Common.HC.Common;
using HC.Patient.Repositories.IRepositories.Telehealth;
using HC.Patient.Model.MasterData;
using HC.Patient.Service.IServices.MasterData;

namespace HC.Patient.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IMessageService _messageService;
        private readonly ITokenService _tokenService;
        //private readonly ILogger _logger;
        //private readonly JwtIssuerOptions _jwtOptions;
        //private readonly ITokenRepository _tokenRepository;
        private readonly INotificationService _notificationService;
        private readonly IPatientRepository _patientRepository;
        private readonly IChatRoomService _chatRoomService;
        private readonly IChatRoomUserService _chatRoomUserService;
        private readonly IChatRoomUserRepository _chatRoomUserRepository;
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly ITelehealthRepository _telehealthRepository;
        private readonly ILocationService _locationService;

        public ChatHub(IChatService chatService,
            IMessageService messageService,
            ITokenService tokenService,
             //ILoggerFactory loggerFactory,
             //IOptions<JwtIssuerOptions> jwtOptions,
             // ITokenRepository tokenRepository,
             INotificationService notificationService,
             IPatientRepository patientRepository,
             IChatRoomService chatRoomService,
             IChatRoomUserService chatRoomUserService,
             IChatRoomUserRepository chatRoomUserRepository,
             IChatRoomRepository chatRoomRepository,
             ITelehealthRepository telehealthRepository,
              ILocationService locationService
            )
        {
            _chatService = chatService;
            _messageService = messageService;
            _tokenService = tokenService;

            //_jwtOptions = jwtOptions.Value;
            //ThrowIfInvalidOptions(_jwtOptions);
            //_logger = loggerFactory.CreateLogger<ChatHub>();
            //_tokenRepository = tokenRepository;
            _notificationService = notificationService;
            _patientRepository = patientRepository;
            _chatRoomService = chatRoomService;
            _chatRoomUserService = chatRoomUserService;
            _chatRoomUserRepository = chatRoomUserRepository;
            _chatRoomRepository = chatRoomRepository;
            _telehealthRepository = telehealthRepository;
            _locationService = locationService;
        }
        public TokenModel GetBussinessToken(HttpContext httpContext, ITokenService tokenService)
        {
            var authHeader = httpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationToken);
            var authToken = authorizationToken.ToString().Replace("%3D", "=").Replace("Bearer", "").Trim();
            var bussinessName = CommonMethods.Decrypt(authToken);//CommonMethods.Decrypt(httpContext.Request.Headers["businessToken"].ToString());
            DomainToken domainToken = new DomainToken
            {
                BusinessToken = bussinessName
            };
            DomainToken tokenData = tokenService.GetDomain(domainToken);

            TokenModel token = new TokenModel
            {
                Request = httpContext,
                OrganizationID = tokenData.OrganizationId
            };
            return token;
        }

        public async Task<JsonModel> ConnectWithAccessToken()
        {
            try
            {


                TokenModel tokenModel = CommonMethods.GetTokenDataModel(Context.GetHttpContext());
                ChatConnectedUserModel chatConnectedUserModel = new ChatConnectedUserModel()
                {
                    UserId = tokenModel.UserID,
                    ConnectionId = Context.ConnectionId
                };

                var response = await _chatService.ChatConnectedUser(chatConnectedUserModel, tokenModel);
                if (response.StatusCode == (int)HttpStatusCode.OK)
                {
                    await Clients.All.SendAsync("UserConnected", chatConnectedUserModel.UserId);
                }
                return await _chatService.ChatConnectedUser(chatConnectedUserModel, tokenModel);

            }
            catch (Exception ex)
            {
                return new JsonModel(null, ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
        public async Task<JsonModel> ConnectWithBussinessToken(int userId)
        {
            try
            {


                TokenModel tokenModel = GetBussinessToken(Context.GetHttpContext(), _tokenService);
                ChatConnectedUserModel chatConnectedUserModel = new ChatConnectedUserModel()
                {
                    UserId = userId,
                    ConnectionId = Context.ConnectionId
                };
                await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
                var response = await _chatService.ChatConnectedUser(chatConnectedUserModel, tokenModel);
                if (response.StatusCode == (int)HttpStatusCode.OK || response.StatusCode == (int)HttpStatusCode.Created)
                {
                    await Clients.All.SendAsync("UserConnected", chatConnectedUserModel.UserId);
                    return response;
                }
                else
                    return new JsonModel(null, StatusMessage.ChatConnectedNotEstablished, (int)HttpStatusCode.BadRequest);

            }
            catch (Exception ex)
            {
                return new JsonModel(null, StatusMessage.InternalServerError, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<int> Connect(int userId, int room)
        {
            int roomId = 0;
         
            try
            {
                if (room > 0)
                {
                ChatConnectedUserModel chatConnectedUserModel = new ChatConnectedUserModel();
                TokenModel tokenModel = GetBussinessToken(Context.GetHttpContext(), _tokenService);
                chatConnectedUserModel.ConnectionId = Context.ConnectionId;
                chatConnectedUserModel.UserId = userId;
                //chatConnectedUserModel.UserId = tokenModel.UserID;

                await _chatService.ChatConnectedUser(chatConnectedUserModel, tokenModel);
                var chatRoom = _chatRoomRepository.GetChatRoomByName("App-" + room.ToString(), tokenModel);
                if (chatRoom != null)
                {
                    roomId = chatRoom.Id;
                    var connectionIds = _chatRoomUserRepository.GetConectionIdInParticularRoom<RoomConnectionModel>(roomId, userId, tokenModel);
                    if (connectionIds != null && connectionIds.ToList().Count > 0)
                    {
                        connectionIds.ToList().ForEach(async x =>
                        {
                            await Clients.Client(x.ConnectionId).SendAsync("UserConnected", x.UserId, roomId);
                        });

                        //await Clients.All.SendAsync("ReceiveMessage", chatFileResponseModels, UserId, currentRoomId);
                    }
                }
                // await Clients.All.SendAsync("UserConnected", userId, room);
                }
                else
                {
                    ChatConnectedUserModel chatConnectedUserModel = new ChatConnectedUserModel();
                    TokenModel tokenModel = CommonMethods.GetTokenDataModel(Context.GetHttpContext());
                    chatConnectedUserModel.ConnectionId = Context.ConnectionId;
                    chatConnectedUserModel.UserId = userId;
                    await _chatService.ChatConnectedUser(chatConnectedUserModel, tokenModel);
                }
               

            }
            catch (Exception ex)
            {
            }
            return roomId;
        }

      
        //public async Task Connect(int userId)
        //{
        //    try
        //    {
        //        ChatConnectedUserModel chatConnectedUserModel = new ChatConnectedUserModel();
        //        TokenModel tokenModel = CommonMethods.GetTokenDataModel(Context.GetHttpContext());
        //        chatConnectedUserModel.ConnectionId = Context.ConnectionId;
        //        //chatConnectedUserModel.UserId = userId;
        //        chatConnectedUserModel.UserId = tokenModel.UserID;
        //        await _chatService.ChatConnectedUser(chatConnectedUserModel, tokenModel);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        /// <summary>
        /// he SendMessage method can be called by any connected client.
        /// It sends the received message to "all clients".
        /// SignalR code is asynchronous to provide maximum scalability.
        /// </summary>        
        /// <param name="message"></param>        
        /// <param name="UserId"></param>
        /// <param name="currentRoomId"></param>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        public async Task SendMessage(string message, int UserId, int currentRoomId, int appointmentId)
        {
            try
            {
                ChatModel chatModel = new ChatModel();
                TokenModel tokenModel = GetBussinessToken(Context.GetHttpContext(), _tokenService);
                string connectionId = _chatService.GetConnectionId(UserId);
                chatModel.ChatDate = DateTime.UtcNow;
                chatModel.FromUserId = UserId;
                chatModel.RoomId = currentRoomId;
                chatModel.IsSeen = false;
                chatModel.Message = message;
                _chatService.SaveChat(chatModel, tokenModel);

                if (connectionId != null)
                {
                    List<ChatFileResponseModel> chatFileResponseModels = new List<ChatFileResponseModel>() {
                    new ChatFileResponseModel()
                    {
                        Message = message,
                        FileName = "",
                        FileType = "",
                        MessageType = (int)Common.Enums.CommonEnum.MessageType.Text
                    }
                    };
                    int ToUserId = 0;
                    var connectionIds = _chatRoomUserRepository.GetConectionIdInParticularRoom<RoomConnectionModel>(currentRoomId, UserId, tokenModel);
                    if (connectionIds != null && connectionIds.ToList().Count > 0)
                    {
                        ToUserId = connectionIds.Select(x => x.UserId).FirstOrDefault();
                        PushNotificationChatMessageModel model = new PushNotificationChatMessageModel();
                        model.FromUserId = chatModel.FromUserId;
                        model.ToUserId = ToUserId;
                        model.ChatDate = chatModel.ChatDate.ToString("yyyy-MM-ddTHH:mm:ss");
                        model.IsReceived = false;
                        model.RoomId = chatModel.RoomId;
                        model.StatusName = "Message";

                        _chatService.SendChatPushNotification(model, message, tokenModel);
                        connectionIds.ToList().ForEach(async x =>
                        {
                            await Clients.Client(x.ConnectionId).SendAsync("ReceiveMessage", chatFileResponseModels, UserId, currentRoomId, appointmentId);
                        });

                        //await Clients.All.SendAsync("ReceiveMessage", chatFileResponseModels, UserId, currentRoomId);
                    }
                }
                //else
                //    await Clients.All.SendAsync("ReceiveMessage", message, fromUserId);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<JsonModel> SendMessages(CareChatModel chatModel)
        {
            JsonModel jsonModel = new JsonModel();
            TokenModel tokenModel = new TokenModel();
            if (chatModel.ChatType == 1)
            {
                 tokenModel = CommonMethods.GetTokenDataModel(Context.GetHttpContext());
            }
            else
            {
                 tokenModel = GetBussinessToken(Context.GetHttpContext(), _tokenService);

            }

            try
            {
                chatModel.ChatDate = DateTime.UtcNow;
                chatModel.IsSeen = false;
                chatModel.Id = chatModel.Id != null ? chatModel.Id : 0;
                 jsonModel = await _chatService.SaveCareChat(chatModel, tokenModel);
                //    SaveNotification(chatModel, (int)NotificationActionType.ChatMessage, tokenModel);

                  LocationModel locationModal = _locationService.GetLocationOffsets(tokenModel.LocationID, tokenModel);

                chatModel.UTCDateTimeForMobile = chatModel.ChatDate;
                chatModel.ChatDateForWebApp = chatModel.ChatDate;
                //chatModel.ChatDateForWebApp = CommonMethods.ConvertFromUtcTimeWithOffset((DateTime)chatModel.ChatDate, locationModal.DaylightOffset, locationModal.StandardOffset, locationModal.TimeZoneName, tokenModel);

                //jsonModel.data = chatModel;

                //if (chatModel.IsMobileUser)
                //{
                //    HC.Patient.Model.Common.NotificationModel notificationModel = _tokenService.GetLoginNotification(chatModel.ToUserId, tokenModel);
                //    string connId = _chatService.GetConnectionId(chatModel.ToUserId);
                //    if (connId != null)
                //    {
                //        await Clients.Client(connId).SendAsync("NotificationResponse", notificationModel);
                //    }
                //}
                //else
                //{
                //    JsonModel notificationModel = _tokenService.GetChatAndNotificationCount(chatModel.ToUserId, tokenModel);
                //    string connId = _chatService.GetConnectionId(chatModel.ToUserId);
                //    if (connId != null)
                //    {
                //        await Clients.Client(connId).SendAsync("MobileNotificationResponse", notificationModel);
                //    }
                //}
                string connectionId = _chatService.GetConnectionId(chatModel.ToUserId);
                if (!string.IsNullOrEmpty(connectionId))
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", chatModel);
                jsonModel.data = chatModel;
                return jsonModel;

                //else
                //    await Clients.All.SendAsync("ReceiveMessage", message, fromUserId);
            }
            catch (Exception ex)
            {
                tokenModel.Request = null;
                return new JsonModel(tokenModel, ex.ToString(), 500);
            }
        }
        //public async Task SendFileInMessage(JObject file, int UserId, int currentRoomId)
        //{
        //    try
        //    {
        //        //IFormFile files = (IFormFile)file;
        //        FormFile files = JsonConvert.DeserializeObject<FormFile>(file.ToString());
        //        string connectionId = _chatService.GetConnectionId(UserId);
        //        var path = _chatService.SendFileInChat(files, currentRoomId, UserId);

        //        if (connectionId != null)
        //            await Clients.All.SendAsync("ReceiveMessage", path, UserId, currentRoomId);
        //        //else
        //        //    await Clients.All.SendAsync("ReceiveMessage", message, fromUserId);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //public async Task SendGroupMessage(string message, int fromUserId, int appointmentId)
        //{
        //    try
        //    {
        //        ChatModel chatModel = new ChatModel();
        //        TokenModel tokenModel = CommonMethods.GetTokenDataModel(Context.GetHttpContext());
        //        string connectionId = _chatService.GetConnectionId(toUserId);
        //        chatModel.ChatDate = DateTime.UtcNow;
        //        chatModel.FromUserId = fromUserId;
        //        chatModel.ToUserId = toUserId;
        //        chatModel.IsSeen = false;
        //        chatModel.Message = message;
        //        await _chatService.SaveChat(chatModel, tokenModel);

        //        if (connectionId != null)
        //            await Clients.Client(connectionId).SendAsync("ReceiveMessage", message, fromUserId);
        //        //else
        //        //    await Clients.All.SendAsync("ReceiveMessage", message, fromUserId);
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        /// <summary>
        /// this is use for message count notification by signalR for real time dynamic count for new message recieved
        /// </summary>
        /// <param name="forStaff"></param>
        /// <returns></returns>
        public async Task MessageCountRequest(bool forStaff)
        {
            HttpContext httpContext = Context.GetHttpContext();
            MessagesInfoFromSignalRModel messagesInfoFromSignalRModel = _messageService.ExecuteFunctions<MessagesInfoFromSignalRModel>(() => _messageService.GetMessagesInfoFromSignalR(forStaff, httpContext));
            await Clients.All.SendAsync("MessageCountResponse", messagesInfoFromSignalRModel);
        }

        public async Task NotificationRequest(bool forStaff)
        {
            HttpContext httpContext = Context.GetHttpContext();
            TokenModel token = CommonMethods.GetTokenDataModel(httpContext);
            NotificationModel notificationModel = _tokenService.GetLoginNotification(token);
            await Clients.All.SendAsync("NotificationResponse", notificationModel);
        }
        public async Task CallInitiate(int appointmentId, int UserId)
        {
            try
            {
                TokenModel tokenModel = GetBussinessToken(Context.GetHttpContext(), _tokenService);
                string connectionId = _chatService.GetConnectionId(UserId);

                if (connectionId != null)
                {

                    var otTokens = _telehealthRepository.GetOTTokenByAppointmentId(appointmentId, UserId, tokenModel);
                    if (otTokens != null & otTokens.ToList().Count > 0)
                    {
                        otTokens.ToList().ForEach(async x =>
                        {
                            if (x.CreatedBy != UserId)
                            {
                                var connId = _chatService.GetConnectionId((int)x.CreatedBy);
                                if (connId != null)
                                {

                                    await Clients.Client(connId).SendAsync("CallInitiated", appointmentId, UserId, x.CreatedBy);

                                }
                            }
                        });

                    }
                }

            }
            catch (Exception ex)
            {
            }
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
        public Task ImageMessage(ImageMessage file)
        {
            return Clients.All.SendAsync("ImageMessage", file);
        }


        //private void SaveNotification(ChatModel chatModel, int actionId, TokenModel token)
        //{
        //    NotificationModel notificationModel = new NotificationModel();
        //    if (chatModel.IsMobileUser) // from Patient
        //    {
        //        notificationModel.PatientId = _patientRepository.GetPatientByUserId(chatModel.FromUserId);
        //        Staffs staff = _tokenRepository.GetStaffByuserID(chatModel.ToUserId);
        //        notificationModel.StaffId = 0;
        //        if (staff != null)
        //        {
        //            notificationModel.StaffId = staff.Id;
        //        }
        //    }
        //    else
        //    {
        //        notificationModel.PatientId = _patientRepository.GetPatientByUserId(chatModel.ToUserId);
        //        Staffs staff = _tokenRepository.GetStaffByuserID(chatModel.FromUserId);
        //        notificationModel.StaffId = 0;
        //        if (staff != null)
        //        {
        //            notificationModel.StaffId = staff.Id;
        //        }
        //    }
        //    notificationModel.ActionTypeId = actionId;
        //    notificationModel.ChatId = chatModel.Id;
        //    notificationModel.Message = CommonMethods.Encrypt(chatModel.Message);
        //    notificationModel.IsMobileUser = chatModel.IsMobileUser;
        //    if (notificationModel.PatientId > 0 && notificationModel.StaffId > 0)
        //    {
        //        _notificationService.SaveNotification(notificationModel, token);
        //    }
        //}

    }
    public class ImageMessage
    {
        public byte[] ImageBinary { get; set; }
        public string ImageHeaders { get; set; }
    }
}
