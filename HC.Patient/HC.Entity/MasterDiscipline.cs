using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterDiscipline : BaseEntity
    {        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("DisciplineID")]
        public int Id { get; set; }
        public string Discipline { get; set; }
        [NotMapped]
        public string value { get { return this.Discipline; } set { this.Discipline = value; } }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }

}
