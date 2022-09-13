using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Event : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("EventId")]
        public int Id { get; set; }        
        public DateTime InsertedDate { get; set; }
        
        public DateTime? LastUpdatedDate { get; set; }
        
        public string Data { get; set; }
    }
}
