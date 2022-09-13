using HC.Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PatientEncounter : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("Patient")]
        public Nullable<int> PatientID { get; set; }

        [ForeignKey("PatientAppointment")]
        public int? PatientAppointmentId { get; set; }

        [Required]
        [RequiredDate]
        public DateTime DateOfService { get; set; }

        public DateTime StartDateTime { get; set; }

        [RequiredDate]        
        public DateTime EndDateTime { get; set; }

        //[Required]
       // [RequiredNumber]
        [ForeignKey("Staff")]
        public int? StaffID { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string NonBillableNotes { get; set; }
        public bool? IsBillableEncounter { get; set; }

        [ForeignKey("PatientAddress")]
        public int? PatientAddressID { get; set; }

        [ForeignKey("Location1")]
        public int? OfficeAddressID { get; set; }

        [ForeignKey("Location")]
        public int? ServiceLocationID { get; set; }
        public int Status { get; set; }

        [ForeignKey("MasterNoteType")]
        public int? NotetypeId { get; set; }

        [Required]
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string CustomAddress { get; set; }

        [ForeignKey("MasterPatientLocation1")]
        public int? CustomAddressID { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Notes { get; set; }
        public string ManualChiefComplaint { get; set; }
        [ForeignKey("EncounterType")]
        public int? EncounterTypeId { get; set; }
        [ForeignKey("EncounterMethod")]
        public int? EncounterMethodId { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string MemberNotes { get; set; }
        public virtual GlobalCode EncounterType { get; set; }
        public virtual GlobalCode EncounterMethod { get; set; }
        public virtual MasterPatientLocation MasterPatientLocation1 { get; set; }

        //Foreign key's tables        
        public virtual Staffs Staff { get; set; }        
        public virtual Patients Patient { get; set; }

        [ForeignKey("AppointmentType")]
        public int? AppointmentTypeId { get; set; }

        [Column(TypeName ="varchar(max)")]
        public string ChiefComplaint { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string CareManager { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Comments { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Activity { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Goals { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Medication { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string Nutrition { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string SmokingAlcohol { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string DiseaseStates { get; set; }

        public bool IsImported { get; set; }
        public bool IsImportUpdated { get; set; }
        public virtual AppointmentType AppointmentType { get; set; }

        public long VisitId { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string ICD9 { get; set; }
        public bool IsPatientEligible { get; set; }

        [Obsolete]
        public virtual MasterPatientLocation MasterPatientLocation { get; set; }                
        public virtual PatientAppointment PatientAppointment { get; set; }        
        public virtual MasterNoteType MasterNoteType { get; set; }        
        public virtual Organization Organization { get; set; }
        public virtual PatientAddress PatientAddress { get; set; }
        public virtual Location Location { get; set; }
        public virtual Location Location1 { get; set; }
    }
}