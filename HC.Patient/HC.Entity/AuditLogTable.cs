using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class AuditLogTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        
        public int Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string TableName { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }

        
        
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        
        [Obsolete]
        public virtual Organization Organization { get; set; }
    }
}
