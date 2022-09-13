using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterModifiers : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [Column(TypeName = "varchar(5)")]
        public string Modifier { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
