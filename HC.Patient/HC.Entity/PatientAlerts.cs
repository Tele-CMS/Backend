using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity
{
   public class PatientAlerts: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Patients")]
        public int PatientId { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string ProcedureCode { get; set; }
        public DateTime? DateOfService { get; set; }
        public DateTime? LoadDate { get; set; }
        public bool IsAlert { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string ICDCode { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string ClaimNumber { get; set; }
        public DateTime? FillDate { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string NDCCode { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string AlertDetails { get; set; }
        [ForeignKey("GlobalCode")]
        public int MasterAlertTypeId { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string LOINCCode { get; set; }
        [ForeignKey("DFA_PatientDocuments")]
        public int? PatientDocumentId { get; set; }
        public virtual Patients Patients { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual GlobalCode GlobalCode { get; set; }
        public virtual DFA_PatientDocuments DFA_PatientDocuments { get; set; }
    }
}
