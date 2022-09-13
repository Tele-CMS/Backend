using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HC.Patient.Entity
{
    public class User 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("UserID")]
        public int Id { get; set; }
        
        [ForeignKey("User1")]        
        public int? CreatedBy { get; set; }        
        [MaxLength(100)]
        public string UserName { get; set; }        
        public string Password { get; set; }        
        public int AccessFailedCount { get; set; }
        public bool IsBlock { get; set; }
        public DateTime? BlockDateTime { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public bool? IsPasscodeEnable { get; set; }
        public DateTime? CreatedDate { get; set; }
        [Required]
        [ForeignKey("UserRoles")]
        public int RoleID { get; set; }

        public bool IsOnline { get; set; }

        [NotMapped]
        public string RoleName { get; set; }

        [ForeignKey("Users")]
        public int? DeletedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }

        public Nullable<DateTime> PasswordResetDate { get; set; }        
        public virtual User Users { get; set; }        
        public virtual User User1 { get; set; }
        public virtual UserRoles UserRoles { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<MessageRecepient> MessageRecepients { get; set; }
        public virtual ChatConnectedUser ChatConnectedUser { get; set; }
        public int? ExpirationPeriodInDays { get; set; }
        public bool? IsSecurityQuestionMandatory { get; set; }
        public bool? OpenDefaultClient { get; set; }
        public Guid? TempResetPasswordId { get; set; }
        public DateTime? TempIdGeneratedTime { get; set; }
        public string DeviceToken { get; set; }
        public string ApnToken { get; set; }
        public string NPINumber { get; set; }
    }
}
