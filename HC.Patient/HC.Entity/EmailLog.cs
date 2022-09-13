using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class EmailLog : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Subject { get; set; }

        [Column(TypeName = "text")]
        public string Body { get; set; }

        [MaxLength(200)]
        public string ToEmail { get; set; }
        public int PrimaryId { get; set; }
        public bool EmailStatus { get; set; }
        public int EmailType { get; set; }
        public int EmailSubType { get; set; }

    }
}
