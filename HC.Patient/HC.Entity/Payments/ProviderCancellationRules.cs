using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HC.Patient.Entity.Payments
{
    public class ProviderCancellationRules
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Staffs")]
        public int StaffId { get; set; }

        public int UptoHour { get; set; }
        public int RefundPercentage { get; set; }
        public virtual Staffs Staffs { get; set; }
    }
}
