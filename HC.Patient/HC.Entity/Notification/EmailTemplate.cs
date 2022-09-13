using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class EmailTemplate : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public string Template { get; set; }
        [ForeignKey("MasterEmailType")]
        public int EmailTypeId { get; set; }
        public virtual MasterEmailType MasterEmailType { get; set; }
    }
}
