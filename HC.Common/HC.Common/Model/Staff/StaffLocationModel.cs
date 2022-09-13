namespace HC.Common.Model.Staff
{
    public class StaffLocationModel
    {
        public int Id { get; set; } // LocationID
        public bool IsDefault { get; set; }
    }

    public class StaffTeamModel
    {
        public int id { get; set; }
        public int staffid { get; set; }
        public int staffteamid { get; set; }
        public bool isdeleted { get; set; }
    }

    public class StaffTagsModel
    {
        public int Id { get; set; }
        public int StaffID { get; set; }
        public int TagID { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class StaffSpecialityModel
    {
        public int Id { get; set; }
        public int StaffID { get; set; }
        public int SpecialityId { get; set; }
        public string Speciality { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class StaffCareCategoryModel
    {
        public int Id { get; set; }
        public int StaffID { get; set; }
        public int healthcarecategoryID { get; set; }
        public string carecategoryname { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class StaffTaxonomyModel
    {
        public int Id { get; set; }
        public int StaffID { get; set; }
        public int TaxonomyId { get; set; }
        public string TaxonomyName { get; set; }
        public string TaxonomyValue { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class StaffQualificationModel
    {
        public string Id { get; set; }
        public string Course { get; set; }
        public string University { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsDeleted { get; set; }
        public string StaffId { get; set; }
    }
    public class StaffExperienceModel
    {
        public string Id { get; set; }
        public string OrganizationName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TotalExperience { get; set; }
        public bool IsDeleted { get; set; }
        public string StaffId { get; set; }
    }
    public class StaffAwardModel
    {
        public string Id { get; set; }
        public string AwardType { get; set; }
        public string AwardDate { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public string StaffId { get; set; }
    }

    public class StaffServicesModel
    {
        public string Id { get; set; }
        public string ServiceId { get; set; }
        public bool IsDeleted { get; set; }
        public string StaffId { get; set; }
        public string ServiceName { get; set; }
    }
}
