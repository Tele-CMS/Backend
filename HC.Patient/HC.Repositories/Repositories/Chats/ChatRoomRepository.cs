using System;
using System.Collections.Generic;
using System.Text;
using HC.Model;
using HC.Patient.Data;
using HC.Patient.Entity;
using HC.Repositories;
using System.Linq;
using HC.Patient.Repositories.IRepositories;
using System.Threading.Tasks;

namespace HC.Patient.Repositories.Repositories
{
    public class ChatRoomRepository : RepositoryBase<ChatRoom>, IChatRoomRepository
    {
        private HCOrganizationContext _context;
        public ChatRoomRepository(HCOrganizationContext context) : base(context)
        {
            _context = context;
        }
        public ChatRoom SaveNewChatRoom(ChatRoom chatRoom, TokenModel tokenModel)
        {
            _context.ChatRooms.Add(chatRoom);
            var result = _context.SaveChanges();
            if (result > 0)
                return chatRoom;
            else
                return null;
        }
        public ChatRoom GetChatRoomByName(string roomName, TokenModel tokenModel)
        {
            return _context.ChatRooms.Where(s => s.RoomName == roomName && s.OrganizationId == tokenModel.OrganizationID).FirstOrDefault();
        }
        public string GetChatRoomNameById(int roomId)
        {
            return _context.ChatRooms.Where(x => x.Id == roomId && x.IsActive == true && x.IsDeleted == false).Select(x => x.RoomName).FirstOrDefault();
        }
    }
}

