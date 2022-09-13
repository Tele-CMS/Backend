using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class InvitationRejectLog : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string RejectRemarks { get; set; }

        [ForeignKey("UserInvitation")]
        public int InvitationId { get; set; }

        public UserInvitation UserInvitation { get; set; }

    }
}
