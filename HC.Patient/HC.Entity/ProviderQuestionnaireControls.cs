using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class ProviderQuestionnaireControls
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ProviderQuestionnaireControlId")]
        public int Id { get; set; }

        [Required]
        public int StaffID { get; set; }

        [Required]
        [ForeignKey("QuestionnaireTypeControls")]
        public int QuestionnaireTypeControlId { get; set; }
        public virtual QuestionnaireTypeControls QuestionnaireTypeControl { get; set; }

        public string QuestionText { get; set; }
        public string Options { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDeleted { get; set; }
        public string Type { get; set; }
    }
}
