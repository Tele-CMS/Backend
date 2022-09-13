using System;
using System.Collections.Generic;
using System.IO;

namespace HC.Patient.Model.Message
{
    public class MessageComposeModel
    {
        public int Id { get; set; }
        public int FromUserId { get; set; } //it may be patient or staff
        public List<int?> ToUserIds { get; set; } //it may be patient or staff
        public string Subject { get; set; }
        public string Text { get; set; }       
        public bool IsFavourite { get; set; }
        public Dictionary<string, string> Base64 { get; set; }
        public bool ForStaff { get; set; }
        public int? ParentMessageId { get; set; } // if null then new message if greater then 0 then child of parent
    }
    public class InboxDataModel
    {
        public int MessageId { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Thumbnail { get; set; }
        public bool IsStaff { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public DateTime MessageDate { get; set; }        
        public bool Unread { get; set; }
        public bool IsFavourite { get; set; }
        public string AttachedDocs { get; set; } //from sp comma splitted
        public List<string> AttachedDocument { get; set; }
        public int? ParentMessageId { get; set; } // if null then Parent message if greater then 0 then child message with lastest of thread
        public bool FromInbox { get; set; }
        public int TotalCount { get; set; } //total messages count of this thread
        public decimal TotalRecords { get; set; }
        public string FromEmail { get; set; }
        public string StatusName { get; set; }
    }
    public class SentDataModel
    {
        public int MessageId { get; set; }
        public string ToName { get; set; }
        public string Thumbnail { get; set; }
        public bool IsStaff { get; set; }
        public string Subject { get; set; }
        public DateTime MessageDate { get; set; }
        public bool Unread { get; set; }
        public bool IsFavourite { get; set; }
        public string AttachedDocs { get; set; } //from sp comma splitted
        public List<string> AttachedDocument { get; set; }
        public int? ParentMessageId { get; set; } // if null then Parent message if greater then 0 then child message with lastest of thread
        public bool FromInbox { get; set; }
        public int TotalCount { get; set; } //total messages count of this thread
        public decimal TotalRecords { get; set; }
    }
    public class MessageDeleteModel
    {
        public List<int> Id { get; set; }
    }
    public class MessageDetailModel
    {
        public int MessageId { get; set; }
        public DateTime MessageDate { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Thumbnail { get; set; }
        public bool IsStaff { get; set; }        
        public string Subject { get; set; }
        public string Text { get; set; }
        public string AttachedDocs { get; set; } //from sp comma splitted
        public List<string> AttachedDocument { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public int? ParentMessageId { get; set; } // On reply u will get parent id
        public bool IsFavourite { get; set; }
    }
    public class ForwardMessageDetailModel
    {
        public int MessageId { get; set; }
        public DateTime MessageDate { get; set; }
        public string ToName { get; set; }
        public string FromName { get; set; }        
        public string Subject { get; set; }
        public string Text { get; set; }
        public string ForwardReply { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public int? ParentMessageId { get; set; } // On reply get u will get parent id 

    }
    public class FavouriteMessageModel
    {
        public int MessageId { get; set; }
        public string Subject { get; set; }
        public DateTime MessageDate { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string AttachedDocs { get; set; } //from sp comma splitted
        public List<string> AttachedDocument { get; set; }
        public decimal TotalRecords { get; set; }
    }
    public class DeletedMessageModel
    {
        public int MessageId { get; set; }
        public string Subject { get; set; }
        public DateTime MessageDate { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string AttachedDocs { get; set; } //from sp comma splitted
        public List<string> AttachedDocument { get; set; }
        public decimal TotalRecords { get; set; }
    }
    public class DownloadMessageDocumentModel
    {
        public int MessageId { get; set; }
        public MemoryStream File { get; set; }
        public string Name { get; set; }
        public string ApplicationType { get; set; }
        public string Extenstion { get; set; }

    }
    public class UsersDataModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Email { get; set; }
    }
    public class MessageCountModel
    {
        public decimal InboxCount { get; set; }
        public decimal SentboxCount { get; set; }
        public decimal FavouriteMessageCount { get; set; }
        public decimal DeletedMessageCount { get; set; }
    }
    
    public class MessagesInfoFromSignalRModel
    {
        public MessageCountModel MessageCount { get; set; }
        public List<InboxDataModel> InboxData { get; set; }
    }
}
