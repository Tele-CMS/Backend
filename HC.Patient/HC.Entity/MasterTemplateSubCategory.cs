using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterTemplateSubCategory:BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MasterSubCategoryName { get; set; }

        public string MasterSubCategoryValue { get; set; }

        [ForeignKey("MasterTemplateCategory")]
        public int MasterTemplateCategoryId { get; set; }
        public int? DisplayOrder { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        public virtual MasterTemplateCategory MasterTemplateCategory { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
