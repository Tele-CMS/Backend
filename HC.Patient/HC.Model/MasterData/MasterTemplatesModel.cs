namespace HC.Patient.Model.MasterData
{
    public class MasterTemplatesModel
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateJson { get; set; }
        public int? TemplateCategoryId { get; set; }
        public int? TemplateSubCategoryId { get; set; }
        public string TemplateCategoryName { get; set; }
        public string TemplateSubCategoryName { get; set; }
        public decimal TotalRecords { get; set; }
    }
    public class MasterTemplateCategoryModel
    {
        public int Id { get; set; }
        public string MasterCategoryName { get; set; }
        public int OrganizationId { get; set; }
    }
    public class MasterTemplateSubCategoryModel
    {
        public int Id { get; set; }
        public string MasterSubCategoryName { get; set; }
        public string MasterSubCategoryValue { get; set; }
        public int MasterTemplateCategoryId { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
