using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class ClaimServiceLine : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("Claims")]        
        public int ClaimId { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string ServiceCode { get; set; }

        [Column(TypeName ="varchar(5)")]
        public string Modifier1 { get; set; }
        public decimal? RateModifier1 { get; set; }
        [Column(TypeName = "varchar(5)")]
        public string Modifier2 { get; set; }
        public decimal? RateModifier2 { get; set; }
        [Column(TypeName = "varchar(5)")]
        public string Modifier3 { get; set; }
        public decimal? RateModifier3 { get; set; }
        [Column(TypeName = "varchar(5)")]
        public string Modifier4 { get; set; }
        public decimal? RateModifier4 { get; set; }
        public decimal Rate { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }


        [ForeignKey("ClaimDiagnosisCode1")]
        public Nullable<int> DiagnosisCodePointer1 { get; set; }

        [ForeignKey("ClaimDiagnosisCode2")]
        public Nullable<int> DiagnosisCodePointer2 { get; set; }

        [ForeignKey("ClaimDiagnosisCode3")]
        public Nullable<int> DiagnosisCodePointer3 { get; set; }

        [ForeignKey("ClaimDiagnosisCode4")]
        public Nullable<int> DiagnosisCodePointer4 { get; set; }

        public string AuthorizationNumber { get; set; }

        [ForeignKey("AuthProcedureCPT")]
        public Nullable<int> AuthProcedureCPTLinkId { get; set; }
        public virtual AuthProcedureCPT AuthProcedureCPT { get; set; }

        public bool IsBillable { get; set; }

        public bool? IsPatientResponsible { get; set; }

        [ForeignKey("Staffs1")]
        public int? ClinicianId { get; set; }

        [ForeignKey("Staffs2")]
        public int? RenderingProviderId { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string CustomAddress { get; set; }

        [ForeignKey("MasterPatientLocation")]
        public int? CustomAddressID { get; set; }

        [ForeignKey("PatientAddress")]
        public int? PatientAddressID { get; set; }

        [ForeignKey("Location1")]
        public int? OfficeAddressID { get; set; }
        
        public virtual PatientAddress PatientAddress { get; set; }
        public virtual Location Location1 { get; set; }        
        public virtual Staffs Staffs1 { get; set; }
        public virtual Staffs Staffs2 { get; set; }
        public virtual MasterPatientLocation MasterPatientLocation { get; set; }
        public virtual ClaimDiagnosisCode ClaimDiagnosisCode1 { get; set; }        
        public virtual ClaimDiagnosisCode ClaimDiagnosisCode2 { get; set; }        
        public virtual ClaimDiagnosisCode ClaimDiagnosisCode3 { get; set; }
        public virtual ClaimDiagnosisCode ClaimDiagnosisCode4 { get; set; }
        public virtual Claims Claims { get; set; }
    }
}
