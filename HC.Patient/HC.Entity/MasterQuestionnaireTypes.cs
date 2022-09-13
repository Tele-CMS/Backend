using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterQuestionnaireTypes : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("QuestionnaireTypeID")]
        public int Id { get; set; }
        public string QuestionnaireType { get; set; }
        [NotMapped]
        public string value { get { return this.QuestionnaireType; } set { this.QuestionnaireType = value; } }

        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }

}
