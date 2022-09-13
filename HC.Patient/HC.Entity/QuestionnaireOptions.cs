using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class QuestionnaireOptions : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int OptionId { get; set; }
        public string QuestionOptionName { get; set; }

        [Required]
        [ForeignKey("ProviderQuestionnaireQuestions")]
        public int QuestionId { get; set; }

        public virtual ProviderQuestionnaireQuestions ProviderQuestionnaireQuestions { get; set; }
    }
}
