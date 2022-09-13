using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientInsuranceDetails : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientInsuranceID")]
        public int Id { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("InsuranceCompanies")]
        public int InsuranceCompanyID { get; set; }
        [StringLength(500)]
        public string InsuranceCompanyAddress { get; set; }
        
        public byte[] InsuranceIDNumber { get; set; }
        [StringLength(100)]
        public string InsuranceGroupName { get; set; }
        [StringLength(100)]
        public string InsurancePlanName { get; set; }
        public int? InsurancePlanTypeID { get; set; }
        public int? InsuranceTypeID { get; set; }
        [StringLength(20)]
        public string InsuranceClaimOfficeNumber { get; set; }
        public int? VisitsAllowedPerYear { get; set; }
        public DateTime? CardIssueDate { get; set; }
        [StringLength(500)]
        public string Notes { get; set; }    
        public string InsurancePhotoPathFront { get; set; }
        public string InsurancePhotoPathThumbFront { get; set; }
        [NotMapped]
        public string Base64Front { get; set; }
        public string InsurancePhotoPathBack { get; set; }
        public string InsurancePhotoPathThumbBack { get; set; }
        [NotMapped]        
        public string Base64Back { get; set; }
        public bool? InsurancePersonSameAsPatient { get; set; }

        [StringLength(20)]        
        public string CarrierPayerID { get; set; }
        public bool? IsVerified { get; set; }

        [StringLength(50)]        
        public string InsuranceGroupNumber { get; set; }
        public virtual Patients Patient { get; set; }        
        public virtual InsuredPerson InsuredPerson { get; set; }
        public virtual InsuranceCompanies InsuranceCompanies { get; set; }
    }
}