using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterRelationship : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RelationshipID")]
        public int Id { get; set; }
        public string RelationshipName { get; set; }

        
        [Column(TypeName = "varchar(5)")]
        public string RelationshipCode { get; set; }

        [NotMapped]
        public string value { get { return this.RelationshipName; } set { this.RelationshipName = value; } }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }
        public int? DisplayOrder { get; set; }
        public virtual Organization Organization { get; set; }
    }
}