using HC.Common.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HC.Patient.Entity
{
    public class Patients 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PatientID")]
        public int Id { get; set; }
        public byte[] MRN { get; set; }           
        public byte[] FirstName { get; set; }
        public byte[] MiddleName { get; set; }
        public byte[] LastName { get; set; }
        [StringLength(100)]
        public string ClientID { get; set; }

        [Required]
        [RequiredNumber]
        [ForeignKey("MasterGender")]
        public int Gender { get; set; }
        [NotMapped]
        public string GenderValue
        {
            get
            {
                try
                {
                    return MasterGender != null?MasterGender.Gender: null;
                }
                catch (Exception)
                {

                    return null;
                }
            }

            set { }
        }
        public byte[] DOB { get; set; }
        
        public byte[] SSN { get; set; }
        
        public byte[] Email { get; set; }
        
        public bool? OptOut { get; set; }
        
        [ForeignKey("MasterMaritalStatus")]
        public int? MaritalStatus { get; set; }
        
        [ForeignKey("MasterRace")]
        public int? Race { get; set; }
        
        [ForeignKey("SecondaryRace")]
        public int? SecondaryRaceID { get; set; }
        
        [ForeignKey("MasterEthnicity")]
        public int? Ethnicity { get; set; }
        
        [StringLength(100)]
        public string PrimaryProvider { get; set; }

        
       // [ForeignKey("RenderingProvider")]
        public int? RenderingProviderID { get; set; }
        
        [StringLength(100)]
        public string EmergencyContactFirstName { get; set; }
        
        [StringLength(100)]
        public string EmergencyContactLastName { get; set; }
        
        [StringLength(20)]
        public string EmergencyContactPhone { get; set; }
        
        [ForeignKey("RelationShip")]
        public int? EmergencyContactRelationship { get; set; }
        
        [StringLength(100)]
        public string EmergencyContactOthers { get; set; }
        public string PhotoPath { get; set; }

        [NotMapped]        
        public string PhotoBase64 { get; set; }
        
        public string PhotoThumbnailPath { get; set; }

        [ForeignKey("MasterCountry")]
        public int? Citizenship { get; set; }        
        public string Note { get; set; }        
        [ForeignKey("Location")]
        public int LocationID { get; set; }

        [Required]
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        [ForeignKey("Users2")]
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsBBEnabled { get; set; }
        public byte[] Identifier { get; set; }
        public bool IsEligible { get; set; }
        
        [NotMapped]
        public DateTime FromDate
        {
            get
            {
                try
                {
                    return this.CreatedDate;
                }
                catch (Exception)
                {

                    return DateTime.UtcNow;
                }
            }
        }
        [NotMapped]
        public DateTime ToDate
        {
            get
            {
                try
                {
                    return this.CreatedDate;
                }
                catch (Exception)
                {

                    return DateTime.UtcNow;
                }
            }
        }
        [NotMapped]
        public DateTime FromDOB
        {
            get
            {
                try
                {
                    return DateTime.Now;
                    //return this.DOB;
                }
                catch (Exception)
                {

                    return DateTime.UtcNow;
                }
            }
        }
        [NotMapped]
        public DateTime ToDOB
        {
            get
            {
                try
                {
                    return DateTime.Now;
                    //return this.DOB;
                }
                catch (Exception)
                {

                    return DateTime.UtcNow;
                }
            }
        }
        public string CustomerId { get; set; }
        [ForeignKey("Users3")]
        public int? UserID { get; set; }

        [ForeignKey("Users")]
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("Users1")]
        public int? UpdatedBy { get; set; }
        public bool IsVerified { get; set; }
        [ForeignKey("Employment")]
        public int? EmploymentID { get; set; }
        [NotMapped]
        public string StaffName
        {
            get
            {
                try
                {
                    return Staff != null?( Staff.FirstName + " " + Staff.LastName) : null;
                }
                catch (Exception)
                {

                    return null;
                }
            }
        }

        [NotMapped]
        public string PatientInsurance
        {
            get
            {
                try
                {
                    return PatientInsuranceDetails.FirstOrDefault().InsurancePlanName;
                }
                catch (Exception)
                {

                    return null;
                }
            }

        }

        [NotMapped]
        public string PatientNumber
        {
            get
            {
                try
                {
                    return null;
                    //return PhoneNumbers.FirstOrDefault().PhoneNumber;
                }
                catch (Exception)
                {

                    return null;
                }
            }

        }

        [NotMapped]
        public bool IsBlock
        {
            get
            {
                try
                {
                    return Users3 != null? Users3.IsBlock: false;
                }
                catch (Exception)
                {

                    return false;
                }
            }
            set { }

        }

        [NotMapped]
        public dynamic PatientDiagnosis1
        {
            get
            {
                try
                {
                    return this.PatientDiagnosis.Where(l => l.IsDeleted == false).Select(p => new PatientDiagnosis
                    {
                        Id = p.Id,
                        ICDID = p.ICDID,
                        DiagnosisDate = p.DiagnosisDate,
                        PatientID = p.PatientID,
                        IsActive = p.IsActive
                    });
                }
                catch (Exception)
                {
                    return null;
                }
            }
            set
            {
                //this.AuthProcedureCPTLink2 = AuthProcedureCPTLink;
            }
        }

        [Required]
        public int OrganizationID { get; set; }
        public bool IsPortalActivate { get; set; }

        [Required]
        [StringLength(100)]
        [NotMapped]
        public string UserName { get; set; }
        public bool IsPortalRequired { get; set; }
                
        /// <summary>
        /// following tables are used for relationship
        /// </summary>

        public virtual Staffs Staff { get; set; }

        public virtual MasterGender MasterGender { get; set; }
        public virtual GlobalCode MasterMaritalStatus { get; set; }
        public virtual MasterRace MasterRace { get; set; }
        public virtual MasterRace SecondaryRace { get; set; }
        public virtual MasterEthnicity MasterEthnicity { get; set; }
       // public virtual Staffs RenderingProvider { get; set; }
        public virtual MasterRelationship RelationShip { get; set; }
        public virtual GlobalCode Employment { get; set; }
        public virtual MasterCountry MasterCountry { get; set; }        
        public virtual User Users { get; set; }
        public virtual User Users1 { get; set; }
        public virtual User Users2 { get; set; }
        public virtual User Users3 { get; set; }
        public virtual List<PatientAddress> PatientAddress { get; set; }
        public virtual List<PatientMedicalFamilyHistory> PatientMedicalFamilyHistory { get; set; }
        public virtual List<PatientInsuranceDetails> PatientInsuranceDetails { get; set; }
        public virtual List<PhoneNumbers> PhoneNumbers { get; set; }
        public virtual List<PatientAppointment> PatientAppointment { get; set; }
        public virtual List<PatientGuardian> PatientGuardian { get; set; }
        public virtual List<PatientDiagnosis> PatientDiagnosis { get; set; }
        public virtual Location Location { get; set; }
        public virtual List<PatientAllergies> PatientAllergies { get; set; }
        public virtual List<Authorization> Authorization { get; set; }
        public virtual List<PatientCustomLabels> PatientCustomLabels { get; set; }
        public virtual List<PatientVitals> PatientVitals { get; set; }
        public virtual List<InsuredPerson> InsuredPerson { get; set; }
        public virtual List<PatientTags> PatientTags { get; set; }
        public virtual List<PatientLabTest> PatientLabTest { get; set; }
        public virtual List<PatientMedication> PatientMedication { get; set; }
        [NotMapped]
        public string Address
        {
            get
            {
                try
                {
                    if (PatientAddress != null && PatientAddress.FirstOrDefault() != null)
                    {
                        return PatientAddress.FirstOrDefault().Address1 + " " + PatientAddress.FirstOrDefault().City + ", " + PatientAddress.FirstOrDefault().Zip + ", " + PatientAddress.FirstOrDefault().StateID;
                    }
                    return string.Empty;
                }
                catch (Exception)
                {

                    return string.Empty;
                }
            }
        }
    }
}