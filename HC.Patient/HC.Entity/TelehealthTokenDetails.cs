using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class TelehealthTokenDetails : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public  int Id { get; set; }
        
        [ForeignKey("TelehealthSessionDetails")]
        public int TelehealthSessionDetailID { get; set; }
       
        public string Token { get; set; }
       
        public double TokenExpiry { get; set; }
     
        public bool IsStaffToken { get; set; }

        [NotMapped]
        public Exception exception { get; set; }

        [NotMapped]
        public int result { get; set; }

        public virtual TelehealthSessionDetails TelehealthSessionDetails { get; set; }

        [ForeignKey("GroupSessionInvitations")]
        public int? InvitationId { get; set; }
        public GroupSessionInvitations GroupSessionInvitations { get; set; }
        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; }
        public Organization Organization { get; set; }

    }
}
