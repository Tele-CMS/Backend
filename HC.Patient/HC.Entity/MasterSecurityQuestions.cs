using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterSecurityQuestions : MasterBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Question { get; set; }
        
        [NotMapped]
        public string value { get { return this.Question; } set { this.Question = value; } }                
    }
}