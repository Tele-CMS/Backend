using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class QuestionnaireTypeControls : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("QuestionnaireTypeControlID")]
        public int Id { get; set; }

        [ForeignKey("MasterQuestionnaireTypes")]
        public int QuestionnaireTypeId { get; set; }

        [ForeignKey("QuestionnaireControls")]
        public int QuestionnaireControlId { get; set; }

        public virtual QuestionnaireControls QuestionnaireControls { get; set; }
        public virtual MasterQuestionnaireTypes MasterQuestionnaireTypes { get; set; }

    }
}
