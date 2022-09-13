using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class ClaimResubmissionReason : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string ResubmissionReason { get; set; }
        [Column(TypeName = "varchar(4)")]
        public string ResubmissionCode { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Description { get; set; }
    }
}
