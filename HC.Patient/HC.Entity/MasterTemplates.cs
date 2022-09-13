using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterTemplates : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TemplateName { get; set; }

        [Column(TypeName = "text")]
        public string TemplateJson { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }

        [ForeignKey("MasterTemplateCategory")]
        public int? TemplateCategoryId { get; set; }
        [ForeignKey("MasterTemplateSubCategory")]
        public int? TemplateSubCategoryId { get; set; }
        public int? DisplayOrder { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual MasterTemplateCategory MasterTemplateCategory { get; set; }
        public virtual MasterTemplateSubCategory MasterTemplateSubCategory { get; set; }
    }
}