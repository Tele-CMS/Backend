using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class UserRoles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RoleID")]
        public int Id { get; set; }
        [StringLength(100)]
        public string RoleName { get; set; }

        public string UserType { get; set; }
        [NotMapped]
        public string value { get { return this.RoleName; } }
        [Required]
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        [ForeignKey("Users")]
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        [Required]
        public int OrganizationID { get; set; }
        public virtual User Users { get; set; }
    }
}
