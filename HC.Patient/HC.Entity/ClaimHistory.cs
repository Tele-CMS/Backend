using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class ClaimHistory: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [ForeignKey("Claims")]
        public int ClaimId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Action { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string NewValue { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string OldValue { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string ColumnName { get; set; }
        public DateTime LogDate { get; set; }
        public virtual Claims Claims { get; set; }
    }
}
