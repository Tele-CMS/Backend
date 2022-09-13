using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Message : CommonFields
    {
        public int MessageID { get; set; }
        public DateTime MessageDate { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public int FromUserID { get; set; }
        public bool IsFavourite { get; set; }
        public int OrganizationId { get; set; }

        [ForeignKey("Messages")]
        public int? ParentMessageId { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<MessageRecepient> MessageRecepients { get; set; }
        public virtual ICollection<MessageDocuments> MessageDocuments { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

    }
}
