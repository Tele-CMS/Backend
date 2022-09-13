using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MenuPermissions : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public bool Permission { get; set; }
       
        [ForeignKey("UserRoles")]
        public int RoleId { get; set; }

        [ForeignKey("Menu")]
        public int MenuId { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual UserRoles UserRoles { get; set; }
    }
}