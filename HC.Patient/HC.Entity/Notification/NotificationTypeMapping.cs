using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class NotificationTypeMapping : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [ForeignKey("MasterNotificationType")]
        public int NotificationTypeId { get; set; }
        [ForeignKey("Notifications")]
        public int NotificationId { get; set; }
        public bool IsSendNotification { get; set; }
        public bool IsReceivedNotification { get; set; }
        public bool IsReadNotification { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int? AttemptCount { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        public virtual MasterNotificationType MasterNotificationType { get; set; }
        public virtual Notifications Notifications { get; set; }
        public virtual Organization Organization { get; set; }

    }
}
