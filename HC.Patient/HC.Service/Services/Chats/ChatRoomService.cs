using HC.Common.HC.Common;
using HC.Model;
using HC.Service;
using System.Net;
using HC.Patient.Repositories.IRepositories;
using HC.Patient.Model.Chat;
using AutoMapper;
using HC.Patient.Entity;
using HC.Patient.Service.IServices;

namespace HC.Patient.Service.Services
{
    public class ChatRoomService : BaseService, IChatRoomService
    {
        private JsonModel response;

        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IMapper _mapper;
        public ChatRoomService(IChatRoomRepository chatRoomRepository, IMapper mapper)
        {
            response = new JsonModel(null, StatusMessage.NotFound, (int)HttpStatusCode.NotFound);
            _chatRoomRepository = chatRoomRepository;
            _mapper = mapper;
        }

        public JsonModel SaveChatRoomAsync(ChatRoomModel chatRoomModel, TokenModel tokenModel)
        {
            var chatRoom = _chatRoomRepository.GetChatRoomByName(chatRoomModel.RoomName, tokenModel);
            if (chatRoom != null)
                return new JsonModel(chatRoom, StatusMessage.ChatRoomExisted, (int)HttpStatusCode.Created);

            var room = _mapper.Map<ChatRoom>(chatRoomModel);
            room.OrganizationId = tokenModel.OrganizationID;
            room.CreatedBy = chatRoomModel.UserId;

            var savedRoom =  _chatRoomRepository.SaveNewChatRoom(room, tokenModel);

            if (savedRoom != null)
                response = new JsonModel(savedRoom, StatusMessage.ChatRoomSaved, (int)HttpStatusCode.OK);
            else
                response = new JsonModel(null, StatusMessage.ChatRoomNotSaved, (int)HttpStatusCode.InternalServerError);
            return response;
        }
    }
}
