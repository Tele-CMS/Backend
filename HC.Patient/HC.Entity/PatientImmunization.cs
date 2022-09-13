using HC.Common.Filters;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientImmunization : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ImmunizationId")]
        public int Id { get; set; }
        [Required]
        [RequiredNumber]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }

        [ForeignKey("Staff")]
        public int? OrderBy { get; set; }

        [ForeignKey("MasterVFCEligibility")]
        public int? VFCID { get; set; }

        [Required]
        [RequiredDate]
        public DateTime AdministeredDate { get; set; }

        [Required]
        [RequiredNumber]
        [ForeignKey("MasterImmunization")]
        public int Immunization { get; set; }

        
        public int? AmountAdministered { get; set; }
        
        [ForeignKey("MasterManufacture")]
        public int? ManufactureID { get; set; }
        [Required]
        [RequiredDate]
        public DateTime? ExpireDate   { get; set; }
        public string VaccineLotNumber { get; set; }


        [ForeignKey("MasterAdministrationSite")]
        public int? AdministrationSiteID { get; set; }
        [ForeignKey("MasterRouteOfAdministration")]
        public int? RouteOfAdministrationID { get; set; }

        [ForeignKey("Staff1")]
        public int? AdministeredBy { get; set; }
        
        [ForeignKey("MasterImmunityStatus")]
        [Required]
        [RequiredNumber]
        public int ImmunityStatusID { get; set; }

        [Required]
        
        public bool RejectedImmunization { get; set; }
        
        [ForeignKey("MasterRejectionReason")]
        public int? RejectionReasonID { get; set; }
        
        public string RejectionReasonNote { get; set; }

        
        /// <summary>
        /// Foreign key's tables
        /// </summary>       
        
        public virtual Patients Patient { get; set; }
        public virtual Staffs Staff { get; set; }
        public virtual MasterVFCEligibility MasterVFCEligibility { get; set; }
        public MasterImmunization MasterImmunization { get; set; }
        public MasterManufacture MasterManufacture { get; set; }
        public virtual MasterAdministrationSite MasterAdministrationSite { get; set; }
        public MasterRouteOfAdministration MasterRouteOfAdministration { get; set; }
        public virtual Staffs Staff1 { get; set; }
        public MasterImmunityStatus MasterImmunityStatus { get; set; }
        public MasterRejectionReason MasterRejectionReason { get; set; }
        [NotMapped]
        public string ManufacturerName { get; set; }
    }
}