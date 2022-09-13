using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class RolePermission : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RolePermissionID")]
        public  int Id { get; set; }
        public bool CreatePermission { get; set; }
        public bool ViewPermission { get; set; }
        public bool EditPermission { get; set; }
        public bool DeletePermission { get; set; }
        [ForeignKey("UserRoles")]
        public int RoleID { get; set; }
        [ForeignKey("Modules")]
        public int ModuleID { get; set; }
        [Obsolete]
        public virtual Modules Modules { get; set; }
        [Obsolete]
        public virtual UserRoles UserRoles { get; set; }
    }
}