using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
    public class ProviderQuestionnaireQuestions: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int QuestionId { get; set; }

        [Required]
        [Column(TypeName = "varchar(1500)")]
        public string QuestionNameName { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        [Required]
        [ForeignKey("ProviderQuestionnaires")]
        public int QuestionnareId { get; set; }
        public ProviderQuestionnaires ProviderQuestionnaires { get; set; }

    }
}
