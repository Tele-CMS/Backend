using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Screens
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [StringLength(100)]
        public string ScreenName { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ScreenKey { get; set; }

        [StringLength(100)]
        public string NavigationLink { get; set; }

        [ForeignKey("Modules")]
        public int ModuleId { get; set; }
        public virtual Modules Modules { get; set; }

        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
    }
}
