using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class UserInvitation : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required(ErrorMessage = "Provider Name Required")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        
        [MaxLength(50)]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Provider Last Name Required")]
        [MaxLength(50)]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Provider Email Address Required")]
        [MaxLength(50)]
        public string Email { get; set; }


        [MaxLength(15)]
        public string Phone { get; set; }


        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public Location Location { get; set; }


        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        public DateTime InvitationSendDate { get; set; }

        public int InvitationStatus { get; set; }

        [ForeignKey("UserRoles")]
        public int RoleId { get; set; }
        public UserRoles UserRoles { get; set; }

        [ForeignKey("InvitedUser")]
        public int? InvitedUserId { get; set; }
        public User InvitedUser { get; set; }

    }
}
