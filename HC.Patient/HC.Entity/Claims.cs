using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class Claims : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        public DateTime DOS { get; set; }

        [ForeignKey("Location")]
        public int ServiceLocationID { get; set; }


        [ForeignKey("MasterClaimStatus")]
        public int ClaimStatusId { get; set; }

        [ForeignKey("Staffs1")]
        public int ClinicianId { get; set; }

        [ForeignKey("Staffs2")]
        public int RenderingProviderId { get; set; }

        [ForeignKey("PatientInsuranceDetails")]
        public int PatientInsuranceId { get; set; }

        [ForeignKey("Patients")]
        public int PatientId { get; set; }

        [ForeignKey("PatientAddress")]
        public int? PatientAddressID { get; set; }

        [ForeignKey("Location1")]
        public int? OfficeAddressID { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string CustomAddress { get; set; }

        [ForeignKey("MasterPatientLocation")]
        public int? CustomAddressID { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string AdditionalClaimInfo { get; set; }
     
        public int? SubmissionType { get; set; }

        public DateTime? SubmittedDate { get; set; }

        [ForeignKey("ClaimPaymentStatus")]
        public int? ClaimPaymentStatusId { get; set; }

        public DateTime? ClaimSettledDate { get; set; }
        
        public virtual Organization Organization { get; set; }
        public virtual MasterPatientLocation MasterPatientLocation { get; set; }
        public virtual MasterClaimStatus MasterClaimStatus { get; set; }
        public virtual Staffs Staffs1 { get; set; }
        public virtual Staffs Staffs2 { get; set; }
        public virtual PatientInsuranceDetails PatientInsuranceDetails { get; set; }
        public virtual PatientAddress PatientAddress { get; set; }
        public virtual Location Location { get; set; }
        public virtual Location Location1 { get; set; }
        public virtual GlobalCode ClaimPaymentStatus { get; set; }        
    }
}
