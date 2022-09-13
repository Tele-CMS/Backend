using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterCustomLabels : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("CustomLabelID")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomLabelName { get; set; }

        [Required]
        [ForeignKey("GlobalCode")]
        public int CustomLabelTypeID { get; set; }

        [Required]
        [ForeignKey("UserRoleType")]
        public int RoleTypeID { get; set; }

        public virtual UserRoleType UserRoleType { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual GlobalCode GlobalCode { get; set; }        
    }
}
