using System;

namespace HC.Patient.Entity
{
    public class MessageRecepient : CommonFields
    {
        public int MessageRecepientID { get; set; }
        public DateTime MessageDate { get; set; }        
        public bool Unread { get; set; }
        public int MessageId { get; set; }
        public int ToUserID { get; set; }
        public bool IsFavourite { get; set; }
        public Message Message { get; set; }
        public User User { get; set; }
    }
}
