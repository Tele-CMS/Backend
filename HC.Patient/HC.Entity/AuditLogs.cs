using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class AuditLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string OldValue { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string NewValue { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string Action { get; set; }

        [ForeignKey("AuditLogColumn")]
        public Nullable<int> AuditLogColumnId { get; set; }

        
        public AuditLogColumn AuditLogColumn { get; set; }

        [ForeignKey("patient")]
        public Nullable<int> PatientId { get; set; }

        
        public Patients Patient { get; set; }

        [ForeignKey("user")]
        public Nullable<int> CreatedById { get; set; }

        
        public User CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string ScreenName { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string EncryptionCode { get; set; }
        public Nullable<bool> AccessedToDelete { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [Column(TypeName ="varchar(200)")]
        public string ParentInfo { get; set; }

        
        [Column(TypeName = "varchar(200)")]
        public string IPAddress { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        
        [ForeignKey("Location")]
        public int LocationID { get; set; }

        
        [Column(TypeName = "varchar(100)")]
        public string LoginAttempt { get; set; }

        
        [Obsolete]
        public virtual Location Location { get; set; }

        
        [Obsolete]
        public virtual Organization Organization { get; set; }
    }
}
