using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Actions
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [StringLength(100)]
        public string ActionName { get; set; }

        [Column(TypeName ="varchar(100)")]
        public string ActionKey { get; set; }
        [ForeignKey("Screens")]
        public int ScreenId { get; set; }
        public virtual Screens Screens { get; set; }
        public bool IsActive { get; set; }
    }
}
