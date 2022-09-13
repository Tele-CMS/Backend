using HC.Patient.Model.Payment;
using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Patient.Model
{
    public class AppointmentModelList
    {
        public List<AppointmentModel> AppointmentModels { get; set; }
        public List<AppointmentAvailabilityModel> Availabilities { get; set; }
        public List<AppointmentSpecialitiesModel> Specialities { get; set; }
        public List<AppointmentExperienceModel> Experiences { get; set; }
        public List<AppointmentTaxonomyModel> Taxonomies { get; set; }

    }

  

    public class AppointmentModel
    {
        public string ProviderId { get; set; }
        public int StaffId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public decimal PayRate { get; set; }
        public decimal FTFpayRate { get; set; }
        public string ProviderImage { get; set; }
        public string ProviderImageThumbnail { get; set; }
        public string Speciality { get; set; }
        public List<string> Taxonomy { get; set; }
        public int TotalRecords { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string TotalExperience { get; set; }
        public int? Average { get; set; }
        public int? TotalReviews { get; set; }
        public int? FollowUpDays { get; set; }
        public string keyword { get; set; }
        public decimal? FollowUpPayRate { get; set; }
        public decimal? UrgentCarePayRate { get; set; }
        public bool IsOnline { get; set; }
        public List<AppointmentAvailabilityModel> Availabilities { get; set; }
        public List<AppointmentSpecialitiesModel> Specialities { get; set; }
        public List<AppointmentExperienceModel> Experiences { get; set; }
        public List<AppointmentTaxonomyModel> Taxonomies { get; set; }
        public List<CancelationRuleModel> CancelationRules { get; set; }
        

    }

    public class AppointmentSpecialitiesModel
    {
        public int Id { get; set; }
        public string Speciality { get; set; }
        public int StaffId { get; set; }
    }
    public class AppointmentTaxonomyModel
    {
        public int Id { get; set; }
        public string Taxonomy { get; set; }
        public int StaffId { get; set; }
    }
    public class AppointmentExperienceModel
    {
        public int Id { get; set; }
        public string TotalExperience { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Organization { get; set; }
        public int StaffId { get; set; }

    }
    public class AppointmentAvailabilityModel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Day { get; set; }
        public int StaffId { get; set; }

    }

}
