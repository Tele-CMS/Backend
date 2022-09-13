using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class MasterPayrollGroup : BaseEntity
    {        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("PayrollGroupID")]
        public int Id { get; set; }
        public string PayrollGroup { get; set; }
        [NotMapped]
        public string value { get { return this.PayrollGroup; } set { this.PayrollGroup = value; } }
        [Required]
        [ForeignKey("Organization")]
        public int? OrganizationID { get; set; }

        public virtual Organization Organization { get; set; }
    }

}
