using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterTemplateCategory:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string MasterCategoryName { get; set; }

        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
