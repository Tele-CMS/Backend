using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterNotificationType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Type { get; set; }
        [Column(TypeName = "varchar(250)")]
        public string Description { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

    }
}
