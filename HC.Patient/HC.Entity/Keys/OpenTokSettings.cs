using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public  class OpenTokSettings:BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string APIKey { get; set; }
        public string APISecret { get; set; }
        public string APIUrl { get; set; }
        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
