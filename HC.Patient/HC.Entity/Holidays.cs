using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Holidays : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]        
        public int  Id{ get; set; }
        public DateTime Date { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Description{ get; set; }
    }
}
