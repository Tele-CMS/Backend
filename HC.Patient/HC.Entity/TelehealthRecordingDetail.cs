using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class TelehealthRecordingDetail : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [ForeignKey("TelehealthSessionDetails")]
        public int TelehealthSessionDetailID { get; set; }

        public string ArchiveId { get; set; }      

        public virtual TelehealthSessionDetails TelehealthSessionDetails { get; set; }

        [ForeignKey("Organization")]
        public int? OrganizationId { get; set; }
        public Organization Organization { get; set; }
    }
}
