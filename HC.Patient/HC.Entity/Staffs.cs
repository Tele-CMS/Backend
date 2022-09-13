using HC.Common.Filters;
using HC.Common.Model.Staff;
using HC.Patient.Entity.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HC.Patient.Entity
{
    public class Staffs : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("StaffID")]
        public int Id { get; set; }
        [NotMapped]
        public string providerId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [NotMapped]
        public string value { get { return this.FirstName + " " + this.LastName; } }
        [ForeignKey("MasterGender")]
        public int? Gender { get; set; }
        [Required]
        [RequiredDate]
        public DateTime DOB { get; set; }
        [Required]
        [RequiredDate]
        public DateTime DOJ { get; set; }
        [MaxLength(1000)]
        public string Address { get; set; }
        [Required]
        [StringLength(256)]
        public string Email { get; set; }
        public int? MaritalStatus { get; set; }
        [StringLength(50)]
        public string NPINumber { get; set; }
        [StringLength(50)]
        public string TaxId { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        [NotMapped]
        public string Password { get; set; }
        [Required]
        [ForeignKey("UserRoles")]
        public int RoleID { get; set; }
        [NotMapped]
        public string RoleName { get; set; }
        public string PhotoPath { get; set; }
        [NotMapped]
        public string PhotoBase64 { get; set; }
        public string PhotoThumbnailPath { get; set; }
        [ForeignKey("Users3")]
        public int UserID { get; set; }
        [StringLength(100)]
        public string City { get; set; }
        [ForeignKey("MasterState")]
        public int? StateID { get; set; }
        [ForeignKey("MasterCountry")]
        public int? CountryID { get; set; }
        [StringLength(20)]
        public string Zip { get; set; }
        [StringLength(20)]
        public string PhoneNumber { get; set; }
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
        }
        public bool IsRenderingProvider { get; set; }
        public bool IsUrgentCare { get; set; }
        [StringLength(15)]
        public string CAQHID { get; set; }
        [StringLength(50)]
        public string Language { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        [ForeignKey("EmployeeType")]
        public int? EmployeeTypeID { get; set; }
        public DateTime? TerminationDate { get; set; }
        [StringLength(20)]
        public string SSN { get; set; }
        [ForeignKey("PayrollGroup")]
        public int? PayrollGroupID { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        [StringLength(20)]
        public string ApartmentNumber { get; set; }
        [ForeignKey("MasterDegree")]
        public int? DegreeID { get; set; }
        [StringLength(100)]
        public string EmployeeID { get; set; }
        public decimal PayRate { get; set; }
        public decimal FTFpayRate { get; set; }
        public decimal? FollowUpPayRate { get; set; }
        public decimal? UrgentCarePayRate { get; set; }
        public int? FollowUpDays { get; set; }
        public int? TimeInterval { get; set; }

        [NotMapped]
        public string DegreeName
        {
            get
            {
                try
                {
                    return MasterDegree != null?MasterDegree.DegreeName:null;
                }
                catch (Exception)
                {

                    return string.Empty;
                }
            }
            set { }
        }

        [NotMapped]
        public string GenderName { get; set; }
        [NotMapped]
        public List<StaffLocationModel> StaffLocationList { get; set; }
        [NotMapped]
        public List<StaffTeamModel> StaffTeamList { get; set; }
        [NotMapped]
        public List<StaffTagsModel> StaffTagsModel { get; set; }
        [NotMapped]
        public List<StaffSpecialityModel> StaffSpecialityModel { get; set; }

        [NotMapped]
        public List<StaffCareCategoryModel> StaffCareCategoryModel { get; set; }
        [NotMapped]
        public List<StaffTaxonomyModel> StaffTaxonomyModel { get; set; }
        [NotMapped]
        public List<StaffQualificationModel> StaffQualificationModels { get; set; }
        [NotMapped]
        public List<StaffAwardModel> StaffAwardModels { get; set; }
        [NotMapped]
        public List<StaffExperienceModel> StaffExperienceModels { get; set; }
        [NotMapped]
        public List<StaffServicesModel> StaffServicesModels { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual MasterGender MasterGender { get; set; }
        public virtual MasterCountry MasterCountry { get; set; }
        public virtual MasterState MasterState { get; set; }
        public virtual List<StaffTags> StaffTags { get; set; }
        public virtual MasterDegree MasterDegree { get; set; }
        public virtual UserRoles UserRoles { get; set; }
        public virtual User Users3 { get; set; }
        public virtual List<StaffLocation> StaffLocation { get; set; }
        public virtual List<StaffTeam> StaffTeam { get; set; }
        public virtual List<StaffAvailability> StaffAvailability { get; set; }
        public virtual List<StaffSpeciality> StaffSpecialities { get; set; }
        public virtual List<StaffCareCategories> StaffCareCategories { get; set; }
        public virtual List<StaffTaxonomy> StaffTaxonomies { get; set; }
        public virtual List<StaffServices> StaffServices { get; set; }
        public virtual List<StaffExperience> StaffExperiences { get; set; }
        public virtual List<StaffQualification> StaffQualifications { get; set; }
        public virtual List<StaffAward> StaffAwards { get; set; }
        public virtual GlobalCode EmployeeType { get; set; }
        public virtual PayrollGroup PayrollGroup { get; set; }
        public virtual List<StaffPayrollRateForActivity> StaffPayrollRateForActivity { get; set; }
        [Column(TypeName = "varchar(max)")]
        public string AboutMe { get; set; }
        public virtual List<ProviderCancellationRules> ProviderCancellationRules { get; set; }

    }
}

