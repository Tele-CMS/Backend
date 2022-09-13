using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class UserSecurityQuestionAnswer : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int Id { get; set; }

        [ForeignKey("SecurityQuestions")]
        public int QuestionID { get; set; }

        public virtual SecurityQuestions SecurityQuestions { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Answer { get; set; }
        [ForeignKey("Users3")]
        public int UserID { get; set; }
        public virtual User Users3 { get; set; }
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }
}