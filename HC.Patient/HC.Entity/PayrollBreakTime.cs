using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HC.Patient.Entity
{
    public class PayrollBreakTime : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        [Required]
        public decimal Duration { get; set; }
        [Required]
        [ForeignKey("MasterState")]
        public int StateId { get; set; }
        [Required]
        [ForeignKey("Organization")]
        public int OrganizationId { get; set; }
        [Obsolete]
        public virtual Organization Organization { get; set; }
        public virtual MasterState MasterState{ get; set; }
    }
}
