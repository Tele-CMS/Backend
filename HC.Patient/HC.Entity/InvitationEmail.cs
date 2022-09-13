using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class InvitationEmail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Subject { get; set; }

        [MaxLength(2000)]
        public string Body { get; set; }

        [MaxLength(20)]
        public string ToEmail { get; set; }

        [ForeignKey("UserInvitation")]
        public int InvitationId { get; set; }
        public UserInvitation UserInvitation { get; set; }
        public bool EmailStatus { get; set; }

    }
}
