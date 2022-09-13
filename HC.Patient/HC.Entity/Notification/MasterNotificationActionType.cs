using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterNotificationActionType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public int ID { get; set; }
        public int? TypeId { get; set; }
        public int? SubTypeId { get; set; }
        [ForeignKey("Organization")]
        public int OrganizationID { get; set; }
        public virtual Organization Organization { get; set; }

    }
}
