using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterAdjustmentGroupCode:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column(TypeName ="varchar(5)")]
        public string Code { get; set; }
        public string CodeDescription { get; set; }
    }
}
