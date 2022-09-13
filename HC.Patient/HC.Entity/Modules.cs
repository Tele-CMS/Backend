using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Modules
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [StringLength(100)]
        public string ModuleName { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ModuleKey { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        [Column(TypeName ="varchar(250)")]
        public string ModuleIcon { get; set; }
        [StringLength(100)]
        public string NavigationLink { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [ForeignKey("Menu")]
        public int? MenuID { get; set; }

        public virtual Menu Menu { get; set; }
    }
}
