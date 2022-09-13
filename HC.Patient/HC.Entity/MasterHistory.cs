using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Patient.Entity
{
   
    [Table("master_history")]
    public class MasterHistory : BaseEntity
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(500)]
        public string name { get; set; }

    }
}
