using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class ScreenPermissions:BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public bool Permission { get; set; }

        [ForeignKey("UserRoles")]
        public int RoleId { get; set; }

        [ForeignKey("Modules")]
        public int ModuleId { get; set; }

        [ForeignKey("Screens")]
        public int ScreenId { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Modules Modules { get; set; }
        public virtual UserRoles UserRoles { get; set; }
        public virtual Screens Screens { get; set; }
    }
}
