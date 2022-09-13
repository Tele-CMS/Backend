using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterProgram : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ProgramID")]
        public int Id { get; set; }
        public string ProgramName { get; set; }
        [NotMapped]
        public string value { get { return this.ProgramName; } set { this.ProgramName = value; } }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }
}