using HC.Patient.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    
    [Table("MasterSystem")]
    public class MasterSystem : BaseEntity
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

    }
}
