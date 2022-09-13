using HC.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model.Chat
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsSeen { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public DateTime ChatDate { get; set; }
        public bool IsRecieved { get; set; }
        public decimal? TotalRecords { get; set; }
        public bool IsMobileUser { get; set; }
        public int RoomId { get; set; }
        public int? MessageType { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }


    public class CareChatModel
    {
        public int? Id { get; set; }
        public string Message { get; set; }
        public bool? IsSeen { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public DateTime? ChatDate { get; set; }
        public bool? IsRecieved { get; set; }
        public decimal? TotalRecords { get; set; }
        public bool IsMobileUser { get; set; }
        public int? PatientCareGapID { get; set; }
        public bool? IsConfirmType { get; set; }
        public int? ActionTypeId { get; set; }
        public DateTime? UTCDateTimeForMobile { get; set; }
        public DateTime? ChatDateForWebApp { get; set; }
        public int ChatType { get; set; }
    }

    public class ChatParmModel : FilterModel
    {
        public int FromUserId { get; set; } = 0;
        public int ToUserId { get; set; } = 0;
        public int RoomId { get; set; }
        public int OrganizationId { get; set; }
    }

    public class ChatConnectedUserModel
    {
        public string ConnectionId { get; set; }
        public int UserId { get; set; }
    }
    public class ChatRoomModel
    {
        public string RoomName { get; set; }
        public int OranizationId { get; set; }
        public int UserId { get; set; }
    }
    public class ChatRoomUserModel
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
    }

    public class ChatRoomUserDetailModel
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public string Role { get; set; }
        public string Image { get; set; }
    }
    public class ChatFileResponseModel
    {
        public string FileName { get; set; }
        public string Message { get; set; }
        public string FileType { get; set; }
        public int MessageType { get; set; }
        public int RoomId { get; set; }
        public Boolean isRecieved { get; set; } = true;
    }
   public class RoomConnectionModel
    {
        public string ConnectionId { get; set; }
        public int UserId { get; set; }

    }
}
