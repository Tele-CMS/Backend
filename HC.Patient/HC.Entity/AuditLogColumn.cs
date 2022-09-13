using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class AuditLogColumn 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]        
        public int Id { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string ColumnName { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey("AuditLogTable")]
        public int AuditLogTableId { get; set; }

        
        [Obsolete]
        public AuditLogTable AuditLogTable { get; set; }

        [Column(TypeName ="varchar(2000)")]
        public string Query { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
