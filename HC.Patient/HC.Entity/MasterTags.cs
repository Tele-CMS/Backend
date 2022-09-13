using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterTags : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("TagID")]
        public int Id { get; set; }
        [StringLength(100)]
        public string Tag { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }

        [NotMapped]
        public string value { get { return this.Tag; } set { this.Tag = value; } }
        public string ColorName { get; set; }
        public string FontColorCode { get; set; }
        public string ColorCode { get; set; }

        [ForeignKey("UserRoleType")]
        public int? RoleTypeID { get; set; }
        [NotMapped]
        public string RoleTypeName
        {
            get
            {
                try
                {
                    return this.UserRoleType != null? this.UserRoleType.RoleTypeName: null;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        [NotMapped]
        public string TypeKey { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual UserRoleType UserRoleType { get; set; }        
    }
}