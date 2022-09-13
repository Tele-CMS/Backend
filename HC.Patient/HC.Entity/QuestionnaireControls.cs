using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HC.Patient.Entity
{
    public class QuestionnaireControls : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("QuestionnaireControlID")]
        public int Id { get; set; }
        public string ControlName { get; set; }
        public string Key { get; set; }
        public bool HasOptions { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
    }
}
