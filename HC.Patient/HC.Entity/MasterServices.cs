using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterServices : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(200)")]
        public string ServiceName { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        
        [Required]
        [ForeignKey("GlobalCode")]
        public int GlobalCodeId { get; set; }
        public GlobalCode GlobalCode { get; set; }
    }
}
