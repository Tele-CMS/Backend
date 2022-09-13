using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class UserRoleType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RoleTypeID")]
        public int Id { get; set; }
        [StringLength(200)]
        public string RoleTypeName { get; set; }

        [StringLength(200)]
        public string TypeKey { get; set; }

        [NotMapped]
        public string value { get { return this.RoleTypeName; } }

        [Required]
        public int OrganizationID { get; set; }
    }
}
